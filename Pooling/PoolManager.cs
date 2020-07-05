using System.Collections.Generic;
using BricksBucket.Core.Attributes;
using BricksBucket.Core.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR

#endif


namespace BricksBucket.Core.Pooling
{
    /// <summary>
    ///
    /// Pool manager.
    ///
    /// <para>
    /// Manage instances of prefabs related to the pooled component.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2018 </para>
    /// 
    /// </summary>
    public sealed class PoolManager : MonoSingleton<PoolManager>
    {

        #region Class Members

        /// <summary>Dictionary of pools.</summary>
        [SerializeField, DictionaryDrawerSettings (
            DisplayMode = DictionaryDisplayOptions.ExpandedFoldout,
            KeyLabel = "Prefab", ValueLabel = "Pool", IsReadOnly = true
        )]
        private PoolDictionary _poolsDictionary = new PoolDictionary();

        #endregion



        #region Class Accessors

        /// <summary> Gets the pool with the specified instance. </summary>
        public Pool this[PoolInstance reference]
        {
            get => Dictionary[reference];
        }

        /// <summary> Returns PoolManager instance transform. </summary>
        public static Transform Root { get => Instance.transform; }

        public Dictionary<PoolInstance, Pool> Dictionary
        {
            get => _poolsDictionary;
            private set => _poolsDictionary = new PoolDictionary ();
        }

        #endregion



        #region MonoBehaviour Overrides

        /// <summary>
        /// Called on destroy.
        /// </summary>
        private void OnApplicationQuit ()
        {
            foreach (Pool pool in Dictionary.Values)
                pool.LogOverRequest ();
        }

        #endregion



        #region Pool Managing Methods

        /// <summary>
        /// Gets the pool for the passed instance.
        /// </summary>
        /// <returns>The pool.</returns>
        /// <param name="instance">Instance reference.</param>
        private static Pool GetPool (PoolInstance instance)
        {
            //  Return if the passed instance is null.
            if (instance == null)
                throw Utils.NullInstanceException ();

            //  If it is a prefab with already a pool return its pool.
            if (Instance.Dictionary.ContainsKey (instance))
                return Instance.Dictionary[instance];

            //  Return the pool if it have any.
            if (instance.Pool != null)
                if (Instance.Dictionary.ContainsValue (instance.Pool))
                    return instance.Pool;

            //  Return a pool of its prefab.
            if (instance.Prefab != null)
            {
                //  If its source alrady have a prefab return its pool.
                if (Instance.Dictionary.ContainsKey (instance.Prefab))
                    return Instance.Dictionary[instance.Prefab];

                // Create a pool for its source.
                Pool pool = AddPool (instance.Prefab);
                instance.Pool = pool;
                return pool;
            }

            //  Consider the instance as a prefab. Returns its pool.
            return AddPool (instance);
        }

        /// <summary>
        /// Return wether or not the manager contains the specified pool.
        /// </summary>
        /// <param name="prefab">Prefab to look for.</param>
        /// <returns>Wether or not the manager contains a pool.</returns>
        public static bool ContainsPool (PoolInstance prefab)
        {
            return Instance.Dictionary.ContainsKey (prefab);
        }

        //  Adds a new pool from a prefab.
        public static Pool AddPool (PoolInstance prefab)
        {
            if (prefab == null)
                return null;

            if (ContainsPool (prefab))
                return Instance[prefab];

            //  Creates the pool.
            Pool pool = new Pool (prefab);
            Instance.Dictionary.Add (prefab, pool);
            return pool;
        }

        /// <summary>
        /// Deletes the specified pool.
        /// </summary>
        /// <param name="prefab">Prefab key to find pool.</param>
        /// <param name="useGC">If <c>true</c> Use Garbage Collector.</param>
        public static void RemovePool (PoolInstance prefab, bool useGC = true)
        {
            if (ContainsPool (prefab))
            {
                Instance.Dictionary[prefab].DestroyAll ();
                Instance.Dictionary.Remove (prefab);

                if (useGC)
                    System.GC.Collect ();
            }
        }

        /// <summary>
        /// Destroys all pools.
        /// </summary>
        /// <param name="useGC">If <c>true</c> Use Garbage Collector.</param>
        public static void DestroyAll (bool useGC = true)
        {
            foreach (Pool pool in Instance.Dictionary.Values)
                pool.DestroyAll ();
            Instance.Dictionary.Clear ();

            if (useGC)
                System.GC.Collect ();
        }

        #endregion



        #region Spawn and Dispose Methods

        /// <summary>
        /// Spawns a new instance of the specified prefab.
        /// </summary>
        /// <returns>The spawned instance.</returns>
        /// <param name="prefab">Prefab reference to clone.</param>
        /// <param name="spawner">Spawner who call this method.</param>
        public static PoolInstance
        Spawn (PoolInstance prefab, Component spawner = null)
        {
            if (prefab == null)
                throw Utils.NullPrefabException ();

            return GetPool (prefab).Spawn (spawner);
        }

        /// <summary>
        /// Spawns a new instance with the specified delay.
        /// </summary>
        /// <param name="delay">Delay to spawn.</param>
        /// <param name="prefab">Prefab reference to clone.</param>
        /// <param name="spawner">Spawner who call this method.</param>
        /// <param name="callback">Callback to assign clone.</param>
        /// <returns>Returns the coroutine.</returns>
        public static Coroutine SpawnDelayed (
            float delay,
            PoolInstance prefab,
            Component spawner = null,
            System.Action<PoolInstance> callback = null
        ) => Instance.StartCoroutine (
            Utils.DelayAction (
                () => {
                    var instance = Spawn (prefab, spawner);
                    if (callback != null)
                        callback.Invoke (instance);
                },
                delay,
                prefab.UseScaledTime
            )
        );

        /// <summary>
        /// Spawns a new instance of the specified prefab.
        /// </summary>
        /// <returns>The spawned instance.</returns>
        /// <param name="prefab">Prefab reference to clone.</param>
        /// <param name="position">Position where to spawn the instance.</param>
        /// <param name="rotation">Rotation to use.</param>
        /// <param name="parent">Transform where to parent the instance.</param>
        /// <param name="spawner">Spawner who calls this method.</param>
        public static PoolInstance
        SpawnAt (
            PoolInstance prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            Component spawner = null
        ) {
            if (prefab == null)
                throw Utils.NullPrefabException ();

            return GetPool (prefab).SpawnAt (
                position: position,
                rotation: rotation,
                parent: parent,
                spawner: spawner
            );
        }

        /// <summary>
        /// Spawns a new instance with the specified delay.
        /// </summary>
        /// <param name="delay">Delay to spawn.</param>
        /// <param name="prefab">Prefab reference to clone.</param>
        /// <param name="position">Position where to spawn the instance.</param>
        /// <param name="rotation">Rotation to use.</param>
        /// <param name="parent">Transform where to parent the instance.</param>
        /// <param name="spawner">Spawner who calls this method.</param>
        /// <param name="callback">Callback to assign clone.</param>
        /// <returns>Returns the coroutine.</returns>
        public static Coroutine SpawnAtDelayed(
            float delay,
            PoolInstance prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            Component spawner = null,
            System.Action<PoolInstance> callback = null
        ) => Instance.StartCoroutine(
            Utils.DelayAction(
                () =>
                {
                    var instance = SpawnAt(
                        prefab,
                        position,
                        rotation,
                        parent,
                        spawner
                    );
                    if (callback != null)
                        callback.Invoke(instance);
                },
                delay,
                prefab.UseScaledTime
            )
        );

        /// <summary>
        /// Disposes of the specified intance.
        /// </summary>
        /// <param name="instance">Instance to be disposed.</param>
        public static void Dispose (PoolInstance instance)
        {
            if (instance == null)
                throw Utils.NullInstanceException ();

            var pool = GetPool (instance);
            pool.Dispose (instance);
        }

        /// <summary>
        /// Disposes of the specified gameObject.
        /// </summary>
        /// <param name="gameObject">Game object to be despawned.</param>
        /// <returns> True if the gameObject has been despawned. </returns>
        public static bool Dispose (GameObject gameObject)
        {

            if (gameObject == null)
                throw Utils.NullInstanceException ();

            PoolInstance instance = gameObject.GetComponent<PoolInstance> ();
            if (instance)
            {
                Dispose (instance);
                return true;
            }

            // TODO: Implement log method in PoolManager.Dispose.
            /*
            DebugUtils.InternalExtendedLog (
                layer: LogLayer.Logistics,
                type: LogType.Error,
                context: gameObject,
                format: "The game object {0} has not a pooled component.",
                data: gameObject.name
            );
            */

            return false;
        }

        /// <summary>
        /// Disposes of the specified instance on the specified delay.
        /// </summary>
        /// <param name="instance">Instance to be disposed.</param>
        /// <param name="delay">Delay to call dispose.</param>
        public static Coroutine
        DisposeDelayed (PoolInstance instance, float delay)
        {
            return Instance.StartCoroutine (
                Utils.DelayAction (
                    () => { Dispose (instance); },
                    delay,
                    instance.UseScaledTime
                )
            );
        }

        /// <summary>
        /// Dispose of all pooled objects.
        /// </summary>
        public static void DisposeAll ()
        {
            foreach (Pool pool in Instance.Dictionary.Values)
                pool.DisposeAll ();
        }

        #endregion



        #region Editor Methods

        /// <summary>
        /// The prefab source of a prefab instance.
        /// </summary>
        /// <param name="instance">Instance which prefab must be found.</param>
        /// <returns>Prefab of the instance.</returns>
        internal static PoolInstance FindSource (PoolInstance instance)
        {
#if UNITY_EDITOR
            var gameObject = instance.gameObject;
            var status = PrefabUtility.GetPrefabInstanceStatus (gameObject);
            if (status == PrefabInstanceStatus.NotAPrefab)
                return null;

            switch (PrefabUtility.GetPrefabAssetType (instance))
            {
                //  Change the source reference for its original source.
                case PrefabAssetType.Regular:
                case PrefabAssetType.Variant:
                case PrefabAssetType.Model:
                GameObject sourceGO = GetPrefabSource (instance.gameObject);
                return sourceGO.GetComponent<PoolInstance> ();
            }
#endif
            return null;
        }

#if UNITY_EDITOR

        /// <summary>
        /// This method returns the prefab source until the most prefab
        /// instance root, this means that will look trough nested prefabs
        /// until find the origin of this instance.
        /// </summary>
        /// <param name="instance">Instance Attempt.</param>
        /// <returns>Prefab Game Object.</returns>
        private static GameObject GetPrefabSource (GameObject instance)
        {
            GameObject source = PrefabUtility.GetCorrespondingObjectFromSource (
                componentOrGameObject: instance
            );
            if (source == null)
                return instance;

            if (instance == PrefabUtility.GetOutermostPrefabInstanceRoot (
                componentOrGameObject: instance
            ))
                return source;
            return GetPrefabSource (source);
        }

        /// <summary>
        /// Called on save to save all instances on scene.
        /// </summary>
        [OnSave]
        private void AddSceneInstances ()
        {
            //  Load References on scene.
            var scene = SceneManager.GetActiveScene ();
            var roots = scene.GetRootGameObjects ();
            for (int i = 0; i < roots.Length; i++)
            {
                var children = roots[i].GetComponentsInChildren<PoolInstance> (
                    includeInactive: true
                );
                for (int j = 0; j < children.Length; j++)
                {
                    var instance = children[j];

                    if (instance.Pool == null || instance.Pool.Root == null)
                    {
                        var pool = GetPool (instance.Prefab);
                        instance.Pool = pool;

                        bool add =
                            !pool.Stack.Contains (instance) &&
                            !pool.Spawned.Contains (instance);

                        if (add)
                            pool.Spawned.Add (instance);
                    }

                }
            }

            //  Remove from list null references.
            foreach (var pair in Dictionary)
            {
                var pool = pair.Value;

                if (pool != null)
                {
                    if (pool.Spawned != null)
                    {
                        var spawned = pool.Spawned.ToArray ();
                        for (int i = 0; i < spawned.Length; i++)
                            if (spawned[i] == null)
                                pool.Spawned.Remove (spawned[i]);
                    }

                    if (pool.Stack != null)
                    {
                        var stack = pool.Stack;
                        var stackArray = stack.ToArray ();
                        for (int i = 0; i < stackArray.Length; i++)
                            if (stackArray[i] == null)
                                stack.Remove (stackArray[i]);
                    }
                }
                else
                    Dictionary.Remove (pair.Key);
            }

        }

#endif

        #endregion



        #region Nested Classes

        /// <summary>
        ///
        /// Pool Dictionary.
        ///
        /// <para>
        /// Serializable dictionary of pool instances and pools.
        /// </para>
        /// 
        /// </summary>
        [System.Serializable]
        private class PoolDictionary :
        SerializableDictionary<PoolInstance, Pool> { }

        #endregion

    }
}
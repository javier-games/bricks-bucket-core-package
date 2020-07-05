using System.Collections.Generic;
using BricksBucket.Core.Collections;
using UnityEngine;

#if UNITY_EDITOR
using PrefabUtility = UnityEditor.PrefabUtility;
#endif

namespace BricksBucket.Core.Pooling
{
    /// <summary>
    ///
    /// Pool.
    ///
    /// <para>
    /// Pool of instances managed by the PoolManager Class.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2018 </para>
    /// 
    /// </summary>
    [System.Serializable]
    public sealed class Pool
    {

        #region Class Members

        /// <summary>
        /// Reference to the prefab to use to create instances.
        /// </summary>
        [Tooltip ("Prefab to generate instances.")] [SerializeField]
        private PoolInstance _prefab;

        /// <summary>
        /// Reference to the transform to contains instances on stock.
        /// </summary>
        [Tooltip ("Root of the pools stock.")] [SerializeField]
        private Transform _root;

        /// <summary>
        /// List of reference to spawned instances.
        /// </summary>
        [Space, Tooltip ("List of the instances spawned.")] [SerializeField]
        private List<PoolInstance> _spawned = new List<PoolInstance> ();

        /// <summary>
        /// Stack of usable instances.
        /// </summary>
        [Space, Tooltip ("Stack of the instances waiting to spawn.")]
        [SerializeField]
        private PoolInstanceStack _stack = new PoolInstanceStack ();

        #endregion



        #region Class Accessors

        /// <summary> Gets the count of instances in the pool. </summary>
        public int InstanceCount => SpawnedCount + StackCount;

        /// <summary> Gets the count of instances spawned. </summary>
        public int SpawnedCount => _spawned.Count;

        /// <summary> Gets the count of instances on stack. </summary>
        public int StackCount => _stack.Count;

        public List<PoolInstance> Spawned => _spawned;

        public PoolInstanceStack Stack => _stack;

        /// <summary> Gets the prefab of the pool. </summary>
        public PoolInstance Prefab
        {
            get => _prefab;
            private set => _prefab = value;
        }

        /// <summary> Gets the parent. </summary>
        public Transform Root
        {
            get => _root;
            private set => _root = value;
        }

        /// <summary> Amount of over requested instances. </summary>
        private uint OverRequestedInstancesAmount { get; set; }

        #endregion



        #region Constructor

        /// <summary>
        /// Initializes a new instance af a pool. This method has to be called
        /// for the Game Manager.
        /// </summary>
        /// <param name="prefab">Prefab to use as reference.</param>
        internal Pool (PoolInstance prefab)
        {
            Prefab = prefab
                ? prefab
                : throw Utils.NullPrefabException ();

            Root = new GameObject (
                name: string.Concat (prefab.name, " Pool")
            ).transform;

            Root.SetParent (PoolManager.Root);
            Root.position = Vector3.zero;

            AllocateInstance (prefab.Amount);
        }

        #endregion



        #region Class Implementation

        #region Instance Validation

        /// <summary> Indicates if the instance is on the stack. </summary>
        public bool OnStack (PoolInstance instance) =>
            _stack.Contains (instance);

        /// <summary> Indicates if the instance did spawned. </summary>
        public bool DidSpawned (PoolInstance instance) =>
            _spawned.Contains (instance);

        /// <summary> Contains the specified instance. </summary>
        public bool Contains (PoolInstance instance) =>
            OnStack (instance) || DidSpawned (instance);

        #endregion

        /// <summary> Allocates the indicated amount of instances. </summary>
        /// <param name="count">Count.</param>
        private void AllocateInstance (uint count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                var instance =

#if UNITY_EDITOR
                    !Application.isPlaying
                        ? PrefabUtility.InstantiatePrefab (Prefab) as
                            PoolInstance
                        :
#endif
                        Object.Instantiate (
                            original: Prefab.gameObject,
                            position: Vector3.zero,
                            rotation: Quaternion.identity
                        ).GetComponent<PoolInstance> ();

                if (instance == null)
                    continue;

                _stack.Push (instance);
                instance.Pool = this;
                instance.Prefab = Prefab;
                instance.ApplyDispose ();
            }
        }

        /// <summary>
        /// Spawn an instance.
        /// </summary>
        /// <returns>The spawned instance.</returns>
        /// <param name="spawner">Spawner who's call this method.</param>
        public PoolInstance Spawn (Component spawner = null) =>
            SpawnAt (Vector3.zero, Quaternion.identity, null, spawner);

        /// <summary>
        /// Spawns an instance at the specified parameters.
        /// </summary>
        /// <returns>The spawned instance.</returns>
        /// <param name="position">Position to locate instance.</param>
        /// <param name="rotation">Rotation to sets instance transform.</param>
        /// <param name="parent">Parent to attach the instance.</param>
        /// <param name="spawner">Spawner who's call this method.</param>
        public PoolInstance SpawnAt (
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            Component spawner = null
        )
        {

            if (_stack.Count == 0)
            {
                if (_prefab.IsExpandable)
                {

                    AllocateInstance ();
                    OverRequestedInstancesAmount++;
                }
                else
                    return null;
            }

            //  Spawning the first element in the list.
            PoolInstance instance = _stack.Pop ();
            _spawned.Add (instance);
            instance.ApplySpawn (position, rotation, parent, spawner);

            return instance;
        }

        /// <summary> Disposes of the specified instance. </summary>
        /// <param name="instance">Instance to dispose.</param>
        public void Dispose (PoolInstance instance)
        {

            //  Avoid adding missing instances.
            if (instance == null)
                throw Utils.NullInstanceException ();

            //  Avoid adding instances from other pools.
            if (instance.Pool != this)
            {
                // TODO: Implement Log Method in Pool.Dispose.
                /*
                DebugUtils.InternalExtendedLog (
                    layer: LogLayer.Logistics,
                    type: LogType.Error,
                    context: instance,
                    format: string.Concat (
                        "The instance {0} does not belongs to the pool of ",
                        "{1} prefab. You should try call manage the instance ",
                        "with the PoolManager or the instance it self."
                    ),
                    data: new object[] {instance.name, Prefab.name}
                );
                */
                return;
            }

            //  Then the prefab belongs to this pool and the instance has
            //  been spawned by this pool dispose of it.
            if (DidSpawned (instance))
                _spawned.Remove (instance);

            _stack.Push (instance);
            instance.ApplyDispose ();
        }

        /// <summary> Disposes of all. </summary>
        public void DisposeAll ()
        {
            while (_spawned.Count > 0)
            {
                if (_spawned[0] != null)
                    Dispose (_spawned[0]);
                else
                    _spawned.RemoveAt (0);
            }
        }

        /// <summary> Clear this pool. </summary>
        /// <param name="useGarbageCollector">Use Garbage Collector.</param>
        public void DestroyAll (bool useGarbageCollector = true)
        {
            DisposeAll ();
            while (_stack.Count > 0)
            {
                var instance = _stack.Pop ();
                if (instance == null || instance.gameObject == null)
                    continue;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    Object.DestroyImmediate (instance.gameObject);
                else
#endif
                    Object.Destroy (instance.gameObject);
            }
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate (_root.gameObject);
            else
#endif
                Object.Destroy (_root.gameObject);
            _root = null;

            if (useGarbageCollector)
                System.GC.Collect ();
        }

        /// <summary>
        /// Removes this pool from pool manager and/or destroys this instance.
        /// <param name="useGarbageCollector"> Use Garbage Collector.</param>
        /// </summary>
        public void Remove (bool useGarbageCollector = false)
        {
            if (PoolManager.ContainsPool (_prefab))
                PoolManager.RemovePool (_prefab);

            else
                DestroyAll (useGarbageCollector);
        }

        /// <summary> Log the amount of over request instances. </summary>
        public void LogOverRequest ()
        {
            if (OverRequestedInstancesAmount > 0)
            {
                // TODO: Implement Log method in Pool.LgOverRequest.
                /*
                DebugUtils.InternalExtendedLog (
                    layer: LogLayer.Logistics,
                    type: LogType.Error,
                    context: null,
                    format: string.Concat (
                        "Over Request Alert: {0} extra instances of ",
                        "\"{1}\" prefab has been requested during the game."
                    ),
                    data: new object[]
                    {
                        OverRequestedInstancesAmount, Prefab.name
                    }
                );
                */
            }
        }

        #endregion



        #region Nested Classes

        /// <summary>
        ///
        /// PoolInstances Stack.
        ///
        /// <para>
        /// Serializable stack of pool instances.
        /// </para>
        /// 
        /// </summary>
        [System.Serializable]
        public class PoolInstanceStack : SerializableStack<PoolInstance> { }

        #endregion
    }
}
using UnityEngine;

namespace Monogum.BricksBucket.Core
{
    /// <!-- MonoSingleton -->
    /// 
    /// <summary>
    /// Generic singleton that inherits from MonoBehaviour.
    /// </summary>
    /// 
    /// <typeparam name="T">Heiress Class Type.</typeparam>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    [DisallowMultipleComponent]
    public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoSingleton<T>
    {
        #region Fields

        /// <summary>
        /// Instance of the singleton.
        /// </summary>
        private static T _instance;

        #endregion


        #region Properties

        /// <summary>
        /// Flagged true after awake.
        /// </summary>
        /// <returns><value>True</value> after Awake.</returns>
        // ReSharper disable once StaticMemberInGenericType
        protected static bool DidAwoken { get; private set; }

        /// <summary>
        /// Flagged true after on destroy.
        /// </summary>
        /// <returns><value>True</value> after OnDestroy.</returns>
        // ReSharper disable once StaticMemberInGenericType
        protected static bool DidDestroyed { get; private set; }
        
        /// <summary>
        /// Indicates whether this has an instance or not.
        /// </summary>
        /// <returns><value>True</value> if the singleton has been assigned
        /// </returns>
        public static bool InstanceExist => _instance != null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>Null if the instance has been destroyed.</returns>
        public static T Instance
        {
            get
            {
                //  Return null if the instances has been already destroyed.
                if (DidDestroyed)
                {
                    // TODO: Add Log on MonoSingleton.Instance property.
                    Debug.LogWarning (
                        $"{typeof(T)} singleton has been already destroyed."
                    );
                    return null;
                }

                //  Return InstanceForced if this has not passed for Awake.
                if (!DidAwoken && !InstanceExist)
                    return InstanceForced;
                
                return _instance;
            }
        }

        /// <summary>
        /// Gets the instance forced to not return null.
        /// </summary>
        /// <returns>Always returns a value.</returns>
        public static T InstanceForced
        {
            get
            {
                //  Return this instance.
                if (InstanceExist)
                    return _instance;
                
                // TODO: Add Log on MonoSingleton.InstanceForced property.
                Debug.LogWarning ($"{typeof (T)} singleton has been forced.");

                /* If a singleton has to be forced means that it is called
                 * from another awake in other MonoBehaviour class before
                 * this singleton is initialized or the execution order is
                 * wrong.
                 * 
                 * Try to call this singleton from Start method or use
                 * Instance Forced property instead if it is necessary.
                 * 
                 * Check out the Unity's execution order of events:
                 * https://docs.unity3d.com/Manual/ExecutionOrder.html
                 */

                //  Find class in hierarchy.
                _instance = FindObjectOfType (typeof (T)) as T;

                if (FindObjectsOfType (typeof (T)).Length > 1)
                {
                    // TODO: Add Log on MonoSingleton.InstanceForced property.
                    Debug.LogWarning (
                        $"Various instances of {typeof (T)} singleton found."
                    );
                }

                if (InstanceExist)
                    return _instance;

                //  Find prefab in resources folder.
                var prefab = Resources.Load<T> (typeof (T).Name);
                if (prefab != null)
                {
                    _instance = Instantiate (
                        original: prefab.gameObject
                    ).GetComponent<T> ();
                    _instance.name = typeof (T).Name;
                    return _instance;
                }

                //  Create an instance.
                _instance = new GameObject (
                    name: typeof (T).Name,
                    components: typeof (T)
                ).GetComponent<T> ();
                return _instance;
            }
        }

        #endregion


        #region MonoBehaviour Methods

        /// <summary> Called on awake. </summary>
        protected virtual void Awake ()
        {
            if (_instance != null && this != _instance)
                Destroy (gameObject);
            else
            {
                _instance = this as T;
                DidAwoken = true;
                DidDestroyed = false;
            }
        }

        /// <summary> Called on destroy. </summary>
        protected virtual void OnDestroy ()
        {
            _instance = null;
            DidAwoken = false;
            DidDestroyed = true;
        }

        #endregion
    }
}
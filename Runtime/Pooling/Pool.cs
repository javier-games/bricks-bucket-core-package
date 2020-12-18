using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable IntroduceOptionalParameters.Global

namespace Monogum.BricksBucket.Core.Pooling
{
	/// <summary>
	/// Maintains a pool of objects.
	/// </summary>
	public class Pool<T>
	{
		/// <summary>
		/// Our factory function.
		/// </summary>
		protected Func<T> Factory;

		/// <summary>
		/// Our resetting function.
		/// </summary>
		protected readonly Action<T> Reset;

		/// <summary>
		/// A list of all Available items.
		/// </summary>
		protected readonly List<T> Available;

		/// <summary>
		/// A list of all items managed by the pool.
		/// </summary>
		protected readonly List<T> All;

		/// <summary>
		/// Create a new pool with a given number of starting elements.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.</param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public Pool(Func<T> factory, Action<T> reset, int initialCapacity)
		{
			Available = new List<T>();
			All = new List<T>();
			Factory = factory ?? throw new ArgumentNullException(
				nameof(factory)
			);
			Reset = reset;

			if (initialCapacity > 0)
			{
				Grow(initialCapacity);
			}
		}

		/// <summary>
		/// Creates a new blank pool.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.
		/// </param>
		public Pool(Func<T> factory)
			: this(factory, null, 0)
		{
		}

		/// <summary>
		/// Create a new pool with a given number of starting elements.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public Pool(Func<T> factory, int initialCapacity)
			: this(factory, null, initialCapacity)
		{
		}

		/// <summary>
		/// Gets an item from the pool, growing it if necessary.
		/// </summary>
		/// <returns>Gets an instance of poolable.</returns>
		public virtual T Get()
		{
			return Get(Reset);
		}

		/// <summary>
		/// Gets an item from the pool, growing it if necessary, and with a
		/// specified reset function.
		/// </summary>
		/// <param name="resetOverride">A function to use to reset the given
		/// object.</param>
		public virtual T Get(Action<T> resetOverride)
		{
			if (Available.Count == 0)
			{
				Grow(1);
			}

			if (Available.Count == 0)
			{
				throw new InvalidOperationException("Failed to grow pool");
			}

			var itemIndex = Available.Count - 1;
			var item = Available[itemIndex];
			Available.RemoveAt(itemIndex);

			resetOverride?.Invoke(item);

			return item;
		}

		/// <summary>
		/// Gets whether or not this pool contains a specified item.
		/// </summary>
		public virtual bool Contains(T pooledItem)
		{
			return All.Contains(pooledItem);
		}

		/// <summary>
		/// Return an item to the pool.
		/// </summary>
		public virtual void Return(T pooledItem)
		{
			if (All.Contains(pooledItem) &&
				!Available.Contains(pooledItem))
			{
				ReturnToPoolInternal(pooledItem);
			}
			else
			{
				throw new InvalidOperationException("Trying to return an" +
					" item to a pool that does not contain it: "
					+ pooledItem + ", " + this);
			}
		}

		/// <summary>
		/// Return all items to the pool.
		/// </summary>
		public virtual void ReturnAll()
		{
			ReturnAll(null);
		}

		/// <summary>
		/// Returns all items to the pool, and calls a delegate on each one.
		/// </summary>
		public virtual void ReturnAll(Action<T> preReturn)
		{
			for (var i = 0; i < All.Count; ++i)
			{
				var item = All[i];
				if (Available.Contains(item)) continue;
				preReturn?.Invoke(item);
				ReturnToPoolInternal(item);
			}
		}

		/// <summary>
		/// Grow the pool by a given number of elements.
		/// </summary>
		public void Grow(int amount)
		{
			for (var i = 0; i < amount; ++i)
			{
				AddNewElement();
			}
		}

		/// <summary>
		/// Returns an object to the Available list. Does not
		/// check for consistency.
		/// </summary>
		protected virtual void ReturnToPoolInternal(T element)
		{
			Available.Add(element);
		}

		/// <summary>
		/// Adds a new element to the pool.
		/// </summary>
		protected virtual T AddNewElement()
		{
			var newElement = Factory();
			All.Add(newElement);
			Available.Add(newElement);

			return newElement;
		}

		/// <summary>
		/// Dummy factory that returns the default T value.
		/// </summary>		
		protected static T DummyFactory()
		{
			return default;
		}
	}

	/// <summary>
	/// A variant pool that takes Unity components. Automatically enables
	/// and disables them as necessary.
	/// </summary>
	public class UnityComponentPool<T> : Pool<T>
	where T : Component
	{
		/// <summary>
		/// Create a new pool with a given number of starting elements.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.
		/// </param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public UnityComponentPool(Func<T> factory, Action<T> reset,
			int initialCapacity)
			: base(factory, reset, initialCapacity)
		{
		}

		/// <summary>
		/// Creates a new blank pool.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.
		/// </param>
		public UnityComponentPool(Func<T> factory)
			: base(factory)
		{
		}

		/// <summary>
		/// Create a new pool with a given number of starting elements.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public UnityComponentPool(Func<T> factory, int initialCapacity)
			: base(factory, initialCapacity)
		{
		}

		/// <summary>
		/// Retrieve an enabled element from the pool.
		/// </summary>
		public override T Get(Action<T> resetOverride)
		{
			var element = base.Get(resetOverride);

			element.gameObject.SetActive(true);

			return element;
		}

		/// <summary>
		/// Automatically disable returned object.
		/// </summary>
		protected override void ReturnToPoolInternal(T element)
		{
			if (element == null)
			{
				return;
			}

			element.gameObject.SetActive(false);

			base.ReturnToPoolInternal(element);
		}

		/// <summary>
		/// Keep newly created objects disabled.
		/// </summary>
		protected override T AddNewElement()
		{
			var newElement = base.AddNewElement();

			newElement.gameObject.SetActive(false);

			return newElement;
		}
	}

	/// <summary>
	/// A variant pool that takes Unity game objects. Automatically enables
	/// and disables them as necessary.
	/// </summary>
	public class GameObjectPool : Pool<GameObject>
	{
		/// <summary>
		/// Create a new pool with a given number of starting elements.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.
		/// </param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public GameObjectPool(Func<GameObject> factory,
			Action<GameObject> reset, int initialCapacity)
			: base(factory, reset, initialCapacity)
		{
		}

		/// <summary>
		/// Creates a new blank pool.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.
		/// </param>
		public GameObjectPool(Func<GameObject> factory)
			: base(factory)
		{
		}

		/// <summary>
		/// Create a new pool with a given number of starting elements.
		/// </summary>
		/// <param name="factory">The function that creates pool objects.
		/// </param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public GameObjectPool(Func<GameObject> factory, int initialCapacity)
			: base(factory, initialCapacity)
		{
		}

		/// <summary>
		/// Retrieve an enabled element from the pool.
		/// </summary>
		public override GameObject Get(Action<GameObject> resetOverride)
		{
			GameObject element = base.Get(resetOverride);

			element.SetActive(true);

			return element;
		}

		/// <summary>
		/// Automatically disable returned object.
		/// </summary>
		protected override void ReturnToPoolInternal(GameObject element)
		{
			element.SetActive(false);

			base.ReturnToPoolInternal(element);
		}

		/// <summary>
		/// Keep newly created objects disabled.
		/// </summary>
		protected override GameObject AddNewElement()
		{
			var newElement = base.AddNewElement();

			newElement.SetActive(false);

			return newElement;
		}
	}

	/// <summary>
	/// Variant pool that automatically instantiates objects from a given Unity
	/// game object prefab
	/// </summary>
	public class AutoGameObjectPrefabPool : GameObjectPool
	{
		/// <summary>
		/// Create our new prefab item clone.
		/// </summary>
		private GameObject PrefabFactory()
		{
			var newElement = Object.Instantiate(Prefab);
			Initialize?.Invoke(newElement);

			return newElement;
		}

		/// <summary>
		/// Our base prefab.
		/// </summary>
		protected readonly GameObject Prefab;

		/// <summary>
		/// Initialisation method for objects.
		/// </summary>
		protected readonly Action<GameObject> Initialize;

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		public AutoGameObjectPrefabPool(GameObject prefab)
			: this(prefab, null, null, 0)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialize">An initialisation function to call after
		/// creating prefabs.</param>
		public AutoGameObjectPrefabPool(GameObject prefab,
			Action<GameObject> initialize)
			: this(prefab, initialize, null, 0)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialize">An initialisation function to call after
		/// creating prefabs.</param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool.</param>
		public AutoGameObjectPrefabPool(GameObject prefab,
			Action<GameObject> initialize, Action<GameObject> reset)
			: this(prefab, initialize, reset, 0)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab with a given number of
		/// starting elements.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public AutoGameObjectPrefabPool(GameObject prefab, int initialCapacity)
			: this(prefab, null, null, initialCapacity)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialize">An initialisation function to call after
		/// creating prefabs.</param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public AutoGameObjectPrefabPool(GameObject prefab,
			Action<GameObject> initialize, Action<GameObject> reset,
			int initialCapacity)
			: base(DummyFactory, reset, 0)
		{
			// Pass 0 to initial capacity because we need to set ourselves up
			// first
			// We then call Grow again ourselves
			Initialize = initialize;
			Prefab = prefab;
			Factory = PrefabFactory;
			if (initialCapacity > 0)
			{
				Grow(initialCapacity);
			}
		}
	}

	/// <summary>
	/// Variant pool that automatically instantiates objects from a given
	/// Unity component prefab.
	/// </summary>
	public class AutoComponentPrefabPool<T> : UnityComponentPool<T>
	where T : Component
	{
		/// <summary>
		/// Create our new prefab item clone.
		/// </summary>
		T PrefabFactory()
		{
			T newElement = Object.Instantiate(Prefab);
			if (Initialize != null)
			{
				Initialize(newElement);
			}

			return newElement;
		}

		/// <summary>
		/// Our base prefab.
		/// </summary>
		protected readonly T Prefab;

		/// <summary>
		/// Initialisation method for objects.
		/// </summary>
		protected readonly Action<T> Initialize;

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		public AutoComponentPrefabPool(T prefab)
			: this(prefab, null, null, 0)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialize">An initialisation function to call after
		/// creating prefabs.</param>
		public AutoComponentPrefabPool(T prefab, Action<T> initialize)
			: this(prefab, initialize, null, 0)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialize">An initialisation function to call after
		/// creating prefabs.</param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool.</param>
		public AutoComponentPrefabPool(T prefab, Action<T> initialize,
			Action<T> reset)
			: this(prefab, initialize, reset, 0)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab with a given number of
		/// starting elements.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public AutoComponentPrefabPool(T prefab, int initialCapacity)
			: this(prefab, null, null, initialCapacity)
		{
		}

		/// <summary>
		/// Create a new pool for the given Unity prefab.
		/// </summary>
		/// <param name="prefab">The prefab we're cloning.</param>
		/// <param name="initialize">An initialisation function to call after
		/// creating prefabs.</param>
		/// <param name="reset">Function to use to reset items when retrieving
		/// from the pool.</param>
		/// <param name="initialCapacity">The number of elements to seed the
		/// pool with.</param>
		public AutoComponentPrefabPool(T prefab, Action<T> initialize,
			Action<T> reset, int initialCapacity)
			: base(DummyFactory, reset, 0)
		{
			// Pass 0 to initial capacity because we need to set ourselves
			// up first, we then call Grow again ourselves
			Initialize = initialize;
			Prefab = prefab;
			Factory = PrefabFactory;
			if (initialCapacity > 0)
			{
				Grow(initialCapacity);
			}
		}
	}
}
﻿using System.Collections.Generic;
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
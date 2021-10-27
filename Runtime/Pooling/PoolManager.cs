using System.Collections.Generic;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Pooling
{
	/// <summary>
	/// Managers a dictionary of pools, getting and returning.
	/// </summary>
	public class PoolManager : MonoSingleton<PoolManager>
	{
		/// <summary>
		/// List of poolables that will be used to initialize corresponding
		/// pools.
		/// </summary>
		public List<Poolable> poolables;

		/// <summary>
		/// Dictionary of pools, key is the prefab.
		/// </summary>
		public readonly Dictionary<Poolable, AutoComponentPrefabPool<Poolable>>
			Pools
				= new Dictionary<Poolable, AutoComponentPrefabPool<Poolable>>();

		/// <summary>
		/// Gets a poolable component from the corresponding pool.
		/// </summary>
		/// <param name="poolablePrefab"></param>
		/// <returns></returns>
		public Poolable GetPoolable(Poolable poolablePrefab)
		{
			if (!Pools.ContainsKey(poolablePrefab))
			{
				Pools.Add(poolablePrefab, new AutoComponentPrefabPool<Poolable>(
					poolablePrefab, Initialize, null,
					poolablePrefab.initialPoolCapacity));
			}

			var pool = Pools[poolablePrefab];
			var spawnedInstance = pool.Get();

			spawnedInstance.Pool = pool;
			return spawnedInstance;
		}

		/// <summary>
		/// Returns the poolable component to its component pool.
		/// </summary>
		/// <param name="poolable"></param>
		public void ReTurnPoolable(Poolable poolable)
		{
			poolable.Pool.Return(poolable);
		}

		/// <summary>
		/// Initializes the dictionary of pools.
		/// </summary>
		protected void Start()
		{
			foreach (var poolable in poolables)
			{
				if (poolable == null)
				{
					continue;
				}

				Pools.Add(
					poolable, 
					new AutoComponentPrefabPool<Poolable>(
						poolable, 
						Initialize, 
						null, 
						poolable.initialPoolCapacity
					)
				);
			}
		}

		/// <summary>
		/// initializes the pool manager.
		/// </summary>
		/// <param name="poolable">Poolable reference to set is transform.
		/// </param>
		private void Initialize(Component poolable)
		{
			poolable.transform.SetParent(transform, false);
		}
	}
}
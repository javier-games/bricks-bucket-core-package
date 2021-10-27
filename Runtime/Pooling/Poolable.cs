using UnityEngine;

namespace Monogum.BricksBucket.Core.Pooling
{
	/// <summary>
	/// Class that is to be pooled.
	/// </summary>
	public class Poolable : MonoBehaviour
	{
		/// <summary>
		/// Number of poolables the pool will initialize.
		/// </summary>
		public int initialPoolCapacity = 10;

		/// <summary>
		/// Pool that this poolable belongs to.
		/// </summary>
		public Pool<Poolable> Pool;

		/// <summary>
		/// RePool this instance, and move us under the pool manager.
		/// </summary>
		protected virtual void RePool()
		{

			transform.SetParent(PoolManager.Instance.transform, false);
			Pool.Return(this);
		}

		/// <summary>gameObject
		/// Pool the object if possible, otherwise destroy it.
		/// </summary>
		/// <param name="gameObject">GameObject attempting to pool.</param>
		public static void TryPool(GameObject gameObject)
		{
			var poolable = gameObject.GetComponent<Poolable>();
			if (poolable != null && poolable.Pool != null &&
				PoolManager.InstanceExist)
			{
				poolable.RePool();
			}
			else
			{
				Destroy(gameObject);
			}
		}

		/// <summary>
		/// If the prefab is poolable returns a pooled object otherwise
		/// instantiates a new object.
		/// </summary>
		/// <param name="prefab">Prefab of object required.</param>
		/// <typeparam name="T">Component type.</typeparam>
		/// <returns>The pooled or instantiated component.</returns>
		public static T TryGetPoolable<T>(GameObject prefab) where T : Component
		{
			var poolable = prefab.GetComponent<Poolable>();
			T instance = poolable != null && PoolManager.InstanceExist
				? PoolManager.Instance.GetPoolable(poolable).GetComponent<T>()
				: Instantiate(prefab).GetComponent<T>();
			return instance;
		}

		/// <summary>
		/// If the prefab is poolable returns a pooled object otherwise
		/// instantiates a new object.
		/// </summary>
		/// <param name="prefab">Prefab of object required.</param>
		/// <returns>The pooled or instantiated gameObject.</returns>
		public static GameObject TryGetPoolable(GameObject prefab)
		{
			var poolable = prefab.GetComponent<Poolable>();
			GameObject instance = poolable != null && PoolManager.InstanceExist
				? PoolManager.Instance.GetPoolable(poolable).gameObject
				: Instantiate(prefab);
			return instance;
		}
	}
}
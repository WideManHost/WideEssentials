using System.Collections.Generic;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// A manager that manages creating and destroying ObjectPools.
    /// Also lets you index object pools.
    /// Also lets you spawn an object from an Object Pool here, if you don't want to directly index it either.
    /// </summary>
    public class ObjectPoolManager : StaticManager<ObjectPoolManager>
    {
        private Dictionary<GameObject, ObjectPool> _prefabToPool = new Dictionary<GameObject, ObjectPool>();
        [SerializeField]
        private int initialPoolSize = 50;

        /// <summary>
        /// Spawn an object from the pool. Automatically creates a pool if one doesn't exist.
        /// </summary>
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!Instance._prefabToPool.TryGetValue(prefab, out ObjectPool pool))
            {
                GameObject poolGO = new GameObject($"{prefab.name}_Pool");
                poolGO.transform.SetParent(Instance.transform);

                pool = poolGO.AddComponent<ObjectPool>();
                pool.Initialize(prefab, Instance.initialPoolSize); // start empty; expand dynamically
                Instance._prefabToPool[prefab] = pool;
            }

            return pool.Spawn(position, rotation);
        }

        /// <summary>
        /// Returns if a ObjectPool exists of your prefab
        /// </summary>
        /// <param name="prefab">Prefab to index</param>
        /// <returns></returns>
        public static bool PoolExists(GameObject prefab)
        {
            return Instance._prefabToPool.ContainsKey(prefab);
        }

        /// <summary>
        /// Despawn an object by returning it to the pool it came from.
        /// </summary>
        public static void Despawn(GameObject instance, GameObject prefab)
        {
            if (Instance._prefabToPool.TryGetValue(prefab, out ObjectPool pool))
            {
                pool.Despawn(instance);
            }
            else
            {
                Debug.LogWarning($"No object pool found for prefab '{prefab.name}'. Destroying object instead.");
                Destroy(instance);
            }
        }

        /// <summary>
        /// Destroys all pools and clears the registry.
        /// </summary>
        public static void ClearPools()
        {
            foreach (ObjectPool pool in Instance._prefabToPool.Values)
            {
                Destroy(pool.gameObject);
            }

            Instance._prefabToPool.Clear();
        }
    }
}
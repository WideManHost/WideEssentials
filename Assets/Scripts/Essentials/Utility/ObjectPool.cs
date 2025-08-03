using System.Collections.Generic;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Class that handles storing a large amount of objects, spawning them from a pool, and despawning them from a pool to be used for later.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private List<GameObject> _pooledObjects = new List<GameObject>();
        private List<GameObject> _activeObjects = new List<GameObject>();

        private const int MAX_OBJECTS = 150;
        public int ActiveCount => _activeObjects.Count;
        public int InactiveCount
        {
            get
            {
                int count = 0;
                foreach (GameObject obj in _pooledObjects)
                {
                    if (!obj.activeSelf)
                        count++;
                }
                return count;
            }
        }

        public void Initialize(GameObject prefabToPool, int initialSize)
        {
            if (initialSize > MAX_OBJECTS)
            {
                Debug.LogWarning("Max amount of objects to create in an Object Pool is " + MAX_OBJECTS + ", Your initialSize was too large and has been lowered.");
            }
            _prefab = prefabToPool;
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = CreateNewInstance();
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }

        /// <summary>
        /// Private Method to handle the creation and setup of a pooled object.
        /// </summary>
        /// <returns></returns>
        private GameObject CreateNewInstance()
        {
            GameObject obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
            return obj;
        }

        /// <summary>
        /// Takes out an object from the pool and sets it's position and rotation to what you say.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject obj = null;

            // Try to find a disabled object
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeSelf)
                {
                    obj = _pooledObjects[i];
                    break;
                }
            }

            // If none are available, create a new one, if we have room.
            if (obj == null && _pooledObjects.Count < MAX_OBJECTS)
            {
                obj = CreateNewInstance();
                _pooledObjects.Add(obj);
            }
            else if (_pooledObjects.Count < MAX_OBJECTS)
            {
                Debug.LogWarning($"Too many {_prefab.name} Objects! Unable to create a new one when it's needed!");
            }
            else
            {
                Debug.LogWarning($"Reserved Object '{_prefab.name}' was null, Are you calling destroy on an pooled object?");
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.SetParent(null);
            obj.SetActive(true);

            if (!_activeObjects.Contains(obj))
            {
                _activeObjects.Add(obj);
            }
                

            return obj;
        }

        /// <summary>
        /// Despawns the object if it's apart of this pool.
        /// </summary>
        /// <param name="obj"></param>
        public void Despawn(GameObject obj)
        {
            if (_activeObjects.Contains(obj))
            {
                _activeObjects.Remove(obj);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
            }
        }

        /// <summary>
        /// Despawns all the objects. putting them back into their pool.
        /// </summary>
        public void DespawnAll()
        {
            foreach (GameObject obj in _activeObjects)
            {
                obj.SetActive(false);
                obj.transform.SetParent(transform);
            }
            _activeObjects.Clear();
        }       
    }
}

using UnityEngine;


namespace WideWade
{
    /// <summary>
    /// This is an abstract class for other Managers to inherit from as they should all use the same logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StaticManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Look for an existing instance of the manager in the scene
#pragma warning disable CS0618 // SHUT UP
                    _instance = FindObjectOfType<T>();
#pragma warning restore CS0618 // Type or member is obsolete

                    // If no instance is found, create a new one
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        // Ensure that the instance is destroyed if it is not the chosen one
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }

        // Virtual method that can be overridden in the derived class if needed
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }

}


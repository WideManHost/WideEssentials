using UltEvents;
using UnityEngine;


namespace WideWade
{
    /// <summary>
    /// Designer script to enstantiate an object
    /// </summary>
    public class ObjectInstantiator : MonoBehaviour
    {
        [Header("Settings")]
        public bool onStart;
        [Tooltip("If the conditions are met, we will instantiate our object")]
        public bool onConditionMet = false;
        public ConditionEvaluator conditionEvaluator;

        [Header("Instantiation Settings")]
        public GameObject toInstantiate;
        [Tooltip("How many to instantiate when called.")]
        public int instantiationCount;
        [Tooltip("Enables the instantiated object by default.")]
        public bool enableInstantiatedObject = true;
        [Tooltip("The transformation where the object will be spawned in. (if null, it will be set to this gameobject's transform)")]
        public Transform spawnTransform;
        [Tooltip("Where the instantiated object will be parented to.")]
        public Transform parentTransform;
        [Tooltip("Copy the spawn transform's position and set it to the instantiated object.")]
        public bool usesSpawnPosition;

        [Tooltip("Copy the spawn transform's rotation and set it to the instantiated object.")]
        public bool usesSpawnRotation;
        [Tooltip("Applies the rotation")]
        public bool applyRandomRotation;
        public Vector3 minRotation;
        public Vector3 maxRotation;

        [Tooltip("Spawns with scaling 1,1,1, if disabled it will not resize.")]
        public bool usesParentScaling;

        [Header("Events")]
        public UltEvent<GameObject> onObjectInstantiated;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (onStart)
            {
                // if we dont have conditional requirements then we pass, otherwise if we do lets make sure we can.
                if (!onConditionMet || (conditionEvaluator != null && conditionEvaluator.Evaluate()))
                {
                    InstantiateObject();
                }
            }
        }

        /// <summary>
        /// Instantiates the object set in this script.
        /// </summary>
        public void InstantiateObject()
        {
            if (toInstantiate == null)
            {
                Debug.LogWarning("We can't instantiate an object if there is nothing to instantiate!");
            }
            else
            {
                for (int i = 0; i < instantiationCount; i++)
                {
                    InstantiateInternal(toInstantiate);
                }
            }
        }

        /// <summary>
        /// Instantiates your inputted object instead of the one set in this script.
        /// </summary>
        /// <param name="toInstantiate"></param>
        public void InstantiateOverride(GameObject toInstantiate)
        {
            if (toInstantiate == null)
            {
                Debug.LogWarning("We can't instantiate our overrided object if there is nothing to instantiate!");
            }
            else
            {
                for (int i = 0; i < instantiationCount; i++)
                {
                    InstantiateInternal(toInstantiate);
                }
            }
        }

        /// <summary>
        /// The true mastermind method that handles instantiating.
        /// </summary>
        /// <param name="prefab"></param>
        private void InstantiateInternal(GameObject prefab)
        {
            // all my homies love conditional operators
            Transform spawnAt = spawnTransform != null ? spawnTransform : transform;
            Vector3 position = usesSpawnPosition ? spawnAt.position : prefab.transform.position;
            Quaternion rotation = usesSpawnRotation ? spawnAt.rotation : prefab.transform.rotation;

            if (applyRandomRotation)
            {
                Vector3 randomEuler = new Vector3(
                    Random.Range(minRotation.x, maxRotation.x),
                    Random.Range(minRotation.y, maxRotation.y),
                    Random.Range(minRotation.z, maxRotation.z)
                );
                rotation *= Quaternion.Euler(randomEuler);
            }

            GameObject instance = Instantiate(prefab, position, rotation);

            if (parentTransform != null)
            {
                instance.transform.SetParent(parentTransform, worldPositionStays: true);
            }

            if (usesParentScaling)
            {
                instance.transform.localScale = Vector3.one;
            }

            if (instance != null)
            {
                instance.SetActive(enableInstantiatedObject);
                onObjectInstantiated?.Invoke(instance);
            }
        }
    }
}



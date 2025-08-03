using System;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Destroys a list of objects either on start or when called.
    /// Destroys in a sequencial move.
    /// </summary>
    public class DestroyObjects : MonoBehaviour
    {
        [Header("Settings")]
        public bool onStart = true;

        public List<DestroyStage> objectsToDestroy = new List<DestroyStage>();

        [Header("Events")]
        public UltEvent onDestroyStarted;
        public UltEvent<GameObject> onTargetDestroyed;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (onStart)
            {
                Destroy();
            }
        }

        /// <summary>
        /// Destroys the gameobjects in the list.
        /// </summary>
        public void Destroy()
        {
            if (objectsToDestroy.Count > 0)
            {
                float timeDelay = 0;
                onDestroyStarted.Invoke();

                foreach (DestroyStage stage in objectsToDestroy)
                {
                    if (stage.obj != null)
                    {
                        timeDelay += stage.delay;
                        StartCoroutine(FireStage(stage, timeDelay));
                    }
                    else
                    {
                        Debug.LogError("One of the DestroyStages had a null object and has been skipped over!");
                    }
                    
                }
                objectsToDestroy.Clear();
            }
            else
            {
                Debug.LogError("No objects are in the destroy list!");
            }
            
            
        }

        private IEnumerator FireStage(DestroyStage stage, float delay)
        {
            yield return new WaitForSeconds(delay);
            onTargetDestroyed.Invoke(stage.obj);
            Destroy(stage.obj);
        }

    }

    [Serializable]
    public class DestroyStage
    {
        public GameObject obj;
        [Tooltip("how long to wait after the last destroy was called to destroy this stage.")]
        public float delay;
    }

}

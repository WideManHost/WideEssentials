using UltEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WideWade
{
    /// <summary>
    /// A class that sequentially fires events.
    /// </summary>
    public class EventSequence : MonoBehaviour
    {
        [Header("Settings")]
        public bool fireOnStart = true;

        [Tooltip("Unlike the Obejct Activator, This is done sequentially. Each event will be fired after eachother and the delay will add.")]
        public List<EventStage> eventStages = new List<EventStage>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (fireOnStart)
            {
                FireEvents();
            }
        }
        /// <summary>
        /// Fires the events in the eventStages list.
        /// </summary>
        public void FireEvents()
        {
            if (eventStages.Count > 0)
            {
                float timeDelay = 0;

                foreach (EventStage stage in eventStages)
                {
                    timeDelay += stage.fireAfterSeconds;
                    StartCoroutine(FireStage(stage, timeDelay));

                }
            }
            else
            {
                Debug.LogError("No objects are in the destroy list!");
            }
        }

        private IEnumerator FireStage(EventStage stage, float delay)
        {
            yield return new WaitForSeconds(delay);
            stage.toFire.Invoke();
        }
    }
}


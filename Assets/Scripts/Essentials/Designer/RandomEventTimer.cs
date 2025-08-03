using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;


namespace WideWade
{
    public class RandomEventTimer : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Runs on start, if set to false you'll have to activate it manually.")]
        public bool onStart = true;
        [Tooltip("Make sure the last event fired isn't the same as the current one")]
        public bool dontRepeatLastCalledEvent = true;
        [Header("Timer Settings | Min/Max")]
        [Tooltip("The min/max range of time we'll wait until we fire something.")]
        public Vector2 timerLengthRange = new Vector2(1, 5);
        [Header("Event Settings")]
        [Tooltip("How many events we'll roll and fire when the timer hits 0.")]
        public int eventFireCount = 1;

        public List<EventDraw> events = new List<EventDraw>();

        EventDraw lastPicked;

  

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (onStart)
            {
                float time = UnityEngine.Random.Range(timerLengthRange.x, timerLengthRange.y);
                TimerRoll(time);
            }
        }


        public void RollRandomEvents()
        {
            RollForEvents();
        }

        public void RollRandomEvents(float time)
        {
            StartCoroutine(TimerRoll(time));
        }

        private void RollForEvents()
        {

            // First we gotta figure out who's on the list.
            List<EventDraw> validEvents = new List<EventDraw>();

            foreach (EventDraw eventDraw in events)
            {
                if (eventDraw.conditionEvaluator == null || eventDraw.conditionEvaluator.Evaluate())
                {
                    validEvents.Add(eventDraw);
                }
            }


            if (validEvents.Count == 0)
            {
                Debug.Log("All of the events are unable to be fired due to their event conditions");
                return;
            }

            for (int i = 0; i < eventFireCount; i++)
            {
                EventDraw selected;
                if (dontRepeatLastCalledEvent && lastPicked != null && validEvents.Count > 1)
                {
                    int safety = 0;
                    do
                    {
                        selected = RandomPickEvent(validEvents);
                        safety++;
                    }
                    while (selected == lastPicked && safety < 10); // suffering builds character > suffering builds character > suffering builds...
                }
                else
                {
                    selected = RandomPickEvent(validEvents);
                }


                if (selected != null)
                {
                    selected.onEventDraw.Invoke();
                    lastPicked = selected;
                }
                else
                {
                    // Look I can write a funny warning message on a warning thats impossible to happen.i
                    Debug.LogWarning("Event selection failed unexpectedly. No seriously I have no idea how we got here.");
                }
            }
        }

        private EventDraw RandomPickEvent(List<EventDraw> eventPool)
        {
            // Get our weights for the random call.
            int totalWeight = 0;
            foreach (EventDraw eventDraw in eventPool)
            {
                totalWeight += eventDraw.weight;
            }

            if (totalWeight > 0)
            {
                // roll with our weights
                int roll = UnityEngine.Random.Range(1, totalWeight + 1);
                int cumulative = 0;

                foreach (EventDraw eventDraw in eventPool)
                {
                    cumulative += eventDraw.weight;
                    if (roll <= cumulative)
                    {
                        return eventDraw;
                    }
                }

            }


            return null; // should never reach here but C# whines if i dont have this.
        }

        private IEnumerator TimerRoll(float time)
        {
            yield return new WaitForSeconds(time);
            RollForEvents();
        }

    }

    [Serializable]
    public class EventDraw
    {
        [Tooltip("Called if we get drawn.")]
        public UnityEvent onEventDraw;
        [Range(1,100)]
        [Tooltip("How much of a chance we get to be drawn")]
        public int weight;
        [Tooltip("Condition for us to fire our event, if we can't fire our event we'll be taken out of the pool.")]
        public ConditionEvaluator conditionEvaluator;
    }

}
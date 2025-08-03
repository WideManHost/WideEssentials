using System;
using System.Collections;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// A Time Manager that controls the timescale as well as some logic for manipulating time.
    /// </summary>
    public class TimeManager : StaticManager<TimeManager>
    {
        /// <summary>
        /// Represents whether in-game time is stopped.
        /// </summary>
        public bool TimeStopped { get { return TimeStopped; }}

        /// <summary>
        /// Represents whether the time is stopped in an external meta context. I.e Game Paused.
        /// </summary>
        public bool MetaStopped { get { return MetaStopped; }}

        [Tooltip("A bool to tell if game time has been stopped. (not used)")]
        [SerializeField]
        private bool timeStopped;
        [Tooltip("A bool to tell if the game is stopped for a non-game event, like pausing.")]
        [SerializeField]
        private bool metaStopped; // when paused

        public static float currentTimeStep { get; private set; } = 1;

       

        private void Update()
        {
            if (!metaStopped)
            {
                Time.timeScale = currentTimeStep;
            }

            // Debug.Log(Time.timeScale + "..." + currentTimeStep);

        }

        /// <summary>
        /// Meta stop the time.
        /// </summary>
        public static void MetaStop()
        {
            Instance.metaStopped = true;
            Time.timeScale = 0;
        }

        /// <summary>
        /// Meta start the time.
        /// </summary>
        public static void MetaStart()
        {
            Instance.metaStopped = false;
            Time.timeScale = currentTimeStep;
        }


        /// <summary>
        /// Sets the current timeScale to the timeStep, but lerps it over the duration period
        /// </summary>
        /// <param name="timeStep">new timeScale</param>
        /// <param name="duration">how long it takes us to get to that speed.</param>
        public static void LinearIntoTimeStep(float timeStep, float duration)
        {
            Instance.StartCoroutine(Instance.LerpIntoTime(timeStep, duration));
        }

        /// <summary>
        /// Stops time for x amound of seconds
        /// </summary>
        /// <param name="duration">How long to stop time for</param>
        public static void StopTime(float duration)
        {
            if (!Instance.timeStopped)
            {
                Instance.StartCoroutine(Instance.TimeStopDuration(duration));
            }
        }

        private IEnumerator LerpIntoTime(float newTimeStep, float duration)
        {
            while (timeStopped == true) yield return new WaitForEndOfFrame();

            float startTime = currentTimeStep;
            float currTime = 0f;
            while (currTime <= duration)
            {
                if (!metaStopped)
                {
                    currentTimeStep = Mathf.Lerp(startTime, newTimeStep, currTime / duration);
                    currTime += Time.unscaledDeltaTime;


                }
                yield return new WaitForEndOfFrame();
            }

            currentTimeStep = newTimeStep;
        }

        private IEnumerator TimeStopDuration(float duration)
        {
            Instance.timeStopped = true;
            currentTimeStep = 0f;
            float currentTimeStopped = 0;
            while (currentTimeStopped <= duration)
            {
                if (!metaStopped)
                {
                    currentTimeStopped += Time.unscaledDeltaTime;
                }
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
            currentTimeStep = 1;
            Instance.timeStopped = false;
        }
    }

}

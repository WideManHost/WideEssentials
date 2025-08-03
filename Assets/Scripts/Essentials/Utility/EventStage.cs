using System;
using UnityEngine.Events;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// A stage of events for the 
    /// </summary>
    [Serializable]
    public class EventStage
    {
        [Tooltip("how long to wait after the last event stage was fired to fire this stage.")]
        public float fireAfterSeconds = 0;
        public UnityEvent toFire;
    }
}
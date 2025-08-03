using UltEvents;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Fires an event on start.
    /// </summary>
    public class ActivatedEvent : MonoBehaviour
    {
        [Header("Events")]
        [Tooltip("Fires this event as soon as the object is activated.")]
        public UltEvent onStart;

        void Start()
        {
            onStart.Invoke();
        }
    }

}


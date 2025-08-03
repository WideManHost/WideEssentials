using UnityEngine.Events;
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
        public UnityEvent onStart;

        void Start()
        {
            onStart.Invoke();
        }
    }

}


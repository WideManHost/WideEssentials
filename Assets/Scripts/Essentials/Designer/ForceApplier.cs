using System.Collections.Generic;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Applies force to anything within its trigger. 
    /// </summary>
    public class ForceApplier : MonoBehaviour
    {
        [Header("Settings")]
        public TriggerApplicationMode triggerInteractionMode = TriggerApplicationMode.OnEnter;
        public Transform forceDirection;
        public float power;

        [Tooltip("Force mode for how the force should be applied, Force would be a continous force, using mass. Impulse would be really quickly, using mass.")]
        public ForceMode forceMode;

        public enum TriggerApplicationMode
        {
            OnEnter,
            OnExit,
            OnStay,
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerInteractionMode == TriggerApplicationMode.OnEnter)
            {
                Rigidbody rb = other.attachedRigidbody;
                rb.AddForce(forceDirection.forward, forceMode);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerInteractionMode == TriggerApplicationMode.OnStay)
            {
                Rigidbody rb = other.attachedRigidbody;
                rb.AddForce(forceDirection.forward, forceMode);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (triggerInteractionMode == TriggerApplicationMode.OnExit)
            {
                Rigidbody rb = other.attachedRigidbody;
                rb.AddForce(forceDirection.forward, forceMode);
            }
        }

        private void Start()
        {
            if (forceDirection == null)
            {
                Debug.LogWarning("Our force direction transform is null!");
            }
        }

        private void OnDrawGizmos()
        {
            if (forceDirection != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + (forceDirection.forward * (power / 10)));
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(forceDirection.position, 0.5f);
            }

        }
    }
}

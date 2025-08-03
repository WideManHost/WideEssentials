using UnityEngine.Events;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Follows the inputted transform at a defined speed. Lerps.
    /// </summary>
    public class FollowTransform : MonoBehaviour
    {
        [Header("Settings")]
        public Transform toFollow;
        public float followSpeed = 5;
        [Tooltip("How close we have to be to actually count as being at our target.")]
        public float reachThreshold = 0.05f;

        [Header("Rotation Settings")]
        public bool matchRotation = false;
        public float rotationLerpSpeed = 5f;

        [Header("Events")]
        public UnityEvent onReachedTarget;

        private bool hasReached = false;

        private void Update()
        {
            if (toFollow != null)
            {
                Vector3 direction = toFollow.position - transform.position;
                float distance = direction.magnitude;

                // if we're not there, lets keep going.
                if (distance > reachThreshold)
                {
                    Vector3 move = direction.normalized * followSpeed * Time.deltaTime;
                    // slow us down our distance is smaller than our move speed.
                    if (move.magnitude > distance)
                    {
                        move = direction;
                    }
                        

                    transform.position += move;
                    // Reset in case it moves away at a later perioud
                    hasReached = false; 
                }
                else if (!hasReached)
                {
                    // we're here, set our position and fire the event
                    transform.position = toFollow.position;
                    onReachedTarget.Invoke();
                    hasReached = true;
                }

                if (matchRotation)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, toFollow.rotation, rotationLerpSpeed * Time.deltaTime);
                }
            }

            
        }
    }

}

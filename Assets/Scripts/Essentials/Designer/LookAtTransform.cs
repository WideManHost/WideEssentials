using UnityEngine;


namespace WideWade
{
    /// <summary>
    /// Looks at the transform provided.
    /// </summary>
    public class LookAtTransform : MonoBehaviour
    {
        [Header("Settings")]
        // TODO: Add lerping
        public Transform target;

        void Update()
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = targetRotation;
                }
            }
            
        }
    }
}

using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Spins and hovers the object slowly.
    /// </summary>
    public class Spin : MonoBehaviour
    {
        [Header("Settings")]
        public float frequency = 2f;
        public float amplitude = 0.25f;

        public float rotationSpeed = 20;
        
        float _startPosition;


        // Start is called before the first frame update
        void Start()
        {
            _startPosition = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
            transform.position = new Vector3(transform.position.x, _startPosition + (Mathf.Sin(Time.time * frequency) * amplitude), transform.position.z);
        }

    }
}

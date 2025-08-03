using UnityEngine;

namespace WideWade
{
    public class DestroySelf : MonoBehaviour
    {
        [Header("Settings")]
        public bool destoryOnStart = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (destoryOnStart)
            {
                Destroy(gameObject);
            }
        }

        public void DestroyThyself()
        {
            Destroy(gameObject);
        }
    }

}

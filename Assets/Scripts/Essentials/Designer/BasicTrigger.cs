using System.Collections.Generic;
using UltEvents;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Basic Trigger that fires events based on tags, and also different interactions.
    /// </summary>
    public class BasicTrigger : MonoBehaviour
    {
        [Header("Events")]
        [Tooltip("Fires when something with the whitelisted tag enters our trigger")]
        public UltEvent<Collider> onTriggerEnter;
        [Tooltip("Fires when something with the whitelisted tag stays in our trigger")]
        public UltEvent<Collider> onTriggerStay;
        [Tooltip("Fires when something with the whitelisted tag exits our trigger")]
        public UltEvent<Collider> onTriggerExit;

        public List<string> tagWhitelist = new List<string>();

        private void OnTriggerEnter(Collider other)
        {
            if (tagWhitelist.Contains(other.tag))
            {
                onTriggerEnter.Invoke(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (tagWhitelist.Contains(other.tag))
            {
                onTriggerStay.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (tagWhitelist.Contains(other.tag))
            {
                onTriggerExit.Invoke(other);
            }
        }
    }
}


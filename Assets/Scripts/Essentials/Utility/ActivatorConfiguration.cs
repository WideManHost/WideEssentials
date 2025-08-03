using System;
using UnityEngine.Events;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Configuration for an object to be activated. funnily enough it's used interchangably between the Activator and Deactivator
    /// </summary>
    [Serializable]
    public class ActivatorConfiguration 
    {
        [Tooltip("What game object we're activating.")]
        public GameObject toActivate;
        [Tooltip("How long we wait when triggered to activate")]
        public float activateDelay;

        [Tooltip("Fires when the Object Activator runs, regardless of activation or deactivation.")]
        public UnityEvent toFire;

        public void Activate()
        {
            toActivate.SetActive(true);
            toFire.Invoke();
        }

        public void Deactivate()
        {
            toActivate.SetActive(false);
            toFire.Invoke();
        }
       
    }

    
}


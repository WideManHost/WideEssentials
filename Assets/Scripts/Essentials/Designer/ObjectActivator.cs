using System.Collections.Generic;
using System.Collections;
using UltEvents;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Activates/Deactivates and fires events when triggered, be it on start or through external means.
    /// </summary>
    public class ObjectActivator : MonoBehaviour
    {
        [Header("Settings")]
        public bool onlyOnce = true;
        [Tooltip("Enabled by default, If disabled, this will instead activate when all of the activation prerequisites are hit..")]
        public bool automaticCheck = true;
        [Tooltip("Cooldown before the activation/deactivation can happen again. (if onlyOnce is disabled")]
        public float activationCooldown;


        [Header("Conditions")]
        public ConditionEvaluator conditionValidator;

        [Header("To Activate")]
        [Tooltip("Please be aware that this is non sequencial and does not rely on the order they're put in for delays. each delay timer starts at the same time.")]
        public List<ActivatorConfiguration> toActivateList = new List<ActivatorConfiguration>();
        [Header("To Deactivate")]
        [Tooltip("Please be aware that this is non sequencial and does not rely on the order they're put in for delays. each delay timer starts at the same time.")]
        public List<ActivatorConfiguration> toDeactivateList = new List<ActivatorConfiguration>();

        [Header("Events")]
        [Tooltip("Fires when the Object Activator is triggered")]
        public UltEvent onActivatorTriggered;


        bool _activated = false;
        float _currentCooldown = 0;

        // Update is called once per frame
        void Update()
        {
            if (!automaticCheck)
            {
                PrimeActivator();
            }

            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
            }
            else
            {
                _currentCooldown = 0;
            }

        }

        /// <summary>
        /// This is what actually fires the events and activates/deactivates stuff.
        /// </summary>
        void OnActivateTrigger()
        {
            onActivatorTriggered.Invoke();
            foreach (ActivatorConfiguration config in toActivateList)
            {
                StartCoroutine(ActivateCoroutine(config, config.activateDelay));
            }

            foreach (ActivatorConfiguration config in toDeactivateList)
            {
                StartCoroutine(DeactivateCoroutine(config, config.activateDelay));
            }

            _activated = true;
            _currentCooldown = activationCooldown;
        }

        /// <summary>
        /// This primes the activation sequence, which more accurately just checks if we even CAN activate
        /// </summary>
        void PrimeActivator()
        {
            if (_currentCooldown <= 0 && (!onlyOnce || !_activated) )
            {
                // Lets ball out if we met out conditions
                if (conditionValidator != null)
                {
                    if (conditionValidator.Evaluate())
                    {
                        OnActivateTrigger();
                    }
                }
                else
                {
                    OnActivateTrigger();
                }
            }
        }

        private IEnumerator ActivateCoroutine(ActivatorConfiguration config, float delay)
        {
            yield return new WaitForSeconds(delay);
            config.Activate();
            yield return null;
        }

        private IEnumerator DeactivateCoroutine(ActivatorConfiguration config, float delay)
        {
            yield return new WaitForSeconds(delay);
            config.Deactivate();
            yield return null;
        }


        private void OnDrawGizmosSelected()
        {
            // Draw blue line to stuff that will activate
            if (toActivateList.Count > 0)
            {
                Gizmos.color = Color.blue;
                foreach (ActivatorConfiguration toActivate in toActivateList)
                {
                    if (toActivate.toActivate != null)
                    {
                        Gizmos.DrawLine(transform.position, toActivate.toActivate.transform.position);
                    }

                }
            }

            // Draw red line to stuff that will deactivate
            if (toDeactivateList.Count > 0)
            {
                Gizmos.color = Color.red;
                foreach (ActivatorConfiguration toDeactivate in toDeactivateList)
                {
                    if (toDeactivate.toActivate != null)
                    {
                        Gizmos.DrawLine(transform.position, toDeactivate.toActivate.transform.position);
                    }
                }
            }
        }
    }
}

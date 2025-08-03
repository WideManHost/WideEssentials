using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Takes in a number of conditions and evaluates them when called.
    /// </summary>
    public class ConditionEvaluator : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Prerequisites that must be met in order to pass evaluation")]
        public List<ConditionCheck> activationConditions = new List<ConditionCheck>();
        [Tooltip("Should all conditions be true for us to pass validation? or should just one of the conditions?")]
        public ValidatorOperation operation = ValidatorOperation.All;
        [Tooltip("Reverses the output, what would normally be true, would be false and vise-versa.")]
        public bool reverseOutput = false;

        public enum ValidatorOperation
        {
            All,
            Any
        }


        /// <summary>
        /// Evaluates the conditions and returns whether our operation is valid.
        /// </summary>
        /// <returns>True if conditions are in place to proceed./returns>
        public bool Evaluate()
        {
            // if there are no conditions no point doing a check of nothing or erroring. pass to return true
            if (activationConditions.Count > 0)
            {
                foreach (ConditionCheck prerequisite in activationConditions)
                {
                    if (prerequisite != null)
                    {
                        switch (operation)
                        {
                            case ValidatorOperation.All:
                                // we need all to be true and this guy is false, return false.
                                if (!prerequisite.ConditionsMet())
                                {
                                    return false;
                                }
                                break;
                            case ValidatorOperation.Any:
                                // if you're true, everybody is true.
                                if (prerequisite.ConditionsMet())
                                {
                                    return true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("a prerequisite was null!");
                        return false;
                    }
                }

                // Reached end without early return
                return operation == ValidatorOperation.All;
            }

            // Flat out just returns true.
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            // Draw purple line to stuff affect out conditions.
            if (activationConditions.Count > 0)
            {
                Gizmos.color = new Color(0.4f, 0.1f, 1f);
                foreach (ConditionCheck prerequisite in activationConditions)
                {
                    if (prerequisite.targetGameObject != null)
                    {
                        Gizmos.DrawLine(transform.position, prerequisite.targetGameObject.transform.position);
                    }
                }
            }
        }
    }

}


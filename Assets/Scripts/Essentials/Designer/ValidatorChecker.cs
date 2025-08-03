using UnityEngine.Events;
using UnityEngine;

namespace WideWade
{
    public class ValidatorChecker : MonoBehaviour
    {
        [Header("Settings")]
        public ConditionEvaluator validator;

        [Header("Events")]
        public UnityEvent<bool> onValidatorChanged;
        public UnityEvent onValidatorTrue;
        public UnityEvent onValidatorFalse;

        bool lastValue;


        private void Start()
        {
            lastValue = validator.Evaluate();
        }

        // Update is called once per frame
        void Update()
        {
            bool currentValue = validator.Evaluate();
            if (lastValue != currentValue)
            {
                onValidatorChanged.Invoke(currentValue);

                if (currentValue)
                {
                    onValidatorTrue.Invoke();
                }
                else
                {
                    onValidatorFalse.Invoke();
                }
            }


            lastValue = currentValue;
        }
    }

}


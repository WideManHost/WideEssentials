using UltEvents;
using UnityEngine;

namespace WideWade
{
    public class ValidatorChecker : MonoBehaviour
    {
        [Header("Settings")]
        public ConditionEvaluator validator;

        [Header("Events")]
        public UltEvent<bool> onValidatorChanged;
        public UltEvent onValidatorTrue;
        public UltEvent onValidatorFalse;

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


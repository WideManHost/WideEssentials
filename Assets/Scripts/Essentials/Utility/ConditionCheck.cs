using System;
using System.Reflection;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Activator Checks and the glorious logic that comes with it.
    /// 
    /// Shoutout my actual job of I would've never heard of what System.Reflection does.
    /// </summary>
    [Serializable]
    public class ConditionCheck
    {
        public GameObject targetGameObject;
        public int componentIndex;
        public string fieldName;

        public enum ValueType { Float, Bool }
        public ValueType valueType;

        public enum FloatComparison { Equal, Greater, Less, LessOrEqual, GreaterOrEqual, NotEqual }
        public FloatComparison floatComparison;
        public float floatTarget;

        public bool boolExpected;

        public bool ConditionsMet()
        {
            if (targetGameObject == null || string.IsNullOrEmpty(fieldName))
            {
                throw new NullReferenceException("Target Component or Field is Null");
            }

            Type type = targetGameObject.GetType();
            PropertyInfo prop = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);


            // object is the funniest type to me, its just some thing? we dont even know!
            object value;
            Type memberType;

            if (prop != null)
            {
                value = prop.GetValue(targetGameObject);
                memberType = prop.PropertyType;
            }
            else if (field != null)
            {
                value = field.GetValue(targetGameObject);
                memberType = field.FieldType;
            }
            else
            {
                Debug.LogWarning($"Field or property {fieldName} not found.");
                return false;
            }

            if (valueType == ValueType.Float && memberType == typeof(float))
            {
                float current = (float)value;
                switch (floatComparison)
                {
                    case FloatComparison.Equal: 
                        return Mathf.Approximately(current, floatTarget);
                    case FloatComparison.NotEqual: 
                        return !Mathf.Approximately(current, floatTarget);
                    case FloatComparison.Greater: 
                        return current > floatTarget;
                    case FloatComparison.Less: 
                        return current < floatTarget;
                    case FloatComparison.GreaterOrEqual: 
                        return current >= floatTarget;
                    case FloatComparison.LessOrEqual: 
                        return current <= floatTarget;
                }
            }
            else if (valueType == ValueType.Bool && memberType == typeof(bool))
            {
                return (bool)value == boolExpected;
            }

            return false;
        }
    }
}

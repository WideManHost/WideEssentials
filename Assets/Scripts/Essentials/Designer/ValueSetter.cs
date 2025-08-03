using System;
using System.Reflection;
using UnityEngine;

namespace WideWade
{
    public class ValueSetter : MonoBehaviour
    {
        public enum OperationType
        {
            Set,
            Add,
            Subtract,
            CopyFrom,
            InvertBool
        }

        [Header("Settings")]
        [Tooltip("Change on Start?")]
        public bool onStart = true;

        [Header("Target Component & Field")]
        [Tooltip("What gameObject we're changing")]
        public GameObject targetGameObject;
        [HideInInspector]
        [Tooltip("What component we're changing")]
        public Component targetComponent;
        [HideInInspector]
        public string targetFieldName;

        [Header("Operation")]
        public OperationType operation;

        [Header("Input Value")]
        public string inputValue;

        [Header("Reference Component")]
        [Tooltip("What gameObject we're copying from")]
        public GameObject sourceGameObject;
        [HideInInspector]
        [Tooltip("What component we're copying from")]
        public Component sourceComponent;
        //[HideInInspector]
        public string sourceFieldName;

        public void Manipulate()
        {

            Type type = targetComponent.GetType();
            FieldInfo field = type.GetField(targetFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo property = type.GetProperty(targetFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null && property != null)
            {
                // conditional expression my beloved
                object currentValue = field != null ? field.GetValue(targetComponent) : property.GetValue(targetComponent);
                object newValue = null;

                // Copy from another component
                if (operation == OperationType.CopyFrom && sourceComponent != null)
                {
                    Type srcType = sourceComponent.GetType();
                    FieldInfo srcField = srcType.GetField(sourceFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    PropertyInfo srcProperty = srcType.GetProperty(sourceFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    if (srcField != null)
                        newValue = srcField.GetValue(sourceComponent);
                    else if (srcProperty != null)
                        newValue = srcProperty.GetValue(sourceComponent);
                    else
                        Debug.LogWarning("Source field/property not found.");
                }
                else if (operation == OperationType.InvertBool && currentValue is bool b)
                {
                    newValue = !b;
                }
                else
                {
                    try
                    {
                        // 20 dollar parser check
                        if (currentValue is float f)
                        {
                            float parsed = float.Parse(inputValue);
                            newValue = operation switch
                            {
                                OperationType.Set => parsed,
                                OperationType.Add => f + parsed,
                                OperationType.Subtract => f - parsed,
                                _ => f
                            };
                        }
                        else if (currentValue is int i)
                        {
                            int parsed = int.Parse(inputValue);
                            newValue = operation switch
                            {
                                OperationType.Set => parsed,
                                OperationType.Add => i + parsed,
                                OperationType.Subtract => i - parsed,
                                _ => i
                            };
                        }
                        else if (currentValue is bool bo)
                        {
                            bool parsed = bool.Parse(inputValue);
                            newValue = operation switch
                            {
                                OperationType.Set => parsed,
                                OperationType.InvertBool => !bo,
                                _ => bo
                            };
                        }
                        else
                        {
                            Debug.LogWarning("Unsupported field type: " + currentValue.GetType().Name);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Failed to parse value: " + e.Message);
                        return;
                    }
                }

                // Set the value
                if (field != null)
                {
                    field.SetValue(targetComponent, newValue);
                }
                else if (property != null && property.CanWrite)
                {
                    property.SetValue(targetComponent, newValue);
                }
                else
                {
                    Debug.LogWarning("Cannot write to property.");
                }
            }

            
        }

        public void Start()
        {
            Manipulate();
        }
    }
}

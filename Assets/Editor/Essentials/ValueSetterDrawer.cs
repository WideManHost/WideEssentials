using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;


namespace WideWade
{
    /// <summary>
    /// me going into a primal state looking at another editor script.
    /// Please if you take better care at this, I'll be happy for you.
    /// Please if you decide not to take better care at this, i'll be double happy for you.
    /// </summary>
    [CustomEditor(typeof(ValueSetter))]
    public class ValueSetterDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onStart"));
            SerializedProperty targetGameObject = serializedObject.FindProperty("targetGameObject");
            EditorGUILayout.PropertyField(targetGameObject);

            // Get actual FieldOperator instance
            ValueSetter valueSetter = (ValueSetter)target;

            // How can we set a component if we dont have an object stupid nuts.
            if (valueSetter.targetGameObject != null)
            {
                Component[] targetComponents = valueSetter.targetGameObject.GetComponents<Component>();
                string[] targetOptions = targetComponents.Select(c => c.GetType().Name).ToArray();
                int targettedSelectedIndex = Array.IndexOf(targetComponents, valueSetter.targetComponent);
                int newIndex = EditorGUILayout.Popup("Target Component", targettedSelectedIndex, targetOptions);

                if (newIndex != targettedSelectedIndex && newIndex >= 0)
                {
                    Undo.RecordObject(valueSetter, "Change Target Component");
                    valueSetter.targetComponent = targetComponents[newIndex];
                    EditorUtility.SetDirty(valueSetter);
                }

                if (valueSetter.targetComponent != null)
                {
                    string[] memberNames = ConditionCheckDrawer.GetFieldOrPropertyNames(valueSetter.targetComponent);
                    int selectedMemberIndex = Array.IndexOf(memberNames, valueSetter.targetFieldName);
                    int newMemberIndex = EditorGUILayout.Popup("Target Field/Property", selectedMemberIndex, memberNames);

                    if (newMemberIndex != selectedMemberIndex && newMemberIndex >= 0)
                    {
                        Undo.RecordObject(valueSetter, "Change Target Field");
                        valueSetter.targetFieldName = memberNames[newMemberIndex];
                        EditorUtility.SetDirty(valueSetter);
                    }
                }
            }

            SerializedProperty operationProp = serializedObject.FindProperty("operation");
            EditorGUILayout.PropertyField(operationProp);



            // if we're copying another components
            if ((ValueSetter.OperationType)operationProp.enumValueIndex == ValueSetter.OperationType.CopyFrom)
            {
                SerializedProperty sourceGameObject = serializedObject.FindProperty("sourceGameObject");
                EditorGUILayout.PropertyField(sourceGameObject);

                // How can we set a component if we dont have an object stupid nuts.
                if (valueSetter.sourceGameObject != null)
                {
                    Component[] sourceComponents = valueSetter.sourceGameObject.GetComponents<Component>();
                    string[] sourceOptions = sourceComponents.Select(c => c.GetType().Name).ToArray();
                    int sourceSelectedIndex = Array.IndexOf(sourceComponents, valueSetter.sourceComponent);
                    int newIndex = EditorGUILayout.Popup("Source Component", sourceSelectedIndex, sourceOptions);

                    if (newIndex != sourceSelectedIndex && newIndex >= 0)
                    {
                        Undo.RecordObject(valueSetter, "Change Source Component");
                        valueSetter.sourceComponent = sourceComponents[newIndex];
                        EditorUtility.SetDirty(valueSetter);
                    }

                    if (valueSetter.sourceComponent != null)
                    {
                        string[] memberNames = ConditionCheckDrawer.GetFieldOrPropertyNames(valueSetter.sourceComponent);
                        int selectedMemberIndex = Array.IndexOf(memberNames, valueSetter.sourceFieldName);
                        int newMemberIndex = EditorGUILayout.Popup("Source Field/Property", selectedMemberIndex, memberNames);

                        if (newMemberIndex != selectedMemberIndex && newMemberIndex >= 0)
                        {
                            Undo.RecordObject(valueSetter, "Change Source Field");
                            valueSetter.sourceFieldName = memberNames[newMemberIndex];
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("sourceFieldName"));
                            EditorUtility.SetDirty(valueSetter);
                        }
                    }

                }
            }
            
            if ((ValueSetter.OperationType)operationProp.enumValueIndex != ValueSetter.OperationType.InvertBool && (ValueSetter.OperationType)operationProp.enumValueIndex != ValueSetter.OperationType.CopyFrom)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inputValue"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

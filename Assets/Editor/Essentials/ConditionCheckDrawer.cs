using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace WideWade
{
    /// <summary>
    /// I'm going to be 100% with you, I am amazed any of this works. Editor scripts are weird.
    /// 
    /// Also shoutout unity not having polymorphic class handling for inspector shit, makes this convoluted for no reason :sob:
    /// </summary>
    [CustomPropertyDrawer(typeof(ConditionCheck))]
    public class ConditionCheckDrawer : PropertyDrawer
    {
        private List<string> _componentNames = new();
        private List<Component> _cachedComponents = new();
        private List<string> _availableFields = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            float lineHeight = EditorGUIUtility.singleLineHeight + 2f;
            Rect rect = new(position.x, position.y, position.width, lineHeight);

            // Serialized properties
            SerializedProperty gameObjectProp = property.FindPropertyRelative("targetGameObject");
            SerializedProperty componentIndexProp = property.FindPropertyRelative("componentIndex");
            SerializedProperty fieldNameProp = property.FindPropertyRelative("fieldName");
            SerializedProperty valueTypeProp = property.FindPropertyRelative("valueType");
            SerializedProperty floatComparisonProp = property.FindPropertyRelative("floatComparison");
            SerializedProperty floatTargetProp = property.FindPropertyRelative("floatTarget");
            SerializedProperty boolExpectedProp = property.FindPropertyRelative("boolExpected");

            // GameObject field
            EditorGUI.PropertyField(rect, gameObjectProp, new GUIContent("Target GameObject"));
            rect.y += lineHeight;
            GameObject referenceObject = gameObjectProp.objectReferenceValue as GameObject;
            Component selectedComponent;

            _componentNames.Clear();
            _cachedComponents.Clear();
            _availableFields.Clear();

            if (referenceObject != null)
            {
                Component[] comps = referenceObject.GetComponents<Component>();
                for (int i = 0; i < comps.Length; i++)
                {
                    if (comps[i] == null) continue;
                    _componentNames.Add(comps[i].GetType().Name);
                    _cachedComponents.Add(comps[i]);
                }

                if (componentIndexProp.intValue >= _cachedComponents.Count)
                {
                    componentIndexProp.intValue = 0;
                }
                    

                int selectedComponentIndex = componentIndexProp.intValue;

                // Draw component dropdown
                selectedComponentIndex = EditorGUI.Popup(rect, "Component", selectedComponentIndex, _componentNames.ToArray());
                componentIndexProp.intValue = selectedComponentIndex;
                rect.y += lineHeight;

                if (selectedComponentIndex >= 0 && selectedComponentIndex < _cachedComponents.Count)
                {
                    selectedComponent = _cachedComponents[selectedComponentIndex];
                    Type type = selectedComponent.GetType();

                    _availableFields = GetFieldOrPropertyNames(selectedComponent).ToList();

                    // Draw field selector
                    int fieldIndex = _availableFields.IndexOf(fieldNameProp.stringValue);
                    if (fieldIndex < 0 && _availableFields.Count > 0)
                    {
                        fieldIndex = 0;
                    }

                    if (_availableFields.Count > 0)
                    {
                        fieldIndex = EditorGUI.Popup(rect, "Field", fieldIndex, _availableFields.ToArray());
                        rect.y += lineHeight;
                        fieldNameProp.stringValue = _availableFields[fieldIndex];

                        // Set value type (field or bool)
                        string memberName = _availableFields[fieldIndex];
                        MemberInfo member = type.GetField(memberName);

                        if (member == null)
                        {
                            member = type.GetProperty(memberName);
                        }

                        Type fieldType = null;

                        if (member is FieldInfo fi)
                        {
                            fieldType = fi.FieldType;
                        }
                        else if (member is PropertyInfo pi)
                        {
                            fieldType = pi.PropertyType;
                        }

                        if (fieldType == typeof(float))
                        {
                            valueTypeProp.enumValueIndex = (int)ConditionCheck.ValueType.Float;
                        }
                        else if (fieldType == typeof(bool))
                        {
                            valueTypeProp.enumValueIndex = (int)ConditionCheck.ValueType.Bool;
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(rect, "No valid fields");
                        rect.y += lineHeight;
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(rect, "Select a GameObject first.");
                rect.y += lineHeight;
            }

            // Draw the value fields depending on what type they are
            ConditionCheck.ValueType valueType = (ConditionCheck.ValueType)valueTypeProp.enumValueIndex;
            switch (valueType)
            {
                case ConditionCheck.ValueType.Float:
                    EditorGUI.PropertyField(rect, floatComparisonProp);
                    rect.y += lineHeight;
                    EditorGUI.PropertyField(rect, floatTargetProp, new GUIContent("Target Value"));
                    break;

                case ConditionCheck.ValueType.Bool:
                    EditorGUI.PropertyField(rect, boolExpectedProp, new GUIContent("Expected Value"));
                    break;
            }

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Just gets property height. (Overrides PropertyDrawer's implementation)
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionCheck.ValueType valueType = (ConditionCheck.ValueType)property.FindPropertyRelative("valueType").enumValueIndex;
            return (EditorGUIUtility.singleLineHeight + 2f) * (valueType == ConditionCheck.ValueType.Float ? 5 : 4);
        }

        public static string[] GetFieldOrPropertyNames(Component component)
        {
            Type type = component.GetType();
            // Shoutout Linq for being frreaking awsome, also shoutout intro to appdev for showing me about Linq.
            // appdev ptsd those who know.
            IEnumerable<string> fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                             .Where(f => IsSupportedType(f.FieldType))
                             .Select(f => f.Name);

            IEnumerable<string> properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(p => p.CanRead && p.CanWrite && IsSupportedType(p.PropertyType))
                                 .Select(p => p.Name);

            return fields.Concat(properties).Distinct().OrderBy(n => n).ToArray();
        }

        private static bool IsSupportedType(Type type)
        {
            return type == typeof(int) || type == typeof(float) || type == typeof(bool);
        }


        // quick talk on my comments, I know damn well I'm gonna recieve some critisism on my unprofessional comments.
        // Mfw when I make my own scripts and people complain?
        // whatever in a real job I don't make these kinds of comments, what fool do you take me for!



        // a wide one, thats who.
    }
}


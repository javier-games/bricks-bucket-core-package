using System;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Attributes.Editor
{
    /// <!-- EnumMaskAttributeDrawer -->
    ///
    /// <summary>
    /// Custom Mask Attribute Drawer.
    /// </summary>
    ///
    /// <!-- By Javier García | @jvrgms | 2020 -->
    [CustomPropertyDrawer (typeof (EnumMaskAttribute))]
    public class EnumMaskAttributeDrawer : PropertyDrawer
    {
        #region Fields

        /// <summary>
        /// Whether the type has been checked.
        /// </summary>
        private bool _checked;

        /// <summary>
        /// Custom Mask to display.
        /// </summary>
        private EnumMaskAttribute _attribute;

        #endregion


        #region Property

        /// <summary>
        /// Property of the attribute.
        /// </summary>
        private EnumMaskAttribute Attribute =>
            _attribute ?? (
                _attribute = attribute as EnumMaskAttribute
            );

        #endregion


        #region Method Overrides

        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override float
            GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight (property);
        }

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void
            OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                if (!_checked)
                {
                    var serializedObject = property.serializedObject;
                    // TODO: Add Log method to CustomMaskAttributeEditor.OnGUI.
                    Debug.LogWarning (
                        $"Property {property.name} in object " +
                        $"{serializedObject?.targetObject} is of wrong type." +
                        "Type expected: Int",
                        serializedObject?.targetObject
                    );
                    _checked = true;
                }

                EditorGUI.PropertyField (position, property, label);
                return;
            }

            string[] enumNames = Enum.GetNames (Attribute.enumType);

            if (enumNames.Length == 0)
            {
                if (!_checked)
                {
                    var serializedObject = property.serializedObject;
                    // TODO: Add Log method to CustomMaskAttributeEditor.OnGUI.
                    Debug.LogWarning (
                        $"Enum Type {Attribute.enumType} has not elements.",
                        serializedObject?.targetObject
                    );
                    _checked = true;
                }

                EditorGUI.PropertyField (position, property, label);
                return;
            }

            property.intValue = EditorGUI.MaskField (
                position: position,
                label: label,
                mask: property.intValue,
                displayedOptions: Enum.GetNames (Attribute.enumType)
            );
        }

        #endregion
    }
}

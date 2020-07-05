using System.Collections.Generic;
using BricksBucket.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Attributes.Editor
{
    // ReSharper disable CommentTypo
    /// <!-- ShowIfAttributeDrawer -->
    ///
    /// <summary>
    ///
    /// <para>
    /// Drawer for properties to hide or show the property according to
    /// other property value.
    /// </para>
    /// 
    /// <para>
    /// Based in the <see href="https://github.com/Deadcows/MyBox">MyBox
    /// project by @deadcows</see>.
    /// </para>
    ///
    /// </summary>
    ///
    /// <seealso href="https://github.com/Deadcows/MyBox">
    /// Deadcows/MyBox</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    // ReSharper restore CommentTypo
    [CustomPropertyDrawer (typeof (ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        #region Fields

        /// <summary> History of warnings. </summary>
        private readonly HashSet<object> _warnings = new HashSet<object> ();

        /// <summary> Reference attribute. </summary>
        private ShowIfAttribute _attribute;

        /// <summary> Flag for visibility. </summary>
        private bool _isVisible = true;

        /// <summary> Access referenced attribute.</summary>
        private ShowIfAttribute Attribute =>
            _attribute ?? (
                _attribute = attribute as ShowIfAttribute
            );

        #endregion


        #region Method Overrides

        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override float GetPropertyHeight (
            SerializedProperty property, GUIContent label
        )
        {
            _isVisible = CheckPropertyVisibility (property);
            return _isVisible ? EditorGUI.GetPropertyHeight (property) : 0;
        }

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void OnGUI (
            Rect position, SerializedProperty property, GUIContent label
        )
        {
            if (_isVisible)
                EditorGUI.PropertyField (position, property, label, true);
        }

        #endregion


        #region Methods

        /// <summary> Checks whether a property is visible. </summary>
        /// <param name="property"> Property to draw. </param>
        /// <returns> Whether a property is visible. </returns>
        public bool CheckPropertyVisibility (SerializedProperty property)
        {

            SerializedProperty condition = FindRelativeProperty (
                property: property,
                toFind: Attribute.propertyToCheck
            );

            if (condition == null) return true;

            bool isBoolMatch =
                condition.propertyType == SerializedPropertyType.Boolean &&
                condition.boolValue;

            string compareStringValue = Attribute.compareValue != null
                ? Attribute.compareValue.ToString ().ToUpper ()
                : "NULL";

            if (isBoolMatch && compareStringValue == "FALSE")
                isBoolMatch = false;

            string conditionStringValue = condition.ToStringValue ().ToUpper ();
            bool objectMatch = compareStringValue == conditionStringValue;
            bool isVisible = !(!isBoolMatch && !objectMatch);
            if (Attribute.inverse) isVisible = !isVisible;
            return isVisible;
        }

        /// <summary> Finds the relative property. </summary>
        /// <param name="property"> Property to draw. </param>
        /// <param name="toFind"> Name of the property to find. </param>
        /// <returns> Relative property. </returns>
        protected SerializedProperty
            FindRelativeProperty (SerializedProperty property, string toFind)
        {
            if (property.depth == 0)
                return property.serializedObject.FindProperty (toFind);

            var parent = property.FindParentProperty ();
            var path = property.GetValuePath ();

            // If nested property is equals to null hit an array property.
            if (parent != null) return parent.FindPropertyRelative (toFind);
            var cleanPath = path.Substring (0, path.IndexOf ('['));

            var propertySerializedObject = property.serializedObject;

            var arrayProperty = propertySerializedObject?.FindProperty (
                propertyPath: cleanPath
            );
            if (arrayProperty == null) return null;

            if (_warnings.Contains (arrayProperty.exposedReferenceValue))
                return null;

            var arraySerializedObject = arrayProperty.serializedObject;
            if (arraySerializedObject == null) return null;

            var target = arraySerializedObject.targetObject;
            if (target == null) return null;

            // TODO: Implement Log method in ShowIfAttributeDrawer.
            Debug.LogWarning (
                $"Property {arrayProperty.name} in object {target.name}" +
                "are not supported by [ShowIfAttribute]"
            );
            _warnings.Add (arrayProperty.exposedReferenceValue);
            return null;

        }

        #endregion
    }
}
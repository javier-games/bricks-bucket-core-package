using Monogum.BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Editor.Attributes
{
    // ReSharper disable CommentTypo
    /// <!-- TagAttributeDrawer -->
    ///
    /// <summary>
    ///
    /// <para>
    /// Editor tool to draw a menu for a string.
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
    [CustomPropertyDrawer (typeof (TagAttribute))]
    public class TagAttributeDrawer : PropertyDrawer
    {
        #region Properties

        /// <summary> Wether ther type has been checked. </summary>
        private bool _checked;

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
            if (property.propertyType != SerializedPropertyType.String)
            {
                if (!_checked)
                {
                    var serializedObject = property.serializedObject;
                    // TODO: Replace Log Method in TagAttributeDrawer.OnGUI.
                    Debug.LogWarning (
                        $"Property {property.name} in object " + 
                        $"{serializedObject?.targetObject} is of wrong type." +
                        "Type expected: String",
                        serializedObject?.targetObject
                    );
                    _checked = true;
                }
                EditorGUI.PropertyField (position, property, label);
                return;
            }

            property.stringValue = EditorGUI.TagField (
                position: position,
                label: label,
                tag: property.stringValue
            );
        }

        #endregion
    }

}

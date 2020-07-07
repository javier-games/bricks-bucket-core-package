using BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Editor.Attributes
{
    // ReSharper disable CommentTypo
    /// <!-- AutoFillAttributeDrawer -->
    ///
    /// <summary>
    ///
    /// <para>
    /// Property Drawer for the AutoProperty Attribute.
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
    [CustomPropertyDrawer (typeof (AutoFillAttribute))]
    public class AutoFillAttributeDrawer : PropertyDrawer
    {
        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override float GetPropertyHeight (
            SerializedProperty property, GUIContent label
        ) =>
            EditorGUI.GetPropertyHeight (property);

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void OnGUI (
            Rect position, SerializedProperty property, GUIContent label
        )
        {
            GUI.enabled = false;
            EditorGUI.PropertyField (position, property, label);
            GUI.enabled = true;
        }
    }
}

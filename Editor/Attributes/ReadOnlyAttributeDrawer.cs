using Monogum.BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Editor.Attributes
{
    /// <!-- ReadOnlyAttributeDrawer -->
    /// 
    /// <summary>
    /// Draws property but disables its edition.
    /// </summary>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	[CustomPropertyDrawer (typeof(ReadOnlyAttribute))]
	public class ReadOnlyAttributeDrawer : PropertyDrawer
	{
        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
		public override float
        GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
		
		/// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void
        OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}
}

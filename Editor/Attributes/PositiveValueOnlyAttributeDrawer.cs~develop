using Monogum.BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Editor.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- PositiveValueOnlyAttributeDrawer -->
	/// <summary>
	/// 
	/// <para>
	/// Draws and help to not use negative values on numeric properties.
	/// </para>
	/// 
	/// <para>
	/// Based in the <see href="https://github.com/Deadcows/MyBox">MyBox
	/// project by @deadcows</see> and the original version by <see creh="
	/// https://github.com/Kaynn-Cahya">@Kaynn-Cahya</see>.
	/// </para>
	///
	/// </summary>
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// <seealso href="https://github.com/Kaynn-Cahya">
	/// @Kaynn-Cahya</seealso>
	///
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[CustomPropertyDrawer (typeof (PositiveOnlyAttribute))]
	public class PositiveValueOnlyAttributeDrawer : PropertyDrawer
	{
		/// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
		public override float GetPropertyHeight (
			SerializedProperty property, GUIContent label
		)
		{
			return EditorGUI.GetPropertyHeight (property);
		}

		/// <inheritdoc cref="PropertyDrawer.OnGUI"/>
		public override void OnGUI (
			Rect position, SerializedProperty property, GUIContent label
		)
		{
			if (!IsNumerical (property.propertyType))
			{
				EditorGUILayout.HelpBox (
					message: string.Concat (
						"PositiveValueOnly attribute is used ",
						"on non-numeric property."),
					type: MessageType.Warning
				);
			}
			else if (HandleNegativeValues (property))
				property.serializedObject.ApplyModifiedProperties ();

			EditorGUI.PropertyField (position, property, true);
		}


		/// <summary> Handle negative values on properties. </summary>
		/// <returns> Whether the value is set to positive. </returns>
		private static bool HandleNegativeValues (SerializedProperty property)
		{
			switch (property.propertyType)
			{
				case SerializedPropertyType.Float:
				case SerializedPropertyType.Integer:
					return HandleNegativeNumbers (property);

				case SerializedPropertyType.Vector2:
				case SerializedPropertyType.Vector3:
				case SerializedPropertyType.Vector4:
					return HandleVectors (property);

				case SerializedPropertyType.Vector2Int:
				case SerializedPropertyType.Vector3Int:
					return HandleIntVectors (property);
				
				default:
					return false;
			}
		}

		/// <summary> Handles negative values in numeric properties. </summary>
		/// <param name="property"> Property to handle with. </param>
		/// <returns> Whether the property had to be handle. </returns>
		private static bool HandleNegativeNumbers (SerializedProperty property)
		{
			switch (property.propertyType)
			{
				case SerializedPropertyType.Float:
					if (!(property.floatValue < 0)) return false;
					property.floatValue = 0;
					return true;
				case SerializedPropertyType.Integer:
					if (property.intValue >= 0) return false;
					property.intValue = 0;
					return true;
				default:
					return false;
			}
		}

		/// <summary> Handles negative values in vector properties. </summary>
		/// <param name="property"> Property to handle with. </param>
		/// <returns> Whether the property had to be handle. </returns>
		private static bool HandleVectors (SerializedProperty property)
		{
			Vector4 vector;
			switch (property.propertyType)
			{
				case SerializedPropertyType.Vector2:
					vector = property.vector2Value;
					break;
				case SerializedPropertyType.Vector3:
					vector = property.vector3Value;
					break;
				case SerializedPropertyType.Vector4:
					vector = property.vector4Value;
					break;
				default:
					vector = Vector4.zero;
					break;
			}

			bool handled = false;
			for (int i = 0; i < 4; ++i)
			{
				if (!(vector[i] < 0f)) continue;
				vector[i] = 0;
				handled = true;
			}

			switch (property.propertyType)
			{
				case SerializedPropertyType.Vector2:
					property.vector2Value = vector;
					break;
				case SerializedPropertyType.Vector3:
					property.vector3Value = vector;
					break;
				case SerializedPropertyType.Vector4:
					property.vector4Value = vector;
					break;
			}

			return handled;
		}

		/// <summary> Handles negative values in int vectors. </summary>
		/// <param name="property"> Property to handle with. </param>
		/// <returns> Whether the property had to be handle. </returns>
		private static bool HandleIntVectors (SerializedProperty property)
		{
			switch (property.propertyType)
			{

				case SerializedPropertyType.Vector2Int:
					var vector = property.vector2IntValue;
					if (vector.x > 0 && vector.y > 0) return false;

					property.vector2IntValue = new Vector2Int (
						vector.x < 0 ? 0 : vector.x,
						vector.y < 0 ? 0 : vector.y);
					return true;

				case SerializedPropertyType.Vector3Int:
					var vector3 = property.vector3IntValue;
					if (vector3.x > 0 && vector3.y > 0 && vector3.z > 0)
						return false;
					property.vector3Value = new Vector3 (
						vector3.x < 0 ? 0 : vector3.x,
						vector3.y < 0 ? 0 : vector3.y,
						vector3.z < 0 ? 0 : vector3.z);
					return true;

				default:
					return false;
			}
		}

		/// <summary> Defines whether is a valid type. </summary>
		/// <param name="propertyType"> Property type to check. </param>
		/// <returns> Whether is a valid type. </returns>
		private static bool IsNumerical (SerializedPropertyType propertyType)
		{
			switch (propertyType)
			{
				case SerializedPropertyType.Float:
				case SerializedPropertyType.Integer:
				case SerializedPropertyType.Vector2:
				case SerializedPropertyType.Vector3:
				case SerializedPropertyType.Vector4:
				case SerializedPropertyType.Vector2Int:
				case SerializedPropertyType.Vector3Int:
					return true;

				default:
					return false;
			}
		}
	}
}

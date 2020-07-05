using BricksBucket.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Attributes.Editor
{
	// ReSharper disable CommentTypo
	/// <!-- AutoFillAttributeHandler -->
	///
	/// <summary>
	///
	/// <para>
	/// Called on will save assets to search for properties and fill them.
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
	[InitializeOnLoad]
	public static class AutoFillAttributeHandler
	{
		/// <summary>
		/// Subscribing to editor events.
		/// </summary>
		static AutoFillAttributeHandler () =>
			EditorEvents.onSaveAssets += CheckComponents;

		/// <summary>
		/// Check for all auto property attributes.
		/// </summary>
		private static void CheckComponents ()
		{
			var components =
				Core.Editor.BucketEditor.GetFieldsWithAttribute
					<AutoFillAttribute> ();

			for (int i = 0; i < components.Length; i++)
				FillProperty (components[i]);
		}

		/// <summary>
		/// Fills the property.
		/// </summary>
		/// <param name="property">Info of the component.</param>
		private static void FillProperty (
			Core.Editor.BucketEditor.ComponentFieldInfo property
		)
		{
			var propertyType = property.field.FieldType;

			if (propertyType.IsArray)
			{
				var underlyingType = propertyType.GetElementType ();
				// ReSharper disable once CoVariantArrayConversion
				Object[] components =
					property.component.GetComponentsInChildren (
						t: underlyingType,
						includeInactive: true
					);

				if (components != null && components.Length > 0)
				{
					var serializedObject = new SerializedObject (
						obj: property.component
					);

					var serializedProperty = serializedObject.FindProperty (
						propertyPath: property.field.Name
					);

					serializedProperty.ReplaceArray (components);
					serializedObject.ApplyModifiedProperties ();
					serializedObject.Dispose ();
					return;
				}
			}
			else
			{
				var component = property.component.GetComponentInChildren (
					t: propertyType,
					includeInactive: true
				);

				if (component != null)
				{
					var serializedObject = new SerializedObject (
						obj: property.component
					);

					var serializedProperty = serializedObject.FindProperty (
						propertyPath: property.field.Name
					);

					if (serializedProperty != null)
					{
						serializedProperty.objectReferenceValue = component;
						serializedObject.ApplyModifiedProperties ();
						serializedObject.Dispose ();
						return;
					}
				}
			} 
            
			// TODO: Change Log method in AutoFillAttributeHandler.FillProperty.
			Debug.LogError (
				$"AutoProperty not found in {property.component.name}" +
				" component. Make sure it is also serialized."
			);

			throw Core.Editor.BucketEditor.FieldNotFoundException (
				type: propertyType,
				field: property.field.Name
			);
		}
	}
}
using Monogum.BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;
namespace Monogum.BricksBucket.Core.Editor.Attributes
{
	// ReSharper disable CommentTypo
    /// <!-- ShowIfAttributeDrawer -->
    /// 
    /// <summary>
    ///
    /// Sprite Layer Attribute Drawer.
    ///
    /// <para>
    /// Editor tool to draw Popup with sprite layers available.
    /// </para>
	/// 
	/// <para>
	/// Based in the <see href="https://github.com/Deadcows/MyBox">MyBox
	/// project by @deadcows</see>.
	/// </para>
	///
    /// </summary>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[CustomPropertyDrawer (typeof(SpriteLayerAttribute))]
	public class SpriteLayerAttributeDrawer : PropertyDrawer
	{
		#region Fields
		
		/// <summary> Whether their type has been checked. </summary>
		private bool _checkedType;

		#endregion
		

        #region Method Overrides

		/// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
		public override float GetPropertyHeight (
			SerializedProperty property, GUIContent label
		) =>
			EditorGUI.GetPropertyHeight (property);

		/// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.Integer)
			{
                if (!_checkedType)
                {
                    var serializedObject = property.serializedObject;
					// TODO: Replace Log Method in SpriteLayerAttributeDrawer.
					Debug.LogWarning (
						$"Property {property.name} in object " +
						$"{serializedObject?.targetObject} is of wrong type." +
						"Type expected: Int"
					);
                    _checkedType = true;
                }
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			var spriteLayerNames = GetSpriteLayerNames();

            //  Drawing the pop up.
            EditorGUI.BeginProperty (position, label, property);

            // To show which sprite layer is currently selected.
            bool layerFound = TryGetSpriteLayerIndexFromProperty (
                index: out int currentSpriteLayerIndex,
                spriteLayerNames: spriteLayerNames,
                property: property
            );

            if (!layerFound)
            {
                // Set to default layer. (Previous layer was removed)
                var serializedObject = property.serializedObject;
				// TODO: Replace Log Method in SpriteLayerAttributeDrawer.
				Debug.LogWarning (
					$"Property {property.name} in object " +
					$"{serializedObject?.targetObject} is set to the default." +
					"Previously selected layer was removed."
				);
                property.intValue = 0;
                currentSpriteLayerIndex = 0;
            }

            int selectedSpriteLayerIndex = EditorGUI.Popup (
                position: position,
                label: label.text,
                selectedIndex: currentSpriteLayerIndex,
                displayedOptions: spriteLayerNames
            );

            // Change property value if user selects a new sprite layer.
            if (selectedSpriteLayerIndex != currentSpriteLayerIndex)
                property.intValue = SortingLayer.NameToID (
                    spriteLayerNames[selectedSpriteLayerIndex]
                );

            EditorGUI.EndProperty ();
        }

        #endregion

		
        #region Methods

        /// <summary> Try to gets the current index sprite. </summary>
        /// <param name="index"> Current index. </param>
        /// <param name="spriteLayerNames"> Sprite layer names. </param>
        /// <param name="property"> Int Property to draw. </param>
        /// <returns> Whether or not gets the current index sprite. </returns>
		private static bool TryGetSpriteLayerIndexFromProperty(
            out int index,
            string[] spriteLayerNames,
            SerializedProperty property
        ) {
			string layerName = SortingLayer.IDToName(property.intValue);

			// Return the index where on it matches.
			for (int i = 0; i < spriteLayerNames.Length; ++i)
                if (spriteLayerNames[i].Equals(layerName))
				{
					index = i;
					return true;
				}

			// The current layer was removed.
			index = -1;
			return false;
		}

        /// <summary> Returns the collection of sprite layer names.  </summary>
        /// <returns> Collection of sprite layer names. </returns>
		private static string[] GetSpriteLayerNames()
		{
			string[] result = new string[SortingLayer.layers.Length];

			for (int i = 0; i < result.Length; ++i)
				result[i] = SortingLayer.layers[i].name;

			return result;
		}

		#endregion
	}
}


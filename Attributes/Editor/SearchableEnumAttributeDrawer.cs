using BricksBucket.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Attributes.Editor
{
    // ReSharper disable CommentTypo
    /// <!-- SearchableEnumAttributeDrawer -->
    /// 
    /// <summary>
    ///
    /// <para>
    /// Draws the custom enum selector popup for enum fields using the
    /// SearchableEnumAttribute.
    /// </para>
    /// 
    /// <para>
    /// Based in the <see href=
    /// "https://github.com/roboryantron/UnityEditorJunkie">UnityEditorJunkie
    /// project by @roboryantron</see>.
    /// </para>
    ///
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    // ReSharper restore CommentTypo
    [CustomPropertyDrawer (typeof (SearchableEnumAttribute))]
    public class SearchableEnumAttributeDrawer : PropertyDrawer
    {
        #region Fields
        
        /// <summary>
        /// Cache of the hash to use to resolve the ID for the drawer.
        /// </summary>
        private int _idHash;

        #endregion

        
        #region Override Methods

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void
            OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            // If this is not used on an enum, show an error
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                EditorGUILayout.HelpBox (
                    message: string.Concat (
                        "SearchableEnum attribute is used ",
                        "on non-enum property."),
                    type: MessageType.Warning
                );

                EditorGUI.PropertyField (position, property, true);
                return;
            }

            // By manually creating the control ID, we can keep the ID for the
            // label and button the same. This lets them be selected together
            // with the keyboard in the inspector, much like a normal popup.
            if (_idHash == 0)
                _idHash = "SearchableEnumAttributeDrawer".GetHashCode ();

            int id = GUIUtility.GetControlID (
                hint: _idHash,
                focusType: FocusType.Keyboard,
                rect: position
            );

            label = EditorGUI.BeginProperty (position, label, property);
            position = EditorGUI.PrefixLabel (position, id, label);

            GUIContent buttonText = new GUIContent (
                text: property.enumDisplayNames[property.enumValueIndex]
            );

            if (DropdownButton (id, position, buttonText))
            {
                void OnSelect (int i)
                {
                    property.enumValueIndex = i;
                    property.serializedObject.ApplyModifiedProperties ();
                }

                SearchablePopup.Show (
                    activatorRect: position,
                    options: property.enumDisplayNames,
                    current: property.enumValueIndex,
                    onSelectionCallback: OnSelect
                );
            }

            EditorGUI.EndProperty ();
        }

        #endregion

        
        #region Methods

        /// <summary>
        /// A custom button drawer that allows for a controlID so that we can
        /// sync the button ID and the label ID to allow for keyboard
        /// navigation like the built-in enum drawers.
        /// </summary>
        /// <param name="id"> Unique ID reference of the control. </param>
        /// <param name="position"> Position where to draw popup. </param>
        /// <param name="content"> Current option. </param>
        /// <returns> Whether the button has been pressed. </returns>
        private static bool
            DropdownButton (int id, Rect position, GUIContent content)
        {
            var current = Event.current;
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (
                        position.Contains (current.mousePosition) &&
                        current.button == 0
                    )
                    {
                        Event.current.Use ();
                        return true;
                    }
                    break;
                case EventType.KeyDown:
                    if (
                        GUIUtility.keyboardControl == id &&
                        current.character == '\n'
                    )
                    {
                        Event.current.Use ();
                        return true;
                    }
                    break;
                case EventType.Repaint:
                    EditorStyles.popup.Draw (position, content, id, false);
                    break;
                default:
                    return false;
            }

            return false;
        }
        
        #endregion
    }
}

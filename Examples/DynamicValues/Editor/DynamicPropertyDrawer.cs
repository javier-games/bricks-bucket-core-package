using System;
using System.Reflection;
using System.Collections.Generic;
using Monogum.BricksBucket.Core.Generics.Editor;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Monogum.BricksBucket.Core.Examples.DynamicProperties.Editor
{
    /// <!-- AbstractReferenceDrawer -->
    /// <summary>
    /// Drawer for the Variable Class.
    /// </summary>
    [CustomPropertyDrawer(typeof(DynamicProperty))]
    public class DynamicPropertyDrawer : PropertyDrawer
    {
        #region Fields

        /// <summary>
        /// References of a dynamic variable.
        /// </summary>
        private readonly DynamicProperty _reference = new DynamicProperty();
        
        /// <summary>
        /// Reference of components.
        /// </summary>
        private Component[] _components;
        
        /// <summary>
        /// Reference of propertiesInfo.
        /// </summary>
        private PropertyInfo[] _propertiesInfo;
        
        /// <summary>
        /// List that the stores the properties and components.
        /// </summary>
        private readonly List<string> _dropdown = new List<string>();

        #endregion

        #region Methods

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {

            //  Getting the properties references.
            var componentProperty = property.FindPropertyRelative("component");
            var namePropProperty = property.FindPropertyRelative("property");

            _reference.SetComponent(componentProperty.objectReferenceValue);
            _reference.SetProperty(namePropProperty.stringValue);

            if (_reference.Component == null)
            {
                SelectObject(position);
                StoreData(property);
                return;
            }

            if (_reference.Component as GameObject)
            {
                SelectComponent(position);
                StoreData(property);
                return;
            }

            if (!_reference.Registry.ContainsComponent(_reference.Component))
            {
                RegistryComponent(position);
                StoreData(property);
                return;
            }

            if (string.IsNullOrEmpty(_reference.Property))
            {
                SelectProperty(position);
                StoreData(property);
                return;
            }

            DrawReference(position);
            StoreData(property);
        }

        private void SelectObject(Rect position)
        {
            EditorGUI.BeginChangeCheck();
            var objectAttempt = EditorGUI.ObjectField(
                position: position,
                label: "Select Object",
                obj: _reference.Component,
                objType: typeof(Object),
                allowSceneObjects: true
            );
            if (!EditorGUI.EndChangeCheck()) return;
            _reference.SetComponent(objectAttempt);
        }

        private void SelectComponent(Rect position)
        {
            // Set up dropdown.
            _components = (_reference.Component as GameObject)?
                .GetComponents<Component>();
            _dropdown.Clear();
            _dropdown.Add("[Component]");
            if (_components != null)
            {
                foreach (var component in _components)
                    _dropdown.Add(component.GetType().Name);
            }
            _dropdown.Add("[Remove]");

            
            var componentIndex = 0;
            EditorGUI.BeginChangeCheck();
            componentIndex = EditorGUI.Popup(
                position,
                "Component",
                componentIndex,
                _dropdown.ToArray()
            );
            if (!EditorGUI.EndChangeCheck()) return;
            
            // Delete option.
            if (componentIndex == _dropdown.Count - 1)
            {
                _reference.SetComponent(null);
                return;
            }

            // Component Selected
            if (componentIndex == 0 || _components == null) return;
            _reference.SetComponent(_components[componentIndex - 1]);
        }

        private void RegistryComponent(Rect position)
        {
            var type = _reference.Component.GetType();
            
            var rectMessage = position;
            rectMessage.width = EditorGUIUtility.labelWidth;
            var rectButtonCancel = position;
            rectButtonCancel.width =
                (position.width - rectMessage.width) * 0.5f;
            rectButtonCancel.x = position.x + rectMessage.width;
            var rectButtonOk = rectButtonCancel;
            rectButtonOk.x += rectButtonCancel.width;
            rectMessage.width -= 5;

            EditorGUI.LabelField(
                rectMessage,
                new GUIContent(
                    "Not Registered!",
                    EditorGUIUtility.IconContent("console.warnicon.sml").image,
                    $"The Object {type.Name} is not registered yet."
                )
            );

            EditorGUI.BeginChangeCheck();
            GUI.Button(rectButtonCancel, "Cancel");
            if (EditorGUI.EndChangeCheck())
            {
                _reference.SetComponent(null);
            }

            EditorGUI.BeginChangeCheck();
            GUI.Button(rectButtonOk, "Register");
            if (EditorGUI.EndChangeCheck())
            {
                HardwiredFileWriter.RegisterType(
                    _reference.Component.GetType(),
                    _reference.ScriptData,
                    _reference.Registry
                );
            }
        }
        
        private void SelectProperty(Rect position)
        {
            // Dropdown Setup
            _propertiesInfo = _reference.Component.GetType().GetProperties(
                (BindingFlags.Public | BindingFlags.Instance)
            );
            _dropdown.Clear();
            _dropdown.Add("[Property]");
            foreach (var info in _propertiesInfo)
            {
                var isValid =
                    info.CanRead &&
                    info.CanWrite &&
                    !info.IsDefined(typeof(ObsoleteAttribute), inherit: true);
                if (!isValid) continue;
                _dropdown.Add(info.Name);
            }
            
            // Draw Dropdown.
            EditorGUI.BeginChangeCheck();
            var propertyIndex = EditorGUI.Popup(
                position,
                "Property",
                selectedIndex: 0,
                _dropdown.ToArray()
            );
            if (EditorGUI.EndChangeCheck())
            {
                _reference.SetProperty(propertyIndex != 0
                    ? _dropdown[propertyIndex]
                    : string.Empty);
            }
        }

        private void DrawReference(Rect position)
        {
            
            var propertyPosition = position;
            propertyPosition.width -= 30;
            var rectButtonRemove = position;
            rectButtonRemove.width = 25;
            rectButtonRemove.x = position.x + propertyPosition.width + 5;

            if (GUI.Button(rectButtonRemove,
                new GUIContent(
                    EditorGUIUtility.IconContent("console.warnicon.sml").image,
                    $"Reset this reference."
                )))
            {
                _reference.SetComponent(null);
                return;
            }

            var label = new GUIContent(_reference.Property);
            
            EditorGUI.BeginChangeCheck();
            
            var type = _reference.GetPropertyType();
            if (type == null)
            {
                EditorGUI.LabelField(
                    position,
                    label,
                    new GUIContent("Not Supported Yet")
                );
                return;
            }
            
            if (type == typeof(bool))
            {
                _reference.SetValue(
                    EditorGUI.Toggle(
                        propertyPosition,
                        label,
                        (bool) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(int))
            {
                _reference.SetValue(
                    EditorGUI.IntField(
                        propertyPosition,
                        label,
                        (int) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(float))
            {
                _reference.SetValue(
                    EditorGUI.FloatField(
                        propertyPosition,
                        label,
                        (float) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(double))
            {
                _reference.SetValue(
                    EditorGUI.DoubleField(
                        propertyPosition,
                        label,
                        (double) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(Vector2))
            {
                _reference.SetValue(
                    EditorGUI.Vector2Field(
                        propertyPosition,
                        label,
                        (Vector2) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(Vector3))
            {
                _reference.SetValue(
                    EditorGUI.Vector3Field(
                        propertyPosition,
                        label,
                        (Vector3) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(Vector4))
            {
                _reference.SetValue(
                    EditorGUI.Vector4Field(
                        propertyPosition,
                        label,
                        (Vector4) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(Quaternion))
            {
                var aux = EditorGUI.Vector4Field(
                    propertyPosition,
                    label,
                    (Vector4) _reference.GetValue()
                );
                _reference.SetValue(
                    new Quaternion(aux.x, aux.y, aux.z, aux.w)
                );
            }
            
            if (type == typeof(Color))
            {
                _reference.SetValue(
                    EditorGUI.ColorField(
                        propertyPosition,
                        label,
                        (Color) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(string))
            {
                _reference.SetValue(
                    EditorGUI.TextField(
                        propertyPosition,
                        label,
                        (string) _reference.GetValue()
                    )
                );
            }
            
            if (type == typeof(AnimationCurve))
            {
                _reference.SetValue(
                    EditorGUI.CurveField(
                        propertyPosition,
                        label,
                        _reference.GetValue() as AnimationCurve
                    )
                );
            }
            
            if (type == typeof(Object))
            {
                var component = _reference.GetValue() as Object;
                _reference.SetValue(EditorGUI.ObjectField(
                    propertyPosition,
                    label,
                    component,
                    component != null
                        ? component.GetType()
                        : typeof(Object),
                    true
                ));
            }
            
            if (type.IsSubclassOf(typeof(Object)))
            {
                _reference.SetValue(
                    EditorGUI.ObjectField(
                        propertyPosition,
                        label,
                        _reference.GetValue() as Object, 
                        type,
                        true
                    )
                );
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_reference.Component);
            }
        }
            
        /// <summary>
        /// Stores the data.
        /// </summary>
        /// <param name="property">Parent Property.</param>
        private void StoreData(SerializedProperty property)
        {
            property.FindPropertyRelative("component").objectReferenceValue =
                _reference.Component;
            property.FindPropertyRelative("property").stringValue =
                _reference.Property;
        }
        
        #endregion
        
    }
}
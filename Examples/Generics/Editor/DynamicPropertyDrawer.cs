using System.Reflection;
using System.Collections.Generic;
using Monogum.BricksBucket.Core.Examples.Generics;
using UnityEngine;
using UnityEditor;

namespace Monogum.BricksBucket.Core.Generics.Editor
{
    /// <!-- AbstractReferenceDrawer -->
    /// <summary>
    /// Drawer for the Variable Class.
    /// </summary>
    public abstract class DynamicPropertyDrawer : PropertyDrawer
    {

        #region Fields

        /// <summary>
        /// Height of a standard single line.
        /// </summary>
        private readonly float _fieldSize = EditorGUIUtility.singleLineHeight;

        /// <summary>
        /// References of a dynamic variable.
        /// </summary>
        private readonly DynamicProperty _dynRef = new DynamicProperty();

        /// <summary>
        /// List that the stores the properties and components.
        /// </summary>
        private readonly List<string> _dropdown = new List<string>();

        /// <summary>
        /// Reference of propertiesInfo.
        /// </summary>
        private PropertyInfo[] _propertiesInfo;

        /// <summary>
        /// Reference of components.
        /// </summary>
        private Component[] _components;

        /// <summary>
        /// Size of padding.
        /// </summary>
        private const float Padding = 5;

        private Object _objectReferenceAttempt;

        #endregion

        #region Methods

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {

             //  Getting the properties references.
            var dynProperty = property.FindPropertyRelative("value");
            _dynRef.SetComponent(property.FindPropertyRelative("component")
                .objectReferenceValue as Component);
            _dynRef.SetProperty(property.FindPropertyRelative("property")
                .stringValue);

            // Component Unselected.
            if (_dynRef.Component == null)
            {
                // Component selected
                if (_objectReferenceAttempt == null)
                {
                    EditorGUI.BeginChangeCheck();
                    _objectReferenceAttempt = EditorGUI.ObjectField(
                        position: position,
                        label: "Component",
                        obj: _dynRef.Component,
                        objType: _dynRef.Component != null
                            ? _dynRef.Component.GetType()
                            : typeof(Object),
                        allowSceneObjects: true
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (!(_objectReferenceAttempt is GameObject))
                        {
                            _dynRef.SetComponent(
                                _objectReferenceAttempt as Component);
                            _objectReferenceAttempt = null;
                        }
                    }

                    StoreData(property, dynProperty);
                    return;
                }

                // GameObject Selected
                else
                {
                    _components = (_objectReferenceAttempt as GameObject)
                        ?.GetComponents<Component>();
                    _dropdown.Clear();
                    _dropdown.Add("[Component]");
                    _dropdown.Add("GameObject");
                    if (_components != null)
                    {
                        foreach (var component in _components)
                            _dropdown.Add(component.GetType().Name);
                    }

                    _dropdown.Add("[Remove]");

                    var compIndex = 0;
                    EditorGUI.BeginChangeCheck();
                    compIndex = EditorGUI.Popup(
                        position,
                        "Select",
                        compIndex,
                        _dropdown.ToArray()
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        // Delete option.
                        if (compIndex == _dropdown.Count - 1)
                        {
                            _dynRef.SetComponent(null);
                            _objectReferenceAttempt = null;
                            StoreData(property, dynProperty);
                            return;
                        }

                        // Select GameObject
                        if (compIndex == 1)
                        {
                            _dynRef.SetComponent(_objectReferenceAttempt as Component);
                            _objectReferenceAttempt = null;
                            StoreData(property, dynProperty);
                            return;
                        }

                        // Component Selected
                        if (compIndex != 0 && _components != null)
                        {
                            _dynRef.SetComponent(_components[compIndex - 2]);
                            _objectReferenceAttempt = null;
                            StoreData(property, dynProperty);
                            return;
                        }
                    }

                    StoreData(property, dynProperty);
                    return;
                }
            }

            // Component un registered.
            {
                var objectValue = _dynRef.Component;
                var type = objectValue.GetType();
                if (!_dynRef.ComponentRegistry.ContainsComponent(objectValue as Component))
                {
                    var rectMessage = position;
                    rectMessage.width = EditorGUIUtility.labelWidth;
                    var rectButtonCancel = position;
                    rectButtonCancel.width =
                        (position.width - rectMessage.width) * 0.5f;
                    rectButtonCancel.x = position.x + rectMessage.width;
                    var rectButtonOk = rectButtonCancel;
                    rectButtonOk.x += rectButtonCancel.width;
                    rectMessage.width -= Padding;

                    EditorGUI.LabelField(
                        rectMessage,
                        new GUIContent(
                            "Not Registered!",
                            EditorGUIUtility.IconContent("console.warnicon.sml")
                                .image,
                            $"The Object {type.Name} is not registered yet."
                        )
                    );

                    EditorGUI.BeginChangeCheck();
                    GUI.Button(rectButtonCancel, "Cancel");
                    if (EditorGUI.EndChangeCheck())
                    {
                        _dynRef.SetComponent(null);
                    }

                    EditorGUI.BeginChangeCheck();
                    GUI.Button(rectButtonOk, "Register");
                    if (EditorGUI.EndChangeCheck())
                    {
                        HardwiredFileWriter.RegisterType(
                            _dynRef.Component.GetType(),
                            _dynRef.ScriptData,
                            _dynRef.ComponentRegistry
                        );
                    }

                    StoreData(property, dynProperty);
                    return;
                }
            }


            //  If component is different to null.
            if (_dynRef.Component != null)
            {
                //  Shortcuts.
                var objectValue = _dynRef.Component;
                var type = objectValue.GetType();

                //  Property Selection.
                _propertiesInfo = type.GetProperties(
                    (BindingFlags.Public | BindingFlags.Instance)
                );

                _dropdown.Clear();
                _dropdown.Add("None");
                foreach (var info in _propertiesInfo)
                {
                    var isValid =
                        info.CanRead &&
                        info.CanWrite &&
                        !info.IsDefined(
                            typeof(System.ObsoleteAttribute),
                            inherit: true
                        );
                    if (isValid)
                        _dropdown.Add(info.Name);
                }

                var propIndex = 0;
                var inList = false;
                for (var i = 0; i < _dropdown.Count; i++)
                    if (property.FindPropertyRelative("property").stringValue ==
                        _dropdown[i])
                    {
                        propIndex = i;
                        inList = true;
                    }

                if (!inList)
                    _dynRef.SetProperty(string.Empty);


                //  Draw properties.
                var rectObject = position;
                rectObject.width = EditorGUIUtility.labelWidth * 0.5f;
                var rectProperty = rectObject;
                rectProperty.x += rectObject.width;
                var rectValue = position;
                rectValue.width -= EditorGUIUtility.labelWidth;
                rectValue.x += EditorGUIUtility.labelWidth;
                rectProperty.width -= Padding;



                _dynRef.SetComponent(EditorGUI.ObjectField(
                    position: rectObject,
                    obj: _dynRef.Component,
                    objType: _dynRef.Component != null
                        ? _dynRef.Component.GetType()
                        : typeof(Object),
                    allowSceneObjects: true
                ) as Component);

                propIndex = EditorGUI.Popup(rectProperty, propIndex,
                    _dropdown.ToArray());
                
                if (propIndex != 0)
                {
                    _dynRef.SetProperty(_dropdown[propIndex]);

                    switch (_dynRef.Value.Type)
                    {

                        case DynamicValueType.NULL:
                            EditorGUI.LabelField(rectValue, "Null");
                            break;

                        case DynamicValueType.BOOLEAN:
                            _dynRef.SetValue(
                                EditorGUI.Toggle(
                                    rectValue,
                                    (bool) _dynRef.Value.Get(typeof(bool))
                                )
                            );
                            break;

                        case DynamicValueType.INTEGER:
                            _dynRef.SetValue(
                                EditorGUI.IntField(
                                    rectValue,
                                    (int) _dynRef.Value.Get(typeof(int))
                                )
                            );
                            break;

                        case DynamicValueType.FLOAT:
                            _dynRef.SetValue(
                                EditorGUI.FloatField(
                                    rectValue,
                                    (float) _dynRef.Value.Get(typeof(float))
                                )
                            );
                            break;

                        case DynamicValueType.DOUBLE:
                            _dynRef.SetValue(
                                EditorGUI.DoubleField(
                                    rectValue,
                                    (double) _dynRef.Value.Get(
                                        typeof(double))
                                )
                            );
                            break;

                        case DynamicValueType.VECTOR2:
                            _dynRef.SetValue(
                                EditorGUI.Vector2Field(
                                    rectValue,
                                    GUIContent.none, 
                                    (Vector2) _dynRef.Value.Get(
                                        typeof(Vector2))
                                )
                            );
                            break;

                        case DynamicValueType.VECTOR3:
                            _dynRef.SetValue(
                                EditorGUI.Vector3Field(
                                    rectValue,
                                    GUIContent.none,
                                    (Vector3) _dynRef.Value.Get(
                                        typeof(Vector3))
                                )
                            );
                            break;

                        case DynamicValueType.VECTOR4:
                            _dynRef.SetValue(
                                EditorGUI.Vector4Field(
                                    rectValue,
                                    GUIContent.none,
                                    (Vector4) _dynRef.Value.Get(
                                        typeof(Vector4))
                                )
                            );
                            break;

                        case DynamicValueType.QUATERNION:
                            var aux = EditorGUI.Vector4Field(
                                rectValue,
                                GUIContent.none,
                                (Vector4) _dynRef.Value.Get(typeof(Vector4))
                            );
                            _dynRef.SetValue(
                                new Quaternion(aux.x, aux.y, aux.z, aux.w)
                            );
                            break;

                        case DynamicValueType.COLOR:
                            _dynRef.SetValue(
                                EditorGUI.ColorField(
                                    rectValue,
                                    (Color) _dynRef.Value.Get(typeof(Color))
                                )
                            );
                            break;

                        case DynamicValueType.STRING:
                            _dynRef.SetValue(
                                EditorGUI.TextField(
                                    rectValue,
                                    (string) _dynRef.Value.Get(
                                        typeof(string))
                                )
                            );
                            break;

                        case DynamicValueType.CURVE:
                            _dynRef.SetValue(
                                EditorGUI.CurveField(
                                    rectValue,
                                    _dynRef.Value.Get(
                                        typeof(AnimationCurve)
                                    ) as AnimationCurve
                                )
                            );
                            break;

                        case DynamicValueType.ASSET:
                            var component = _dynRef.Value.Get(
                                typeof(Object)
                            ) as Object;
                            _dynRef.SetValue(EditorGUI.ObjectField(
                                rectValue,
                                component,
                                component != null
                                    ? component.GetType()
                                    : typeof(Object),
                                true
                            ));
                            break;
                        
                        default: throw new System.IndexOutOfRangeException();
                    }
                }
                else
                    _dynRef.SetProperty(string.Empty);
            }
            else
                _dynRef.SetProperty(string.Empty);
            
            //  Update properties values.
            StoreData(property, dynProperty);
        }

        /// <summary>
        /// Stores the data.
        /// </summary>
        /// <param name="property">Parent Property.</param>
        /// <param name="dynProperty">Dynamic Value Property</param>
        private void StoreData(SerializedProperty property,
            SerializedProperty dynProperty)
        {

            property.FindPropertyRelative("component").objectReferenceValue =
                _dynRef.Component;
            property.FindPropertyRelative("property").stringValue =
                _dynRef.Property;
            dynProperty.FindPropertyRelative("stringValue").stringValue =
                (string) _dynRef.Value.Get(typeof(string));
            dynProperty.FindPropertyRelative("curve").animationCurveValue =
                _dynRef.Value.Get(typeof(AnimationCurve)) as AnimationCurve;
            dynProperty.FindPropertyRelative("asset").objectReferenceValue =
                _dynRef.Value.Get(typeof(Object)) as Object;
            dynProperty.FindPropertyRelative("vector").vector4Value =
                (Vector4) _dynRef.Value.Get(typeof(Vector4));
            dynProperty.FindPropertyRelative("type").enumValueIndex =
                (int) _dynRef.Value.Type;
        }

        #endregion
    }
}
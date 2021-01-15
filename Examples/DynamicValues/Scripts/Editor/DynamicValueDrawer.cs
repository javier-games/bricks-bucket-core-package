using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Monogum.BricksBucket.Core.Examples.DynamicProperties.Editor
{

    /// <summary>
    /// Drawer for the Variable Class.
    /// </summary>
    [CustomPropertyDrawer(typeof(DynamicValue))]
    public class DynamicValueDrawer : PropertyDrawer
    {
        #region Fields

        /// <summary>
        /// References of a dynamic variable.
        /// </summary>
        private readonly DynamicValue _dynamicValue = new DynamicValue();

        #endregion

        #region Methods

        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label)
        {

            //  Getting the properties references.
            _dynamicValue.Type = (DynamicValueType) property
                .FindPropertyRelative("type")
                .enumValueIndex;
            _dynamicValue.Vector4 =
                property.FindPropertyRelative("vector").vector4Value;
            _dynamicValue.Asset = property.FindPropertyRelative("asset")
                .objectReferenceValue;
            _dynamicValue.Curve = property.FindPropertyRelative("curve")
                .animationCurveValue;
            _dynamicValue.String =
                property.FindPropertyRelative("stringValue").stringValue;

            switch (_dynamicValue.Type)
            {
                case DynamicValueType.NULL:
                    _dynamicValue.Type = (DynamicValueType) EditorGUI.EnumPopup(
                        position,
                        label,
                        _dynamicValue.Type
                    );
                    break;

                case DynamicValueType.BOOLEAN:
                    _dynamicValue.Boolean = EditorGUI.Toggle(
                        position,
                        label,
                        _dynamicValue.Boolean
                    );
                    break;

                case DynamicValueType.INTEGER:
                    _dynamicValue.Integer = EditorGUI.IntField(
                        position,
                        label,
                        _dynamicValue.Integer
                    );
                    break;

                case DynamicValueType.FLOAT:
                    _dynamicValue.Float = EditorGUI.FloatField(
                        position,
                        label,
                        _dynamicValue.Float
                    );
                    break;

                case DynamicValueType.DOUBLE:
                    _dynamicValue.Double = EditorGUI.DoubleField(
                        position,
                        label,
                        _dynamicValue.Double
                    );
                    break;

                case DynamicValueType.VECTOR2:
                    _dynamicValue.Vector2 = EditorGUI.Vector2Field(
                        position,
                        label,
                        _dynamicValue.Vector2
                    );
                    break;

                case DynamicValueType.VECTOR3:
                    _dynamicValue.Vector3 = EditorGUI.Vector3Field(
                        position,
                        label,
                        _dynamicValue.Vector3
                    );
                    break;

                case DynamicValueType.VECTOR4:
                    _dynamicValue.Vector4 = EditorGUI.Vector4Field(
                        position,
                        label,
                        _dynamicValue.Vector4
                    );
                    break;

                case DynamicValueType.QUATERNION:
                    _dynamicValue.Vector4 = EditorGUI.Vector4Field(
                        position,
                        label,
                        _dynamicValue.Vector4
                    );
                    break;

                case DynamicValueType.COLOR:
                    _dynamicValue.Color = EditorGUI.ColorField(
                        position,
                        label,
                        _dynamicValue.Color
                    );
                    break;

                case DynamicValueType.STRING:
                    _dynamicValue.String = EditorGUI.TextField(
                        position,
                        label,
                        _dynamicValue.String
                    );
                    break;

                case DynamicValueType.CURVE:
                    _dynamicValue.Curve = EditorGUI.CurveField(
                        position,
                        label,
                        _dynamicValue.Curve
                    );
                    break;

                case DynamicValueType.ASSET:
                    _dynamicValue.Asset = EditorGUI.ObjectField(
                        position,
                        label,
                        _dynamicValue.Asset,
                        _dynamicValue.Asset != null
                            ? _dynamicValue.Asset.GetType()
                            : typeof(Object),
                        allowSceneObjects: true
                    );
                    break;

                default: throw new System.ArgumentOutOfRangeException();
            }

            //  Update properties values..
            property.FindPropertyRelative("stringValue").stringValue =
                _dynamicValue.String;
            property.FindPropertyRelative("curve").animationCurveValue =
                _dynamicValue.Curve;
            property.FindPropertyRelative("asset").objectReferenceValue =
                _dynamicValue.Asset;
            property.FindPropertyRelative("vector").vector4Value =
                _dynamicValue.Vector4;
            property.FindPropertyRelative("type").enumValueIndex =
                (int) _dynamicValue.Type;
        }

        #endregion
    }
}
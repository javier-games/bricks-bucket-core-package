using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Monogum.BricksBucket.Core.Examples.Generics.Editor
{

    /// <summary>
    /// Drawer for the Variable Class.
    /// </summary>
    [CustomPropertyDrawer(typeof(DynamicValue))]
    public class VariableDrawer : PropertyDrawer
    {

        #region Fields

        /// <summary>
        /// Height of a standard single line.
        /// </summary>
        private readonly float _fieldSize = EditorGUIUtility.singleLineHeight;

        /// <summary>
        /// References of a dynamic variable.
        /// </summary>
        private readonly DynamicValue _dynamicValue = new DynamicValue();

        /// <summary>
        /// Whether to apply extra space because a vector type.
        /// </summary>
        private bool _vectorExtraSpace;

        /// <summary>
        /// Count of fields to draw.
        /// </summary>
        private const int Count = 2;

        /// <summary>
        /// Size of padding.
        /// </summary>
        private const float Padding = 4;

        /// <summary>
        /// Minimum width fore vectors.
        /// </summary>
        private const float MinWidth = 314;

        /// <summary>
        /// Text to show in value.
        /// </summary>
        private const string Text = "Value";

        #endregion
        
        #region Methods

        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label)
        {
            return _vectorExtraSpace
                ? (_fieldSize * (Count + 1)) + (Padding * (Count + 2))
                : (_fieldSize * Count) + (Padding * (Count + 1));
        }

        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label)
        {

            //  For vector properties the width modifies the height.
            _vectorExtraSpace = position.width < MinWidth;

            //  Declaration of rects.
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var rectType = new Rect(
                x: position.x,
                y: position.y + Padding,
                width: position.width,
                height: _fieldSize
            );
            var rectValue = new Rect(
                x: position.x,
                y: position.y + _fieldSize + (Padding * 2),
                width: position.width,
                height: _fieldSize
            );
            EditorGUI.indentLevel = indent;

            //  Getting the properties references.
            _dynamicValue.Type = (DynamicValueType) property.FindPropertyRelative("type")
                .enumValueIndex;
            _dynamicValue.Vector4 =
                property.FindPropertyRelative("vector").vector4Value;
            _dynamicValue.Asset = property.FindPropertyRelative("asset")
                .objectReferenceValue;
            _dynamicValue.Curve = property.FindPropertyRelative("curve")
                .animationCurveValue;
            _dynamicValue.String =
                property.FindPropertyRelative("stringValue").stringValue;

            //  Draw properties.
            _dynamicValue.Type =
                (DynamicValueType) EditorGUI.EnumPopup(rectType, "Variable",
                    _dynamicValue.Type);
            switch (_dynamicValue.Type)
            {

                case DynamicValueType.NULL:
                    EditorGUI.LabelField(rectValue, Text + " null");
                    break;

                case DynamicValueType.BOOLEAN:
                    _dynamicValue.Boolean =
                        EditorGUI.Toggle(rectValue, Text, _dynamicValue.Boolean);
                    break;

                case DynamicValueType.INTEGER:
                    _dynamicValue.Integer =
                        EditorGUI.IntField(rectValue, Text,
                            _dynamicValue.Integer);
                    break;

                case DynamicValueType.FLOAT:
                    _dynamicValue.Float =
                        EditorGUI.FloatField(rectValue, Text,
                            _dynamicValue.Float);
                    break;

                case DynamicValueType.DOUBLE:
                    _dynamicValue.Double =
                        EditorGUI.DoubleField(rectValue, Text,
                            _dynamicValue.Double);
                    break;

                case DynamicValueType.VECTOR2:
                    _dynamicValue.Vector2 =
                        EditorGUI.Vector2Field(rectValue, Text,
                            _dynamicValue.Vector2);
                    break;

                case DynamicValueType.VECTOR3:
                    _dynamicValue.Vector3 =
                        EditorGUI.Vector3Field(rectValue, Text,
                            _dynamicValue.Vector3);
                    break;

                case DynamicValueType.VECTOR4:
                    _dynamicValue.Vector4 =
                        EditorGUI.Vector4Field(rectValue, Text,
                            _dynamicValue.Vector4);
                    break;

                case DynamicValueType.QUATERNION:
                    _dynamicValue.Vector4 =
                        EditorGUI.Vector4Field(rectValue, Text,
                            _dynamicValue.Vector4);
                    break;

                case DynamicValueType.COLOR:
                    _dynamicValue.Color =
                        EditorGUI.ColorField(rectValue, Text,
                            _dynamicValue.Color);
                    break;

                case DynamicValueType.STRING:
                    _dynamicValue.String =
                        EditorGUI.TextField(rectValue, Text,
                            _dynamicValue.String);
                    break;

                case DynamicValueType.CURVE:
                    _dynamicValue.Curve =
                        EditorGUI.CurveField(rectValue, Text,
                            _dynamicValue.Curve);
                    break;

                case DynamicValueType.ASSET:
                    _dynamicValue.Asset = EditorGUI.ObjectField(
                        rectValue,
                        Text,
                        _dynamicValue.Asset,
                        _dynamicValue.Asset != null
                            ? _dynamicValue.Asset.GetType()
                            : typeof(Object),
                        true
                    );
                    break;

                default: throw new System.ArgumentOutOfRangeException();
            }

            //  Update properties values..
            property.FindPropertyRelative("stringValue").stringValue =
                _dynamicValue.String;
            property.FindPropertyRelative("curve").animationCurveValue =
                _dynamicValue.Curve;
            property.FindPropertyRelative("object").objectReferenceValue =
                _dynamicValue.Asset;
            property.FindPropertyRelative("vector").vector4Value =
                _dynamicValue.Vector4;
            property.FindPropertyRelative("type").enumValueIndex =
                (int) _dynamicValue.Type;

        }

        #endregion
    }
}
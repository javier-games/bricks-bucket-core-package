using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace BricksBucket.Core.Generic.Editor
{

    /// <summary>
    /// Drawer for the Variable Class.
    /// </summary>
    [CustomPropertyDrawer(typeof(Variable))]
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
        private readonly Variable _variable = new Variable();

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
            _variable.Type = (DataType) property.FindPropertyRelative("type")
                .enumValueIndex;
            _variable.Vector4 =
                property.FindPropertyRelative("vector").vector4Value;
            _variable.Asset = property.FindPropertyRelative("asset")
                .objectReferenceValue;
            _variable.Curve = property.FindPropertyRelative("curve")
                .animationCurveValue;
            _variable.String =
                property.FindPropertyRelative("stringValue").stringValue;

            //  Draw properties.
            _variable.Type =
                (DataType) EditorGUI.EnumPopup(rectType, "Variable",
                    _variable.Type);
            switch (_variable.Type)
            {

                case DataType.NULL:
                    EditorGUI.LabelField(rectValue, Text + " null");
                    break;

                case DataType.BOOLEAN:
                    _variable.Boolean =
                        EditorGUI.Toggle(rectValue, Text, _variable.Boolean);
                    break;

                case DataType.INTEGER:
                    _variable.Integer =
                        EditorGUI.IntField(rectValue, Text,
                            _variable.Integer);
                    break;

                case DataType.FLOAT:
                    _variable.Float =
                        EditorGUI.FloatField(rectValue, Text,
                            _variable.Float);
                    break;

                case DataType.DOUBLE:
                    _variable.Double =
                        EditorGUI.DoubleField(rectValue, Text,
                            _variable.Double);
                    break;

                case DataType.VECTOR2:
                    _variable.Vector2 =
                        EditorGUI.Vector2Field(rectValue, Text,
                            _variable.Vector2);
                    break;

                case DataType.VECTOR3:
                    _variable.Vector3 =
                        EditorGUI.Vector3Field(rectValue, Text,
                            _variable.Vector3);
                    break;

                case DataType.VECTOR4:
                    _variable.Vector4 =
                        EditorGUI.Vector4Field(rectValue, Text,
                            _variable.Vector4);
                    break;

                case DataType.QUATERNION:
                    _variable.Vector4 =
                        EditorGUI.Vector4Field(rectValue, Text,
                            _variable.Vector4);
                    break;

                case DataType.COLOR:
                    _variable.Color =
                        EditorGUI.ColorField(rectValue, Text,
                            _variable.Color);
                    break;

                case DataType.STRING:
                    _variable.String =
                        EditorGUI.TextField(rectValue, Text,
                            _variable.String);
                    break;

                case DataType.CURVE:
                    _variable.Curve =
                        EditorGUI.CurveField(rectValue, Text,
                            _variable.Curve);
                    break;

                case DataType.ASSET:
                    _variable.Asset = EditorGUI.ObjectField(
                        rectValue,
                        Text,
                        _variable.Asset,
                        _variable.Asset != null
                            ? _variable.Asset.GetType()
                            : typeof(Object),
                        true
                    );
                    break;

                default: throw new System.ArgumentOutOfRangeException();
            }

            //  Update properties values..
            property.FindPropertyRelative("stringValue").stringValue =
                _variable.String;
            property.FindPropertyRelative("curve").animationCurveValue =
                _variable.Curve;
            property.FindPropertyRelative("object").objectReferenceValue =
                _variable.Asset;
            property.FindPropertyRelative("vector").vector4Value =
                _variable.Vector4;
            property.FindPropertyRelative("type").enumValueIndex =
                (int) _variable.Type;

        }

        #endregion
    }
}
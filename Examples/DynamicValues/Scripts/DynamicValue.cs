using System;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;
using Monogum.BricksBucket.Core.Generics;

namespace Monogum.BricksBucket.Core.Examples.DynamicProperties
{

    /// <!-- Variable -->
    /// <summary>
    /// Dynamic variable that stores different types of data.
    /// </summary>
    [Serializable]
    public class DynamicValue : AbstractValue
    {

        #region Class Members

        /// <summary>
        /// Hashcode of this instance.
        /// </summary>
        private int _hashCode = -1;

        /// <summary>
        /// String variable.
        /// </summary>
        [SerializeField]
        protected string stringValue;

        /// <summary>
        /// Vector4 variable.
        /// </summary>
        [SerializeField]
        protected Vector4 vector;

        /// <summary>
        /// UnityEngine.Object variable.
        /// </summary>
        [SerializeField]
        protected Object asset;

        /// <summary>
        /// Animation curve value.
        /// </summary>
        [SerializeField]
        protected AnimationCurve curve = new AnimationCurve();

        /// <summary>
        /// Stores the type of variable.
        /// </summary>
        [SerializeField]
        protected DynamicValueType type;

        #endregion


        #region Accesors

        /// <summary>
        /// Type of variable.
        /// </summary>
        /// <returns>Type of the variable.</returns>
        public DynamicValueType Type
        {
            get => type;
            set
            {
                if (value != type)
                {
                    vector = Vector4.zero;
                    stringValue = string.Empty;
                    asset = null;
                    if (curve != null)
                        for (var i = 0; i < curve.keys.Length; i++)
                            curve.RemoveKey(i);
                }

                type = value;
            }
        }

        public Type SystemType
        {
            get => DynamicValueType2Type(Type);
            set => Type = Type2DynamicValueType(value);
        }

        /// <summary>
        /// Boolean value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public bool Boolean
        {
            get => (vector.x > 0 || vector.x < 0);
            set => vector.x = value ? 1 : 0;
        }

        /// <summary>
        /// Integer value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public int Integer
        {
            get => (int) vector.x;
            set => vector.x = value;
        }

        /// <summary>
        /// Float value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public float Float
        {
            get => vector.x;
            set => vector.x = value;
        }

        /// <summary>
        /// Double value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public double Double
        {
            get => vector.x;
            set => vector.x = (float) value;
        }

        /// <summary>
        /// Vector2 value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Vector2 Vector2
        {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Vector3 value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Vector3 Vector3
        {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Vector4 value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Vector4 Vector4
        {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Color value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Color Color
        {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Quaternion value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Quaternion Quaternion
        {
            get => new Quaternion(
                vector.x,
                vector.y,
                vector.z,
                vector.w
            );
            set => vector = new Vector4(value.x, value.y, value.z, value.w);
        }

        /// <summary>
        /// String value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public string String
        {
            get => stringValue;
            set => stringValue = value;
        }

        /// <summary>
        /// Object value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Object Asset
        {
            get => asset;
            set => asset = value;
        }

        /// <summary>
        /// Animation Curve value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public AnimationCurve Curve
        {
            get => curve;
            set => curve = value;
        }

        /// <summary>
        /// Whether the variable is nil.
        /// </summary>
        /// <returns><value>TRUE</value> if the variable is nil.</returns>
        public bool IsNil => Type == DynamicValueType.NULL;

        /// <summary>
        /// Hashcode of this instance.
        /// </summary>
        /// <returns><value>-1</value> if the variable is empty.</returns>
        public int HashCode
        {
            get => _hashCode;
            private set => _hashCode = value;
        }

        #endregion


        #region Class Implementation

        #region Constructuctors

        /// <summary>
        /// Creates a new instance of a Variable.
        /// </summary>
        public DynamicValue()
        {
            Type = DynamicValueType.NULL;
            Vector4 = Vector4.zero;
            String = string.Empty;
            Asset = null;
            Curve = new AnimationCurve();
            HashCode = -1;
            for (var i = 0; i < curve.keys.Length; i++)
                curve.RemoveKey(i);
        }

        /// <summary>
        /// Clones this instance, overriding the "readonly".
        /// </summary>
        public DynamicValue Clone()
        {
            var v = new DynamicValue
            {
                Vector4 = vector,
                String = stringValue,
                Curve = curve,
                Asset = asset,
                HashCode = HashCode,
                Type = type,
            };
            return v;
        }

        #endregion


        #region Overrieded Methods

        /// <inheritdoc cref="AbstractValue.Set"/>
        public override void Set(object value)
        {
            switch (Type)
            {
                case DynamicValueType.NULL:
                    return;
                
                case DynamicValueType.BOOLEAN:
                    Boolean = (bool) Convert.ChangeType(
                        value,
                        typeof(bool)
                    );
                    break;
                
                case DynamicValueType.INTEGER:
                    Integer = (int) Convert.ChangeType(
                        value,
                        typeof(int)
                    );
                    break;
                
                case DynamicValueType.FLOAT:
                    Float = (float) Convert.ChangeType(
                        value,
                        typeof(float)
                    );
                    break;
                
                case DynamicValueType.DOUBLE:
                    Double = (double) Convert.ChangeType(
                        value,
                        typeof(double)
                    );
                    break;
                
                case DynamicValueType.VECTOR2:
                    Vector2 = (Vector2) Convert.ChangeType(
                        value,
                        typeof(Vector2)
                    );
                    break;
                
                case DynamicValueType.VECTOR3:
                    Vector3 = (Vector3) Convert.ChangeType(
                        value,
                        typeof(Vector3)
                    );
                    break;
                
                case DynamicValueType.VECTOR4:
                    Vector4 = (Vector4) Convert.ChangeType(
                        value,
                        typeof(Vector4)
                    );
                    break;
                
                case DynamicValueType.QUATERNION:
                    Quaternion = (Quaternion) Convert.ChangeType(
                        value,
                        typeof(Quaternion)
                    );
                    break;
                
                case DynamicValueType.COLOR:
                    Color = (Color) Convert.ChangeType(
                        value,
                        typeof(Color)
                    );
                    break;
                
                case DynamicValueType.CURVE:
                    Curve = (AnimationCurve) Convert.ChangeType(
                        value,
                        typeof(AnimationCurve)
                    );
                    break;
                
                case DynamicValueType.STRING:
                    String = (string) Convert.ChangeType(
                        value,
                        typeof(string)
                    );
                    break;
                
                case DynamicValueType.ASSET:
                    if (value is Object o)
                    {
                        Asset = o;
                    }
                    break;
                
                default:
                    return;
            }
        }

        /// <inheritdoc cref="AbstractValue.Get"/>
        public override object Get(Type desiredType)
        {
            var desiredDynamicValueType = Type2DynamicValueType(desiredType);
            switch (desiredDynamicValueType)
            {
                case DynamicValueType.NULL:
                    return Boolean;
                case DynamicValueType.BOOLEAN:
                    return Integer;
                case DynamicValueType.INTEGER:
                    return Float;
                case DynamicValueType.FLOAT:
                    break;
                case DynamicValueType.DOUBLE:
                    return Double;
                case DynamicValueType.VECTOR2:
                    return Vector2;
                case DynamicValueType.VECTOR3:
                    return Vector3;
                case DynamicValueType.VECTOR4:
                    return Vector4;
                case DynamicValueType.QUATERNION:
                    return Quaternion;
                case DynamicValueType.COLOR:
                    return Color;
                case DynamicValueType.CURVE:
                    return Curve;
                case DynamicValueType.STRING:
                    return String;
                case DynamicValueType.ASSET:
                    return Asset;
                default:
                    return null;
            }
            return null;
        }

        /// <inheritdoc cref="System.object"/>
        public override bool Equals(object obj)
        {
            if (!(obj is DynamicValue other))
                return false;

            if (other.Type == Type && Type == DynamicValueType.NULL)
                return true;

            if (other.Type != Type)
                return false;

            switch (Type)
            {
                case DynamicValueType.NULL:
                    return true;

                case DynamicValueType.BOOLEAN:
                case DynamicValueType.DOUBLE:
                case DynamicValueType.INTEGER:
                case DynamicValueType.FLOAT:
                case DynamicValueType.VECTOR2:
                case DynamicValueType.VECTOR3:
                case DynamicValueType.VECTOR4:
                case DynamicValueType.QUATERNION:
                case DynamicValueType.COLOR:
                    return Vector4 == other.Vector4;

                case DynamicValueType.STRING:
                    return String == other.String;

                case DynamicValueType.CURVE:
                    return Curve == other.Curve;

                case DynamicValueType.ASSET:
                    return Asset == other.Asset;

                default:
                    return ReferenceEquals(this, other);
            }
        }

        /// <inheritdoc cref="System.object.ToString"/>
        public override string ToString() =>
            Type switch
            {
                DynamicValueType.NULL => "Null",
                DynamicValueType.BOOLEAN => Boolean.ToString().ToLower(),
                DynamicValueType.DOUBLE => Float.ToString(CultureInfo
                    .InvariantCulture),
                DynamicValueType.INTEGER => Float.ToString(CultureInfo
                    .InvariantCulture),
                DynamicValueType.FLOAT => Float.ToString(CultureInfo
                    .InvariantCulture),
                DynamicValueType.VECTOR2 => Vector2.ToString(),
                DynamicValueType.VECTOR3 => Vector3.ToString(),
                DynamicValueType.VECTOR4 => Vector4.ToString(),
                DynamicValueType.QUATERNION => Quaternion.ToString(),
                DynamicValueType.COLOR => Color.ToString(),
                DynamicValueType.STRING => String,
                DynamicValueType.ASSET => Asset.ToString(),
                DynamicValueType.CURVE => Curve.ToString(),
                _ => string.Empty
            };

        /// <inheritdoc cref="System.object.GetHashCode"/>
        public override int GetHashCode()
        {
            if (HashCode != -1)
                return HashCode;

            var baseValue = ((int) (Type)) << 27;

            switch (Type)
            {

                case DynamicValueType.NULL:
                    HashCode = 0;
                    break;

                case DynamicValueType.BOOLEAN:
                    HashCode = Boolean ? 1 : 2;
                    break;

                case DynamicValueType.DOUBLE:
                case DynamicValueType.INTEGER:
                case DynamicValueType.FLOAT:
                    HashCode = baseValue ^ Double.GetHashCode();
                    break;

                case DynamicValueType.STRING:
                    HashCode = baseValue ^ String.GetHashCode();
                    break;

                case DynamicValueType.VECTOR2:
                    HashCode = baseValue ^ Vector2.GetHashCode();
                    break;

                case DynamicValueType.VECTOR3:
                    HashCode = baseValue ^ Vector3.GetHashCode();
                    break;

                case DynamicValueType.VECTOR4:
                    HashCode = baseValue ^ Vector4.GetHashCode();
                    break;

                case DynamicValueType.QUATERNION:
                    HashCode = baseValue ^ Quaternion.GetHashCode();
                    break;

                case DynamicValueType.COLOR:
                    HashCode = baseValue ^ Color.GetHashCode();
                    break;

                case DynamicValueType.ASSET:
                    HashCode = baseValue ^ Asset.GetHashCode();
                    break;

                case DynamicValueType.CURVE:
                    HashCode = baseValue ^ Curve.GetHashCode();
                    break;

                default:
                    HashCode = 999;
                    break;
            }

            return HashCode;
        }

        #endregion

        public static DynamicValueType Type2DynamicValueType(Type type) =>
            type == typeof(bool) ? DynamicValueType.BOOLEAN :
            type == typeof(int) ? DynamicValueType.INTEGER :
            type == typeof(float) ? DynamicValueType.FLOAT :
            type == typeof(double) ? DynamicValueType.DOUBLE :
            type == typeof(Vector2) ? DynamicValueType.VECTOR2 :
            type == typeof(Vector3) ? DynamicValueType.VECTOR3 :
            type == typeof(Vector4) ? DynamicValueType.VECTOR4 :
            type == typeof(Quaternion) ? DynamicValueType.QUATERNION :
            type == typeof(Color) ? DynamicValueType.COLOR :
            type == typeof(string) ? DynamicValueType.STRING :
            type == typeof(AnimationCurve) ? DynamicValueType.CURVE :
            type == typeof(Object) ? DynamicValueType.ASSET :
            DynamicValueType.NULL;

        public static Type DynamicValueType2Type(DynamicValueType type) =>
            type switch
            {
                DynamicValueType.NULL => null,
                DynamicValueType.BOOLEAN => typeof(bool),
                DynamicValueType.INTEGER => typeof(int),
                DynamicValueType.FLOAT => typeof(float),
                DynamicValueType.DOUBLE => typeof(double),
                DynamicValueType.VECTOR2 => typeof(Vector2),
                DynamicValueType.VECTOR3 => typeof(Vector3),
                DynamicValueType.VECTOR4 => typeof(Vector4),
                DynamicValueType.QUATERNION => typeof(Quaternion),
                DynamicValueType.COLOR => typeof(Color),
                DynamicValueType.CURVE => typeof(AnimationCurve),
                DynamicValueType.STRING => typeof(string),
                DynamicValueType.ASSET => typeof(Object),
                _ => null
            };

        #endregion

    }
}
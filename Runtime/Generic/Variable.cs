using System;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BricksBucket.Core.Generic {

    /// <!-- Variable -->
    /// <summary>
    /// Dynamic variable that stores different types of data.
    /// </summary>
    [Serializable]
    public class Variable : IVariable {
        
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
        protected AnimationCurve curve = new AnimationCurve ();

        /// <summary>
        /// Stores the type of variable.
        /// </summary>
        [SerializeField]
        protected DataType type;

        #endregion
        
        #region Accesors

        /// <summary>
        /// Type of variable.
        /// </summary>
        /// <returns>Type of the variable.</returns>
        public DataType Type {
            get => type;
            set {
                if (value != type) {
                    vector = Vector4.zero;
                    stringValue = string.Empty;
                    asset = null;
                    if (curve != null)
                        for (var i = 0; i < curve.keys.Length; i++)
                            curve.RemoveKey (i);
                }
                type = value;
            }
        }

        /// <summary>
        /// Boolean value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public bool Boolean {
            get => (vector.x > 0 || vector.x < 0);
            set => vector.x = value ? 1 : 0;
        }

        /// <summary>
        /// Integer value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public int Integer {
            get => (int)vector.x;
            set => vector.x = value;
        }

        /// <summary>
        /// Float value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public float Float {
            get => vector.x;
            set => vector.x = value;
        }

        /// <summary>
        /// Double value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public double Double {
            get => vector.x;
            set => vector.x = (float)value;
        }

        /// <summary>
        /// Vector2 value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Vector2 Vector2 {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Vector3 value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Vector3 Vector3 {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Vector4 value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Vector4 Vector4 {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Color value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Color Color {
            get => vector;
            set => vector = value;
        }

        /// <summary>
        /// Quaternion value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Quaternion Quaternion {
            get => new Quaternion (
                vector.x,
                vector.y,
                vector.z,
                vector.w
            );
            set => vector = new Vector4 (value.x, value.y, value.z, value.w);
        }

        /// <summary>
        /// String value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public string String {
            get => stringValue;
            set => stringValue = value;
        }

        /// <summary>
        /// Object value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public Object Asset {
            get => asset;
            set => asset = value;
        }

        /// <summary>
        /// Animation Curve value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        public AnimationCurve Curve {
            get => curve;
            set => curve = value;
        }

        /// <summary>
        /// Whether the variable is declared as read only.
        /// </summary>
        /// <returns><value>TRUE</value> if the variable can only be read
        /// </returns>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Whether the variable is nil.
        /// </summary>
        /// <returns><value>TRUE</value> if the variable is nil.</returns>
        public bool IsNil => Type == DataType.NULL;

        /// <summary>
        /// Hashcode of this instance.
        /// </summary>
        /// <returns><value>-1</value> if the variable is empty.</returns>
        public int HashCode {
            get => _hashCode;
            private set => _hashCode = value;
        }

        #endregion

        #region Class Implementation

        /// <summary>
        /// Creates a new instance of a Variable.
        /// </summary>
        /// <param name="readOnly">Whether the variable must be
        /// read only.</param>
        public Variable (bool readOnly = false) {
            Type = DataType.NULL;
            Vector4 = Vector4.zero;
            String = string.Empty;
            Asset = null;
            Curve = new AnimationCurve ();
            HashCode = -1;
            ReadOnly = readOnly;
            for (var i = 0; i < curve.keys.Length; i++)
                curve.RemoveKey (i);
        }

        /// <summary>
        /// Clones this instance, overriding the "readonly".
        /// </summary>
        public Variable Clone (bool readOnly) {
            var v = new Variable {
                Vector4 = vector,
                String = stringValue,
                Curve = curve,
                Asset = asset,
                HashCode = HashCode,
                Type = type,
                ReadOnly = readOnly
            };
            return v;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        public Variable Clone () => Clone (ReadOnly);

        /// <summary>
        /// Returns this value as readonly
        /// </summary>
        public Variable AsReadOnly () => ReadOnly ? this : Clone (true);

        /// <summary>
        /// Clones this instance, returning it as writable.
        /// </summary>
        public Variable AsWritable () => Clone (false);

        /// <summary>
        /// Performs an assignment with the specified value.
        /// </summary>
        public void Assign (Variable value) {
            if (ReadOnly)
                throw new Exception ("Assigning on r-value");

            Vector4 = value.vector;
            String = value.stringValue;
            Asset = value.asset;
            Curve = value.curve;
            Type = value.Type;
            HashCode = -1;
        }

        /// <summary>
        /// Sets the value of the variable with the specified value.
        /// </summary>
        public void Set (object value) {

            if (ReadOnly)
                throw new Exception ("Writing on r-value");

            if (value == null) {
                Type = DataType.NULL;
                return;
            }

            var desiredType = value.GetType ();

            if (desiredType == typeof (bool)) {
                Type = DataType.BOOLEAN;
                Boolean = (bool)Convert.ChangeType (
                    value,
                    typeof (bool)
                );
            } else if (desiredType == typeof (int)) {
                Type = DataType.INTEGER;
                Integer = (int)Convert.ChangeType (
                    value,
                    typeof (int)
                );
            } else if (desiredType == typeof (float)) {
                Type = DataType.FLOAT;
                Float = (float)Convert.ChangeType (
                    value,
                    typeof (float)
                );
            } else if (desiredType == typeof (double)) {
                Type = DataType.DOUBLE;
                Double = (double)Convert.ChangeType (
                    value,
                    typeof (double)
                );
            } else if (desiredType == typeof (Vector2)) {
                Type = DataType.VECTOR2;
                Vector2 = (Vector2)Convert.ChangeType (
                    value,
                    typeof (Vector2)
                );
            } else if (desiredType == typeof (Vector3)) {
                Type = DataType.VECTOR3;
                Vector3 = (Vector3)Convert.ChangeType (
                    value,
                    typeof (Vector3)
                );
            } else if (desiredType == typeof (Vector4)) {
                Type = DataType.VECTOR4;
                Vector4 = (Vector4)Convert.ChangeType (
                    value,
                    typeof (Vector4)
                );
            } else if (desiredType == typeof (Quaternion)) {
                Type = DataType.QUATERNION;
                Quaternion = (Quaternion)Convert.ChangeType (
                    value,
                    typeof (Quaternion)
                );
            } else if (desiredType == typeof (Color)) {
                Type = DataType.COLOR;
                Color = (Color)Convert.ChangeType (
                    value,
                    typeof (Color)
                );
            } else if (desiredType == typeof (string)) {
                Type = DataType.STRING;
                String = (string)Convert.ChangeType (
                    value,
                    typeof (string)
                );
            } else if (desiredType == typeof (AnimationCurve)) {
                Type = DataType.CURVE;
                Curve = (AnimationCurve)Convert.ChangeType (
                    value,
                    typeof (AnimationCurve)
                );
            } else {
                try {
                    Type = DataType.ASSET;
                    Asset = value as Object;
                }
                catch (Exception e) {
                    Debug.LogWarning (e);
                }
            }
        }

        /// <summary>
        /// Returns the value of the variable to an object of a T type.
        /// </summary>
        /// <typeparam name="T">Desired type.</typeparam>
        /// <returns>The value of the variable of the type T.</returns>
        public T Get<T> () {
            return (T)Get (typeof (T));
        }

        /// <summary>
        /// Returns this variable to an object of desired type.
        /// </summary>
        /// <param name="desiredType">Desired Type.</param>
        /// <returns>Value of the variable.</returns>
        /// <exception cref="Exception">Throw exception fi the data type
        /// property is corrupted.</exception>
        public object Get (Type desiredType) {

            if (ReadOnly)
                throw new Exception ("Writing on r-value");

            if (desiredType == typeof (bool))
                return Boolean;

            if (desiredType == typeof (int))
                return Integer;

            if (desiredType == typeof (float))
                return Float;

            if (desiredType == typeof (double))
                return Double;

            if (desiredType == typeof (Vector2))
                return Vector2;

            if (desiredType == typeof (Vector3))
                return Vector3;

            if (desiredType == typeof (Vector4))
                return Vector4;

            if (desiredType == typeof (Quaternion))
                return Quaternion;

            if (desiredType == typeof (Color))
                return Color;

            if (desiredType == typeof (string))
                return String;

            if (desiredType == typeof (AnimationCurve))
                return Curve;

            return Asset;
        }

        /// <inheritdoc cref="System.object"/>
        public override bool Equals (object obj) {
            if (!(obj is Variable other))
                return false;

            if (other.Type == Type && Type == DataType.NULL)
                return true;

            if (other.Type != Type)
                return false;

            switch (Type) {
                case DataType.NULL:
                    return true;

                case DataType.BOOLEAN:
                case DataType.DOUBLE:
                case DataType.INTEGER:
                case DataType.FLOAT:
                case DataType.VECTOR2:
                case DataType.VECTOR3:
                case DataType.VECTOR4:
                case DataType.QUATERNION:
                case DataType.COLOR:
                    return Vector4 == other.Vector4;

                case DataType.STRING:
                    return String == other.String;

                case DataType.CURVE:
                    return Curve == other.Curve;

                case DataType.ASSET:
                    return Asset == other.Asset;

                default:
                    return ReferenceEquals (this, other);
            }
        }

        /// <inheritdoc cref="System.object.ToString"/>
        public override string ToString () {
            switch (Type) {

                case DataType.NULL:
                    return "Null";

                case DataType.BOOLEAN:
                    return Boolean.ToString ().ToLower ();

                case DataType.DOUBLE:
                case DataType.INTEGER:
                case DataType.FLOAT:
                    return Float.ToString (
                        CultureInfo.InvariantCulture
                    );

                case DataType.VECTOR2:
                    return Vector2.ToString ();

                case DataType.VECTOR3:
                    return Vector3.ToString ();

                case DataType.VECTOR4:
                    return Vector4.ToString ();

                case DataType.QUATERNION:
                    return Quaternion.ToString ();

                case DataType.COLOR:
                    return Color.ToString ();

                case DataType.STRING:
                    return String;

                case DataType.ASSET:
                    return Asset.ToString ();

                case DataType.CURVE:
                    return Curve.ToString ();

                default:
                    return string.Empty;
            }
        }

        /// <inheritdoc cref="System.object.GetHashCode"/>
        public override int GetHashCode () {
            if (HashCode != -1)
                return HashCode;

            var baseValue = ((int)(Type)) << 27;

            switch (Type) {

                case DataType.NULL:
                    HashCode = 0;
                    break;

                case DataType.BOOLEAN:
                    HashCode = Boolean ? 1 : 2;
                    break;

                case DataType.DOUBLE:
                case DataType.INTEGER:
                case DataType.FLOAT:
                    HashCode = baseValue ^ Double.GetHashCode ();
                    break;

                case DataType.STRING:
                    HashCode = baseValue ^ String.GetHashCode ();
                    break;

                case DataType.VECTOR2:
                    HashCode = baseValue ^ Vector2.GetHashCode ();
                    break;

                case DataType.VECTOR3:
                    HashCode = baseValue ^ Vector3.GetHashCode ();
                    break;

                case DataType.VECTOR4:
                    HashCode = baseValue ^ Vector4.GetHashCode ();
                    break;

                case DataType.QUATERNION:
                    HashCode = baseValue ^ Quaternion.GetHashCode ();
                    break;

                case DataType.COLOR:
                    HashCode = baseValue ^ Color.GetHashCode ();
                    break;

                case DataType.ASSET:
                    HashCode = baseValue ^ Asset.GetHashCode ();
                    break;

                case DataType.CURVE:
                    HashCode = baseValue ^ Curve.GetHashCode ();
                    break;

                default:
                    HashCode = 999;
                    break;
            }

            return HashCode;
        }

        #endregion
        
    }
}
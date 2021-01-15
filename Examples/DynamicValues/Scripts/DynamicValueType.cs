namespace Monogum.BricksBucket.Core.Examples.DynamicProperties
{
    /// <!-- DataType -->
    /// <summary>
    /// Enumeration of possible data types.
    /// </summary>
    public enum DynamicValueType {
        /// <summary>
        /// Null value.
        /// </summary>
        NULL,
        
        /// <summary>
        /// Boolean value.
        /// </summary>
        BOOLEAN,
        
        /// <summary>
        /// Integer value.
        /// </summary>
        INTEGER,
        
        /// <summary>
        /// Float value.
        /// </summary>
        FLOAT,
        
        /// <summary>
        /// Double value.
        /// </summary>
        DOUBLE,
        
        /// <summary>
        /// Vector2 value.
        /// </summary>
        VECTOR2,
        
        /// <summary>
        /// Vector3 value.
        /// </summary>
        VECTOR3,
        
        /// <summary>
        /// Vector4 value.
        /// </summary>
        VECTOR4,
        
        /// <summary>
        /// Quaternion value.
        /// </summary>
        QUATERNION,
        
        /// <summary>
        /// Color value.
        /// </summary>
        COLOR,
        
        /// <summary>
        /// Animation Curve value.
        /// </summary>
        CURVE,
        
        /// <summary>
        /// String value.
        /// </summary>
        STRING,
        
        /// <summary>
        /// Asset reference.
        /// </summary>
        ASSET
    }
}
using System;

namespace BricksBucket.Core.Editor
{
    /// <!-- ReflectionExtensions -->
    /// 
    /// <summary>
    /// Extensions related to the reflection namespace.
    /// </summary>
    ///
    /// <!-- By Javier García | @jvrgms | 2020 -->
	public static class ReflectionExtensions
	{
        /// <summary> Whether the type is a bool. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a bool. </returns>
        public static bool IsBool (this Type type) =>
            type == typeof (bool);

        /// <summary> Whether the type is a sbyte. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a sbyte. </returns>
        public static bool IsSignedByte (this Type type) =>
            type == typeof (sbyte);

        /// <summary> Whether the type is a short. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an short. </returns>
        public static bool IsShort (this Type type) =>
            type == typeof (short);

        /// <summary> Whether the type is an int. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an int. </returns>
        public static bool IsInt (this Type type) =>
            type == typeof (int);

        /// <summary> Whether the type is a float. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a float. </returns>
        public static bool IsFloat (this Type type) =>
            type == typeof (float);

        /// <summary> Whether the type is a long. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an long. </returns>
        public static bool IsLong (this Type type) =>
            type == typeof (long);

        /// <summary> Whether the type is a decimal. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an decimal. </returns>
        public static bool IsDecimal (this Type type) =>
            type == typeof (decimal);

        /// <summary> Whether the type is a double. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a double. </returns>
        public static bool IsDouble (this Type type) =>
            type == typeof (double);

        /// <summary> Whether the type is a byte. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a byte. </returns>
        public static bool IsByte (this Type type) =>
            type == typeof (byte);

        /// <summary> Whether the type is an ushort. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an ushort. </returns>
        public static bool IsUnsignedShort (this Type type) =>
            type == typeof (ushort);

        /// <summary> Whether the type is a uint. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an uint. </returns>
        public static bool IsUnsignedInt (this Type type) =>
            type == typeof (uint);

        /// <summary> Whether the type is an ulong. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is an ulong. </returns>
        public static bool IsUnsignedLong (this Type type) =>
            type == typeof (ulong);

        /// <summary> Whether the type is a string. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a string. </returns>
        public static bool IsString (this Type type) =>
            type == typeof (string);

        /// <summary> Whether the type is a char. </summary>
        /// <param name="type"> Type to evaluate. </param>
        /// <returns> Whether the type is a char. </returns>
        public static bool IsChar (this Type type) =>
            type == typeof (char);
	}
}
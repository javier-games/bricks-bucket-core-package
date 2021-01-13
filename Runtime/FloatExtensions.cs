using UnityEngine;
using Monogum.BricksBucket.Core.Math;

namespace Monogum.BricksBucket.Core
{
    /// <!-- IntegerExtensions -->
    ///
    /// <summary>
    /// Collection of extension methods for the <see href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.single">
    /// System.Single</see> structure.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.single">
    /// System.Single</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class FloatExtensions
    {
        /// <summary> Infinity Value </summary>
        private const float Infinity = Mathf.Infinity;

        /// <summary>
        /// Pow to -1.
        /// </summary>
        /// <param name="x">Number to invert.</param>
        /// <returns>Inverse of the float.</returns>
        public static float Invert (this float x) =>
            x.Approximately (0) ? Infinity : Mathf.Pow (x, -1);

        /// <summary>
        /// Absolute Value.
        /// </summary>
        /// <param name="x">Number to absolute.</param>
        /// <returns>Absolute value.</returns>
        public static float Absolute (this float x) => Mathf.Abs (x);

        /// <summary>
        /// Return if the value approximates to zero.
        /// </summary>
        /// <param name="x">Number to validate.</param>
        /// <param name="compare">Value to use as comparision.</param>
        /// <returns>Whether approximates to zero or not.</returns>
        public static bool Approximately (this float x, float compare) =>
            Mathf.Approximately (x, compare);

        /// <summary>
        /// Evaluates if value is between zero and one.
        /// </summary>
        /// <param name="x">Number to validate.</param>
        /// <returns>Whether value is in range.</returns>
        public static bool InRange01 (this float x) => InRange (x, 0, 1);

        /// <summary>
        /// Evaluates if value is between min and max.
        /// </summary>
        /// <param name="x">Number to validate.</param>
        /// <param name="min">Minimum value in range.</param>
        /// <param name="max">Maximum value in range.</param>
        /// <returns>Whether value is in range.</returns>
        public static bool InRange (this float x, float min, float max) =>
            (x > min && x < max) ||
            (x >= min && x > max) ||
            (x > min && x <= max);

        /// <summary>
        /// Rounds a float with the specified method.
        /// </summary>
        /// <param name="x">Float to convert.</param>
        /// <param name="roundType">Round method to use.</param>
        /// <returns>Rounded value as int.</returns>
        public static int
            RoundToInt (this float x, RoundType roundType = RoundType.ROUND)
        {
            switch (roundType)
            {
                case RoundType.CEIL:
                    return Mathf.CeilToInt (x);

                case RoundType.FLOOR:
                    return Mathf.FloorToInt (x);

                default:
                    return Mathf.RoundToInt (x);
            }
        }

        /// <summary>
        /// Rounds a float with the specified method.
        /// </summary>
        /// <param name="x">Float to convert.</param>
        /// <param name="roundType">Round method to use.</param>
        /// <returns>Rounded value.</returns>
        public static float
            Round (this float x, RoundType roundType = RoundType.ROUND)
        {
            switch (roundType)
            {
                case RoundType.CEIL:
                    return Mathf.Ceil (x);

                case RoundType.FLOOR:
                    return Mathf.Floor (x);

                default:
                    return Mathf.Round (x);
            }
        }
    }
}
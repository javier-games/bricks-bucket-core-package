using UnityEngine;
using Monogum.BricksBucket.Core.Math;

namespace Monogum.BricksBucket.Core
{
    /// <!-- DoubleExtensions -->
    ///
    /// <summary>
    /// Collection of extension methods for the <see href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.double">
    /// System.Double</see> structure.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.double">
    /// System.Double</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class DoubleExtensions
    {

        /// <summary> Infinity Value </summary>
        private const float Infinity = Mathf.Infinity;

        /// <summary>
        /// Pow to -1.
        /// </summary>
        /// <param name="x">Number to invert.</param>
        /// <returns>Inverse of the float.</returns>
        public static double Invert (this double x) =>
            x.Approximately (0) ? Infinity : System.Math.Pow (x, -1);

        /// <summary>
        /// Absolute Value.
        /// </summary>
        /// <param name="x">Number to absolute.</param>
        /// <returns>Absolute value.</returns>
        public static double Absolute (this double x) => System.Math.Abs (x);

        /// <summary>
        /// Return if the value approximates to zero.
        /// </summary>
        /// <param name="x">Number to approximate.</param>
        /// <param name="compare">Compare value.</param>
        /// <returns>Whether approximates to zero or not.</returns>
        public static bool Approximately (this double x, double compare) =>
            x.CompareTo (compare) == 1;

        /// <summary>
        /// Evaluates if value is between zero and one.
        /// </summary>
        /// <param name="x">Number to validate.</param>
        /// <returns>Whether value is in range.</returns>
        public static bool InRange01 (this double x) => InRange (x, 0, 1);

        /// <summary>
        /// Evaluates if value is between min and max.
        /// </summary>
        /// <param name="x">Number to validate.</param>
        /// <param name="min">Minimum value in range.</param>
        /// <param name="max">Maximum value in range.</param>
        /// <returns>Whether value is in range.</returns>
        public static bool InRange (this double x, double min, double max) =>
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
            RoundToInt (this double x, RoundType roundType = RoundType.ROUND)
        {
            switch (roundType)
            {
                case RoundType.CEIL:
                    return (int) System.Math.Ceiling (x);

                case RoundType.FLOOR:
                    return (int) System.Math.Floor (x);

                default:
                    return (int) System.Math.Round (x);
            }
        }

        /// <summary>
        /// Rounds a float with the specified method.
        /// </summary>
        /// <param name="x">Float to convert.</param>
        /// <param name="roundType">Round method to use.</param>
        /// <returns>Rounded value.</returns>
        public static double
            Round (this double x, RoundType roundType = RoundType.ROUND)
        {
            switch (roundType)
            {
                case RoundType.CEIL:
                    return System.Math.Ceiling (x);

                case RoundType.FLOOR:
                    return System.Math.Floor (x);

                default:
                    return System.Math.Round (x);
            }
        }
    }
}
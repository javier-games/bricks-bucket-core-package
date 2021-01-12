using UnityEngine;
using Monogum.BricksBucket.Core.Math;

namespace Monogum.BricksBucket.Core
{
    /// <!-- VectorExtensions -->
    ///
    /// <summary>
    /// Collection of extension methods for Vector related structures.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.unity3d.com/ScriptReference/Vector3.html">
    /// UnityEngine.Vector3</seealso>
    /// <seealso href=
    /// "https://docs.unity3d.com/ScriptReference/Vector2.html">
    /// UnityEngine.Vector2</seealso>
    /// <seealso href=
    /// "https://docs.unity3d.com/ScriptReference/Vector3Int.html">
    /// UnityEngine.Vector3</seealso>
    /// <seealso href=
    /// "https://docs.unity3d.com/ScriptReference/Vector2Int.html">
    /// UnityEngine.Vector3</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class VectorExtensions
    {
        #region Vector2

        /// <summary>
        /// Set the value of X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">New value for X.</param>
        public static void SetX (this ref Vector2Int v, int x) => v.x = x;

        /// <summary>
        /// Set the value of Y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">New value for Y.</param>
        public static void SetY (this ref Vector2Int v, int y) => v.y = y;

        /// <summary>
        /// Add value to X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">Value to add at X.</param>
        public static void AddToX (this ref Vector2Int v, int x) => v.x += x;

        /// <summary>
        /// Add value to y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">Value to add at Y.</param>
        public static void AddToY (this ref Vector2Int v, int y) => v.y += y;

        /// <summary>
        /// Set the value of X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">New value for X.</param>
        public static void SetX (this ref Vector2 v, float x) => v.x = x;

        /// <summary>
        /// Set the value of Y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">New value for Y.</param>
        public static void SetY (this ref Vector2 v, float y) => v.y = y;

        /// <summary>
        /// Add value to X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">Value to add at X.</param>
        public static void AddToX (this ref Vector2 v, float x) => v.x += x;

        /// <summary>
        /// Add value to y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">Value to add at Y.</param>
        public static void AddToY (this ref Vector2 v, float y) => v.y += y;

        /// <summary>
        /// Converts to Vector2Int
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="type">Rounding method to use.</param>
        /// <returns>Rounded Vector.</returns>
        public static Vector2Int Vector2Int (this Vector2 v, RoundType type = 0)
        {
            int x = default;
            int y = default;

            switch (type)
            {
                case RoundType.ROUND:
                    x = Mathf.RoundToInt (v.x);
                    y = Mathf.RoundToInt (v.y);
                    break;
                case RoundType.CEIL:
                    x = Mathf.CeilToInt (v.x);
                    y = Mathf.CeilToInt (v.y);
                    break;
                case RoundType.FLOOR:
                    x = Mathf.FloorToInt (v.x);
                    y = Mathf.FloorToInt (v.y);
                    break;
            }

            return new Vector2Int (x, y);
        }

        /// <summary>
        /// Returns a premed of an array of vectors.
        /// </summary>
        /// <param name="collection">Collection of vectors.</param>
        /// <returns>Premed vector in an array.</returns>
        public static Vector2 Average (this Vector2[] collection)
        {
            var premed = Vector2.zero;
            var values = collection;
            for (int i = 0; i < values.Length; i++) premed += values[i];
            premed /= values.Length;
            return premed;
        }

        #endregion


        #region Vector3

        /// <summary>
        /// Set the value of X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">New value for X.</param>
        public static void SetX (this ref Vector3Int v, int x) => v.x = x;

        /// <summary>
        /// Set the value of Y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">New value for Y.</param>
        public static void SetY (this ref Vector3Int v, int y) => v.y = y;

        /// <summary>
        /// Set the value of Z.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="z">New value for Z.</param>
        public static void SetZ (this ref Vector3Int v, int z) => v.z = z;

        /// <summary>
        /// Set the value of X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">New value for X.</param>
        public static void SetX (this ref Vector3 v, float x) => v.x = x;

        /// <summary>
        /// Set the value of Y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">New value for Y.</param>
        public static void SetY (this ref Vector3 v, float y) => v.y = y;

        /// <summary>
        /// Set the value of Z.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="z">New value for Z.</param>
        public static void SetZ (this ref Vector3 v, float z) => v.z = z;

        /// <summary>
        /// Add value to X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">Value to add at X.</param>
        public static void AddToX (this ref Vector3Int v, int x) => v.x += x;

        /// <summary>
        /// Add value to Y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">Value to add at Y.</param>
        public static void AddToY (this ref Vector3Int v, int y) => v.y += y;

        /// <summary>
        /// Add value to Z.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="z">Value to add at Z.</param>
        public static void AddToZ (this ref Vector3Int v, int z) => v.z += z;

        /// <summary>
        /// Add value to X.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="x">Value to add at X.</param>
        public static void AddToX (this ref Vector3 v, float x) => v.x += x;

        /// <summary>
        /// Add value to Y.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="y">Value to add at Y.</param>
        public static void AddToY (this ref Vector3 v, float y) => v.y += y;

        /// <summary>
        /// Add value to Z.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="z">Value to add at Z.</param>
        public static void AddToZ (this ref Vector3 v, float z) => v.z += z;

        /// <summary>
        /// Converts to Vector3Int.
        /// </summary>
        /// <param name="v">Vector to modify.</param>
        /// <param name="type">Rounding method to use.</param>
        /// <returns>Rounded Vector.</returns>
        public static Vector3Int Vector3Int (this Vector3 v, RoundType type = 0)
        {
            int x = default;
            int y = default;
            int z = default;

            switch (type)
            {
                case RoundType.ROUND:
                    x = Mathf.RoundToInt (v.x);
                    y = Mathf.RoundToInt (v.y);
                    z = Mathf.RoundToInt (v.z);
                    break;
                case RoundType.CEIL:
                    x = Mathf.CeilToInt (v.x);
                    y = Mathf.CeilToInt (v.y);
                    z = Mathf.RoundToInt (v.z);
                    break;
                case RoundType.FLOOR:
                    x = Mathf.FloorToInt (v.x);
                    y = Mathf.FloorToInt (v.y);
                    z = Mathf.RoundToInt (v.z);
                    break;
            }

            return new Vector3Int (x, y, z);
        }

        /// <summary>
        /// Returns a premed of an array of vectors.
        /// </summary>
        /// <param name="collection">Collection of vectors.</param>
        /// <returns>Premed vector in an array.</returns>
        public static Vector3 Average (this Vector3[] collection)
        {
            var premed = Vector3.zero;
            var values = collection;
            for (int i = 0; i < values.Length; i++) premed += values[i];

            premed /= values.Length;

            return premed;
        }

        #endregion
    }
}
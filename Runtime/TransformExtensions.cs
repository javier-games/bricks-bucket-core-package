using UnityEngine;

namespace Monogum.BricksBucket.Core
{
    /// <!-- TransformExtensions -->
    ///
    /// <summary>
    /// Collection of extension methods for the <see href=
    /// "https://docs.unity3d.com/ScriptReference/Transform.html">Transform
    /// </see> class.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.unity3d.com/ScriptReference/Transform.html">
    /// UnityEngine.Transform</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class TransformExtensions
    {
        /// <summary>
        /// Set the value of X.
        /// </summary>
        /// <param name="transform">Transform of position to set.</param>
        /// <param name="x">New value for X.</param>
        public static void PositionSetX (this Transform transform, float x)
        {
            var position = transform.position;
            transform.position = new Vector3 (x, position.y, position.z);
        }

        /// <summary>
        /// Set the value of Y.
        /// </summary>
        /// <param name="transform">Transform of position to set.</param>
        /// <param name="y">New value for Y.</param>
        public static void PositionSetY (this Transform transform, float y)
        {
            var position = transform.position;
            transform.position = new Vector3 (position.x, y, position.z);
        }

        /// <summary>
        /// Set the value of Z.
        /// </summary>
        /// <param name="transform">Transform of position to set.</param>
        /// <param name="z">New value for Z.</param>
        public static void PositionSetZ (this Transform transform, float z)
        {
            var position = transform.position;
            transform.position = new Vector3 (position.x, position.y, z);
        }

        /// <summary>
        /// Add value to X.
        /// </summary>
        /// <param name="transform">Transform of position to set.</param>
        /// <param name="x">Value to add at X.</param>
        public static void PositionAddToX (this Transform transform, float x)
        {
            transform.PositionSetX (transform.position.x + x);
        }

        /// <summary>
        /// Add value to Y.
        /// </summary>
        /// <param name="transform">Transform of position to set.</param>
        /// <param name="y">Value to add at Y.</param>
        public static void PositionAddToY (this Transform transform, float y)
        {
            transform.PositionSetY (transform.position.y + y);
        }

        /// <summary>
        /// Add value to Z.
        /// </summary>
        /// <param name="transform">Transform of position to set.</param>
        /// <param name="z">Value to add at Z.</param>
        public static void PositionAddToZ (this Transform transform, float z)
        {
            transform.PositionSetZ (transform.position.z + z);
        }
    }
}
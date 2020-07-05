using System;
using UnityEngine;

namespace BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- PositiveOnlyAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Constrains the value to be positive only.
	/// </para>
	/// 
	/// <para>
	/// Based in the <see href="https://github.com/Deadcows/MyBox">MyBox
	/// project by @deadcows</see> and the original version by <see creh="
	/// https://github.com/Kaynn-Cahya">@Kaynn-Cahya</see>.
	/// </para>
	///
	/// </summary>
	///
	/// <example><code>
	/// using UnityEngine;
	/// using BricksBucket.Core.Attributes;
	///
	/// public class PositiveOnlyAttributeTest : MonoBehaviour
	/// {
	///   // Constrains the value to be positive only.
	///   [SerializeField, PositiveOnly]
	///   private int m_integer;
	///
	///   // Also for floats, doubles...
	///   [SerializeField, PositiveOnly]
	///   private float m_float;
	///
	///   // And vectors.
	///   [SerializeField, PositiveOnly]
	///   private Vector3 m_vector3;
	/// }
	/// </code></example>
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// <seealso href="https://github.com/Kaynn-Cahya">
	/// @Kaynn-Cahya</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class PositiveOnlyAttribute : PropertyAttribute { }
}
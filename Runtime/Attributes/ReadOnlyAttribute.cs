using System;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- ReadOnlyAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Show the value on inspector but disable its editability.
	/// </para>
	/// 
	/// <para>
	/// Based in the <see href="https://github.com/Deadcows/MyBox">MyBox
	/// project by @deadcows</see>.
	/// </para>
	///
	/// </summary>
	///
	/// <example><code>
	/// using UnityEngine;
	/// using BricksBucket.Core.Attributes;
	///
	/// public class ReadOnlyAttributeTest : MonoBehaviour
	/// {
	///   // Displays the string but disable its editability on inspector.
	///   [SerializeField, ReadOnly]
	///   private string m_string = "You can only edit me by code.";
	/// }
	/// </code></example>
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class ReadOnlyAttribute : PropertyAttribute { }
}
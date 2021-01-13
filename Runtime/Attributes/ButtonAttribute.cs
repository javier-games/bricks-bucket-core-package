using System;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- ButtonAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Easy creates a button on inspector. The functionallity for this
	/// attribute is canceled by <see href="https://odininspector.com">
	/// Odin Inspector</see>, do not use both together.
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
	/// public class ButtonAttributeTest : MonoBehaviour
	/// {
	///   [Button]
	///   public void MyMethod ()
	///   {
	///     Debug.Log ("My Method has been pressed.");
	///   }
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
	[AttributeUsage (AttributeTargets.Method)]
	public class ButtonAttribute : PropertyAttribute { }
}
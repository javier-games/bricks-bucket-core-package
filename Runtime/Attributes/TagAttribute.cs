using System;
using UnityEngine;

namespace BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- TagAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Shows a menu with tags for a string field.
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
	/// public class TagAttribute : MonoBehaviour
	/// {
	///   // Shows a menu with the existing tags.
	///   [SerializeField, Tag]
	///   private string m_tag;
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
	public class TagAttribute : PropertyAttribute { }
}
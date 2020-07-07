using System;
using UnityEngine;

namespace BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- SpriteLayerAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Creates Popup with sprite layers available.
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
	/// public class SpriteLayerAttributeTest : MonoBehaviour
	/// {
	///   // Shows the available sprite layers.
	///   [SerializeField, SpriteLayer]
	///   private int m_spriteLayer;
	/// }
	/// </code></example>
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class SpriteLayerAttribute : PropertyAttribute { }
}
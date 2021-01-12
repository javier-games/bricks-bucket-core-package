using System;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- AutoFillAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Automatically assign components from the Game Object on scene save.
	/// </para>
	/// 
	/// <para>
	/// Based in the <see href="https://github.com/Deadcows/MyBox">MyBox
	/// project by @deadcows</see>.
	/// </para>
	///
	/// </summary>
	///
	/// <example>
	/// <code>
	/// using UnityEngine;
	/// using BricksBucket.Core.Attributes;
	/// 
	/// public class AutoFillAttributeTest : MonoBehaviour
	/// {
	///   // The variable m_myTransform is assigned when the scene
	///   // is saved.
	///   [SerializeField, AutoProperty]
	///   public Transform m_transform;
	/// }
	/// </code>
	/// </example>
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class AutoFillAttribute : PropertyAttribute { }
}
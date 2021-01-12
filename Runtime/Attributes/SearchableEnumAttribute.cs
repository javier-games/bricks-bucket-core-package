using System;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- SearchableEnumAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Adds a search menu to the enum dropdown.
	/// </para>
	/// 
	/// <para>
	/// Based in the <see href=
	/// "https://github.com/roboryantron/UnityEditorJunkie">UnityEditorJunkie
	/// project by @roboryantron</see>.
	/// </para>
	///
	/// </summary>
	///
	/// <example><code>
	/// using UnityEngine;
	/// using BricksBucket.Core.Attributes;
	///
	/// public class SearchableEnumAttributeTest : MonoBehaviour
	/// {
	///   private enum SomeEnum { A, B, C }
	///
	///   // Adds a search menu to the enum dropdown.
	///   [SerializeField, SearchableEnum]
	///   private SomeEnum m_enum;
	/// }
	/// </code></example>
	///
	/// <seealso href="https://github.com/roboryantron/UnityEditorJunkie">
	/// roboryantron/UnityEditorJunkie</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class SearchableEnumAttribute : PropertyAttribute { }
}
using System;
using UnityEngine;

namespace BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- ShowIfAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Hides or shows the property according to the value of other property.
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
	/// public class ShowIfAttributeTest : MonoBehaviour
	/// {
	///   [SerializeField]
	///   private bool m_showMyString;
	///
	///   // The property is showed accordin to de value of m_showMyString.
	///   [SerializeField]
	///   [ShowIf("m_showMyString")]
	///   private string m_myString;
	/// }
	/// </code></example>
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class ShowIfAttribute : PropertyAttribute
	{
		#region Fields

		/// <summary> Name of the property to check. </summary>
		public readonly string propertyToCheck;

		/// <summary> Value to compare. </summary>
		public readonly object compareValue;

		/// <summary> Inverts the behaviour. </summary>
		public readonly bool inverse;

		#endregion

        
		#region Methods

		/// <summary> Creates an instance of this attribute. </summary>
		/// <param name="propertyToCheck"> Property to check. </param>
		/// <param name="compareValue"> Use this value to compare. </param>
		/// <param name="inverse"> Inverse the behaviour. </param>
		public ShowIfAttribute (
			string propertyToCheck,
			object compareValue = null,
			bool inverse = false
		)
		{
			this.propertyToCheck = propertyToCheck;
			this.compareValue = compareValue;
			this.inverse = inverse;
		}

		#endregion
	}
}
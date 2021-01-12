using System;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- EnumMaskAttribute -->
	///
	/// <summary>
	/// Shows an int field as a mask for enums.
	/// </summary>
	///
	/// <example><code>
	/// using UnityEngine;
	/// using BricksBucket.Core.Attributes;
	///
	/// public class EnumMaskAttributeTest : MonoBehaviour
	/// {
	///   public enum SomeEnum { A, B, C, }
	///
	///   // Shows this enum as a mask.
	///   [SerializeField, EnumMask(typeof(SomeEnum))]
	///   private int m_enumMask;
	/// }
	/// </code></example>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class EnumMaskAttribute : PropertyAttribute
	{
		#region Fields

		/// <summary> Reference to the type of enum. </summary>
		public readonly Type enumType;

		#endregion

        
		#region Methods

		/// <summary> Creates a new instance of attribute. </summary>
		/// <param name="enumType"> Type of enum. </param>
		public EnumMaskAttribute (Type enumType) => this.enumType = enumType;

		#endregion
	}
}
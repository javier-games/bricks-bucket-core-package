using System;
using UnityEngine;

namespace BricksBucket.Core.Attributes
{
	// ReSharper disable CommentTypo
	/// <!-- DefinedValuesAttribute -->
	///
	/// <summary>
	///
	/// <para>
	/// Creates Popup with predefined values for string, int or float property.
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
	/// BricksBucket.Core.Attributes;
	/// 
	/// public class DefinedValuesAttributeTest : MonoBehaviour
	/// {
	///   // Displays the defined values instead an input string.
	///   [SerializeField, DefinedValues(1,1.5,2)]
	///   private string m_definedValue;
	/// }
	/// </code></example>
	/// 
	///
	/// <seealso href="https://github.com/Deadcows/MyBox">
	/// Deadcows/MyBox</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[AttributeUsage (AttributeTargets.Field)]
	public class DefinedValuesAttribute : PropertyAttribute
	{
		#region Fields

		/// <summary> Collection of defined values. </summary>
		public readonly object[] valuesArray;

		#endregion

        
		#region Methods

		/// <summary> Creates new instance of Defined Values. </summary>
		/// <param name="definedValues"> Defined values to use. </param>
		public DefinedValuesAttribute (params object[] definedValues) =>
			valuesArray = definedValues;

		#endregion
	}
}
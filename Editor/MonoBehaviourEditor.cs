using Monogum.BricksBucket.Core.Attributes;
using Monogum.BricksBucket.Core.Editor.Attributes;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Editor
{
	// ReSharper disable CommentTypo
	/// <!-- MonoBehaviourEditor -->
	/// 
	/// <summary>
	///
	/// <para>
	/// Override Editor for a general MonoBehaviour. Adds the functionality to
	/// display methods as buttons that use the <see
	/// cref="ButtonAttribute"/> on inspector.
	/// </para>
	///
	/// <para>
	/// This class is canceled by OdinInspector, do not use both together.
	/// </para>
	/// 
	/// </summary>
	///
	/// <seealso cref="ButtonAttribute"/>
	///
	/// <!-- By Javier García | @jvrgms | 2020 -->
	// ReSharper restore CommentTypo
	[CustomEditor (typeof (MonoBehaviour), true), CanEditMultipleObjects]
	public class MonoBehaviourEditor : UnityEditor.Editor
	{
		#region Properties

		/// <summary>
		/// Current target.
		/// </summary>
		private MonoBehaviour _target;

		/// <summary>
		/// Handler for methods.
		/// </summary>
		private ButtonAttributeHandler _handler;

		#endregion


		#region Methods

		/// <summary> Called on enable. </summary>
		private void OnEnable ()
		{
			_target = target as MonoBehaviour;
			if (_target == null) return;

			_handler = new ButtonAttributeHandler (_target);
		}

		/// <inheritdoc cref="UnityEditor.Editor.OnInspectorGUI"/>
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			_handler?.OnInspectorGUI ();
		}

		#endregion
	}
}
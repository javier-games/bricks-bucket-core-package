using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BricksBucket.Core.Editor.Attributes
{
	// ReSharper disable CommentTypo
    /// <!-- ButtonAttributeHandler -->
    /// <summary>
    ///
    /// <para>
    /// Handler for buttons.
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
    /// <seealso href="https://github.com/Deadcows/MyBox">
    /// Deadcows/MyBox</seealso>
    /// <seealso href="https://github.com/Kaynn-Cahya">
    /// @Kaynn-Cahya</seealso>
    ///
    /// <!-- By Javier García | @jvrgms | 2020 -->
    // ReSharper restore CommentTypo
    public class ButtonAttributeHandler
    {
		#region Fields
		
		/// <summary>
		/// List of methods in the target.
		/// </summary>
		private readonly List<(MethodInfo Method, string Name)> _targetMethods;

		/// <summary>
		/// Reference to the target;
		/// </summary>
		private readonly Object _target;
		
		#endregion

		
		#region Properties
		
		/// <summary>
		/// Amount of methods on list.
		/// </summary>
		public int Amount => _targetMethods?.Count ?? 0;
		
		#endregion
		
		
        #region Public Methods
		
		/// <summary>
		/// Create an instance of ButtonAttributeHandler;
		/// </summary>
		/// <param name="target">Object Target to display it methods.</param>
		public ButtonAttributeHandler (Object target)
		{
			_target = target;

			var type = target.GetType ();
			const BindingFlags bindings =
				BindingFlags.Instance |
				BindingFlags.Static |
				BindingFlags.Public |
				BindingFlags.NonPublic;
			var members = type.GetMembers (bindings).Where (IsButtonMethod);

			foreach (var member in members)
			{
				var method = member as MethodInfo;
				if (method == null) continue;

				if (!IsValidMember (method, member)) continue;
				if (_targetMethods == null)
					_targetMethods = new List<(MethodInfo, string)> ();
				_targetMethods.Add ((method, method.Name));
			}
		}

		/// <summary> Called oniInspector GUI to draw button. </summary>
		public void OnInspectorGUI ()
		{
			if (_targetMethods == null) return;
			EditorGUILayout.Space ();

			foreach (var method in _targetMethods)
			{
				if (GUILayout.Button (method.Name))
					InvokeMethod (_target, method.Method);
			}
		}

        #endregion


        #region Private Methods

		/// <summary> Invokes the method from the own target. </summary>
		/// <param name="method"> Method to call. </param>
		public void Invoke (MethodInfo method) =>
			InvokeMethod (_target, method);

        /// <summary> Invokes the method. </summary>
        /// <param name="target"> Current target. </param>
        /// <param name="method"> Method to call. </param>
        private static void InvokeMethod (Object target, MethodInfo method)
        {
            var result = method.Invoke (target, null);

            if (result != null)
            {
                // TODO: Implement Log in ButtonMethodAttributeEditor in Invoke.
                Debug.Log (
                    $"{result}\nResult of Method '{method.Name}' " +
                    $"called by {target.name}"
                );
            }
        }

        /// <summary> Validates a method. </summary>
        /// <param name="method"> Method to validate. </param>
        /// <param name="member"> Member of the method. </param>
        /// <returns> Whether the method is valid. </returns>
        private static bool IsValidMember (MethodInfo method, MemberInfo member)
        {
            if (method == null)
            {
                // TODO: Implement Log in ButtonMethodAttributeEditor in Invoke.
                Debug.LogWarning (
                    $"Property {member.Name}  is not a method button. " +
                    "Remove unnecessary EditorButtonAttribute. "
                );
                return false;
            }

            if (method.GetParameters ().Length <= 0) return true;
            
            // TODO: Implement Log in ButtonMethodAttributeEditor in Invoke.
            Debug.LogWarning (
                "Methods with parameters are not supported by " +
                $"EditorButtonAttribute at Method {method.Name}"
            );
            return false;

        }

        /// <summary> Validates whether a member is a button method. </summary>
        /// <param name="memberInfo"> Member info to validate. </param>
        /// <returns> Whether a member is a button method. </returns>
        private static bool IsButtonMethod (MemberInfo memberInfo) =>
            Attribute.IsDefined (memberInfo, typeof (ButtonAttribute));

        #endregion
    }
}
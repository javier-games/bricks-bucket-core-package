using System;
using System.Reflection;
using BricksBucket.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Editor.Attributes
{
    /// <summary>
    ///
    /// On Save Attribute Handler.
    ///
    /// <para>
    /// Called on will save assets to search for methods and call them.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2019 </para>
    ///
    /// </summary>
    [InitializeOnLoad]
    public static class OnSaveAttributeHandler
    {
        #region Methods

        /// <summary> Subscribing to editor events. </summary>
        static OnSaveAttributeHandler () =>
            EditorEvents.onSaveAssets += CheckComponents;

        /// <summary> Check for On Save method attributes. </summary>
        private static void CheckComponents ()
        {
            var components =
                Core.Editor.BucketEditor.GetMethodsWithAttribute<OnSaveAttribute> ();

            for (int i = 0; i < components.Length; i++)
                InvokeMethod (components[i].component, components[i].method);
        }

        /// <summary> Invokes the method. </summary>
        /// <param name="target"> Current target. </param>
        /// <param name="method"> Method to call. </param>
        private static void InvokeMethod (Component target, MethodInfo method)
        {
            if(!IsValidMethod(target, method))
                return;
            
            var result = method.Invoke (target, null);

            if (result != null)
            {
                // TODO: Implement Log method for OnSaveAttributeHandler.Invoke.
                Debug.Log(
                    $"{result}\nResult of Method '{method.Name}' " + 
                    "called by {target.name}",
                    target
                );
            }
        }

        /// <summary> Validates a method. </summary>
        /// <param name="target">Target to validate.</param>
        /// <param name="method"> Method to validate. </param>
        /// <returns> Whether the method is valid. </returns>
        private static bool IsValidMethod (Component target, MethodInfo method)
        {
            if (method == null)
            {
                // TODO: Implement Log method for OnSaveAttributeHandler.Valid.
                Debug.LogWarning (
                    $"Property {target.name}  is not a method button. " +
                    "Remove unnecessary EditorButtonAttribute. "
                );
                return false;
            }

            if (method.GetParameters ().Length > 0)
            {
                // TODO: Implement Log method for OnSaveAttributeHandler.Valid.
                Debug.LogWarning (
                    "Methods with parameters are not supported by " +
                    $"EditorButtonAttribute at Method {method.Name}",
                    target
                );
                return false;
            }

            if (IsOnSaveMethod (method)) return true;
            // TODO: Implement Log method for OnSaveAttributeHandler.Valid.
            Debug.LogWarning (
                "Methods with parameters are not supported by " +
                $"EditorButtonAttribute at Method {method.Name}",
                target
            );
            return false;

        }

        /// <summary> Validates whether a member is a on save method. </summary>
        /// <param name="memberInfo"> Member info to validate. </param>
        /// <returns> Whether a member is a button method. </returns>
        private static bool IsOnSaveMethod (MemberInfo memberInfo) =>
            Attribute.IsDefined (memberInfo, typeof (OnSaveAttribute));

        #endregion
    }
}
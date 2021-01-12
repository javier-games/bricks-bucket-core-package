using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using EditorProcessor = UnityEditor.AssetModificationProcessor;

// ReSharper disable UnassignedField.Global
namespace Monogum.BricksBucket.Core.Editor
{
    /// <!-- EditorEvents -->
    ///
    /// <summary>
    /// Events to manage editor scripts. 
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    // ReSharper disable once Unity.RedundantInitializeOnLoadAttribute
    [InitializeOnLoad]
    public class EditorEvents : EditorProcessor, IPreprocessBuildWithReport
    {
        /// <summary> Delegate called on save assets. </summary>
        public static Action onSaveAssets;

        /// <summary> Delegate called on entered edit mode. </summary>
        public static Action onEnteredEditMode;

        /// <summary> Delegate called on entered play mode. </summary>
        public static Action onEnteredPlayMode;

        /// <summary> Delegate called on exiting edit mode. </summary>
        public static Action onExitingEditMode;

        /// <summary> Delegate called on exiting play mode. </summary>
        public static Action onExitingPlayMode;


        /// <summary> Order to execute. </summary>
        public int callbackOrder => 0;

        /// <summary> Called on will save assets. </summary>
        /// <param name="paths"></param>
        /// <returns> Collection of Paths to save. </returns>
        private static string[] OnWillSaveAssets (string[] paths)
        {
            onSaveAssets?.Invoke ();
            return paths;
        }

        /// <summary>
        /// Called on play mode state changed.
        /// </summary>
        /// <param name="state"> Current state. </param>
        // ReSharper disable once UnusedMember.Local
        private static void OnPlayModeStateChanged (PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                onEnteredEditMode?.Invoke ();
                break;
                case PlayModeStateChange.EnteredPlayMode:
                onEnteredPlayMode?.Invoke ();
                break;
                case PlayModeStateChange.ExitingEditMode:
                onExitingEditMode?.Invoke ();
                break;
                case PlayModeStateChange.ExitingPlayMode:
                onExitingPlayMode?.Invoke ();
                break;
            }
        }

        /// <summary>Called after a build.</summary>
        /// <param name="report">Report of the build.</param>
        public void OnPreprocessBuild (BuildReport report)
        {

        }
    }
}

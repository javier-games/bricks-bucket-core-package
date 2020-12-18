using System;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Generic.Editor
{
    /// <!-- DynamicValuesWizard -->
    /// <summary>
    /// Displays a Wizard window to set up the hardwired scripts.
    /// </summary>
    public class DynamicValuesWizard : ScriptableWizard
    {
        #region Fields
        
        /// <summary>
        /// Local path where to put the scripts.
        /// </summary>
        [SerializeField]
        [Tooltip("Local path where to put the scripts.")]
        private string path = "";
        
        /// <summary>
        /// Name of the folder container.
        /// </summary>
        [SerializeField]
        [Tooltip("Name of the folder container")]
        private string folderName = "Custom Values";

        /// <summary>
        /// Name space where to put the scripts.
        /// </summary>
        [SerializeField]
        private string nameSpace = "MyNameSpace";

        #endregion

        #region Methods
        
        /// <summary>
        /// Creates a Custom Dynamic Environment.
        /// </summary>
        [MenuItem("Tools/BricksBucket/Create Custom Dynamic Values")]
        public static void CreateWizard()
        {
            DisplayWizard<DynamicValuesWizard>(
                "Create Custom Dynamic Values",
                "Create",
                "Cancel"
            );
        }

        /// <summary>
        /// Called after the wizard window is accepted.
        /// </summary>
        public void OnWizardCreate()
        {
            HardwiredFileWriter.CreateClasses(path + folderName, nameSpace);
        }

        /// <summary>
        /// Called when the second option is pressed.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnWizardOtherButton()
        {
            return;
        }

        #endregion
    }
}
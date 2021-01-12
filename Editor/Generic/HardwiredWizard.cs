using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Generic.Editor
{
    /// <!-- HardwiredWizard -->
    /// <summary>
    /// Displays a Wizard window to set up the hardwired scripts.
    /// </summary>
    public class HardwiredWizard : ScriptableWizard
    {
        #region Fields
        
        /// <summary>
        /// Local path where to put the scripts.
        /// </summary>
        [SerializeField]
        [Tooltip("Local path where to put the scripts with out assets folder.")]
        private string path = "";

        /// <summary>
        /// Name space where to put the scripts.
        /// </summary>
        [SerializeField]
        [Tooltip("Namespace of the class.")]
        private string nameSpace = "";

        /// <summary>
        /// Name of the hardwired class.
        /// </summary>
        [SerializeField]
        [Tooltip("Name of the hardwired class.")]
        private string className = "";

        /// <summary>
        /// Name space where to put the scripts.
        /// </summary>
        [SerializeField]
        [Tooltip("Extension of the file, with out the point.")]
        private string extension = "";

        #endregion

        #region Methods
        
        /// <summary>
        /// Creates a Custom Dynamic Environment.
        /// </summary>
        [MenuItem("Tools/BricksBucket/ComponentRegistry/Create Collection")]
        public static void CreateWizard()
        {
            DisplayWizard<HardwiredWizard>(
                "Create ComponentRegistry Component Collection",
                "Create",
                "Cancel"
            );
        }

        /// <summary>
        /// Called after the wizard window is accepted.
        /// </summary>
        public void OnWizardCreate()
        {
            HardwiredFileWriter.ResetFile(
                path,
                extension,
                nameSpace,
                className
            );
        }

        /// <summary>
        /// Called when the second option is pressed.
        /// </summary>
        public void OnWizardOtherButton()
        {
            // ReSharper disable once RedundantJumpStatement
            return;
        }

        #endregion
    }
}
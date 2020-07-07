using UnityEngine;

namespace BricksBucket.Core
{
    /// <!-- ScriptableSingleton -->
    /// 
    /// <summary>
    /// Generic singleton that inherits from scriptable object.
    /// </summary>
    /// 
    /// <typeparam name="T">Heiress Class Type.</typeparam>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public abstract class ScriptableSingleton<T> : ScriptableObject
    where T : ScriptableSingleton<T>
    {

        #region Constants

        /// <summary>
        /// Defines the default _folderPath in project folder.
        /// </summary>
        private const string DefaultFolderPath = "Assets/Resources";

        /// <summary>
        /// Defines the default extension for the scriptable object.
        /// </summary>
        private const string DefaultExtension = ".asset";

        #endregion


        #region Fields

        /// <summary>
        /// Scriptable Instance.
        /// </summary>
        private static T _instance;

        #endregion


        #region Properties

        /// <summary>
        /// Name of the file.
        /// </summary>
        /// <returns>Name of the type of the singleton as default.</returns>
        protected virtual string FileName => typeof (T).Name;

        /// <summary>
        /// Extension of the scriptable object asset.
        /// </summary>
        /// <returns><value>".asset"</value> as default.</returns>
        protected virtual string FileExtension => DefaultExtension;

        /// <summary>
        /// Path of the file's folder.
        /// </summary>
        /// <returns><value>"Assets/Resources"</value> as default.</returns>
        protected virtual string FolderPath => DefaultFolderPath;

        /// <summary>
        /// Complete path of the file.
        /// </summary>
        /// <returns>Path of the file.</returns>
        public string Path =>
            string.Concat (
                FolderPath, "/", FileName, FileExtension
            );

        /// <summary>
        /// Indicates whether an instance exists or not.
        /// </summary>
        /// <returns><value>Null</value> if the Instance does not exists.
        /// </returns>
        public static bool InstanceExist => _instance != null;

        /// <summary>
        /// Returns the scriptable instance.
        /// </summary>
        /// <returns>Always returns a value.</returns>
        // ReSharper disable once MemberCanBeProtected.Global
        public static T Instance
        {
            get
            {
                //  Returns the current instance if it exits.
                if (InstanceExist) return _instance;

                //  Try to find an instance on Resources Folder.
                var instances = Resources.LoadAll ("", typeof (T));
                if (instances != null && instances.Length > 0)
                {
                    if (instances.Length > 1)
                        Debug.LogWarning (
                            "There are more than one" + typeof (T));
                    _instance = instances[0] as T;
                }

                if (InstanceExist) return _instance;

                // Creates a new asset in Resources folder.
                _instance = ScriptableObject.CreateInstance<T> ();

#if UNITY_EDITOR
                var path = string.IsNullOrWhiteSpace (_instance.Path)
                    ? DefaultFolderPath
                    : _instance.Path;
                if (!UnityEditor.AssetDatabase.IsValidFolder (path))
                {
                    var segments = path.Split ('/');
                    var currentPath = segments[0];
                    for (int i = 1; i < segments.Length; i++)
                    {
                        var wanted = currentPath + "/" + segments[i];
                        if (UnityEditor.AssetDatabase.IsValidFolder (wanted))
                        {
                            currentPath = wanted;
                            continue;
                        }

                        string guid = UnityEditor.AssetDatabase.CreateFolder (
                            parentFolder: currentPath,
                            newFolderName: segments[i]
                        );
                        currentPath =
                            UnityEditor.AssetDatabase.GUIDToAssetPath (guid);
                    }

                    path = currentPath;
                }

                UnityEditor.AssetDatabase.CreateAsset (_instance, path);
#endif
                return _instance;
            }
        }

        #endregion
    }
}

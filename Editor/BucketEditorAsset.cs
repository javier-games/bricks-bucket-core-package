using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Editor
{
    /* ASSET BUCKET */
    public static partial class BucketEditor
    {
        /// <summary>
        /// Adds referenced sub assets.
        /// </summary>
        /// <param name="referencedAssets">Assets attempt to add.</param>
        /// <param name="allAssets">All current sub assets in asset.</param>
        /// <param name="asset">Asset origin.</param>
        public static void AddReferencedSubAssets (
            HashSet<Object> referencedAssets, Object[] allAssets, Object asset
        )
        {
            if (asset == null) return;

            if (!allAssets.Contains (asset)) return;

            if (!referencedAssets.Add (asset)) return;

            IEnumerable<SerializedProperty> children;
            using (var serializedObject = new SerializedObject (asset))
                children = serializedObject.EnumerateChildProperties ();
            foreach (var child in children)
                AddReferencedSubAssets (referencedAssets, allAssets, child);
        }

        /// <summary>
        /// Adds referenced sub assets.
        /// </summary>
        /// <param name="referencedAssets">Assets attempt to add.</param>
        /// <param name="allAssets">All current sub assets in asset.</param>
        /// <param name="property">Property of the asset origin.</param>
        public static void AddReferencedSubAssets (
            HashSet<Object> referencedAssets,
            Object[] allAssets,
            SerializedProperty property
        )
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
                AddReferencedSubAssets (
                    referencedAssets: referencedAssets,
                    allAssets: allAssets,
                    asset: property.objectReferenceValue
                );
            else
            {
                var children = property.EnumerateChildProperties ();
                var serializedProperties = children.ToList ();
                for (int i = 0; i < serializedProperties.Count (); i++)
                {
                    AddReferencedSubAssets (
                        referencedAssets: referencedAssets,
                        allAssets: allAssets,
                        asset: property.objectReferenceValue
                    );
                }

            }
        }

        /// <summary>
        /// Returns an scriptable asset instance of a given type at the
        /// specified type.
        /// </summary>
        /// <typeparam name="T">Type of the scriptable object.</typeparam>
        /// <param name="path">Path to look for a scriptable.</param>
        /// <returns><value>Null</value> if the object is not found.</returns>
        public static T GetScriptable<T> (string path)
        where T : ScriptableObject
        {
            var settings = !File.Exists (path)
                ? CreateScriptable<T> (path)
                : LoadScriptable<T> (path);

            if (settings == null)
            {
                DeleteFile (path);
                settings = CreateScriptable<T> (path);
            }

            settings.hideFlags = HideFlags.HideAndDontSave;
            return settings;
        }

        /// <summary>
        /// Loads an scriptable object at the given path.
        /// </summary>
        /// <typeparam name="T">Type of the scriptable object.</typeparam>
        /// <param name="path">Path to look for a scriptable.</param>
        /// <returns>Loaded Scriptable.</returns>
        public static T LoadScriptable<T> (string path)
        where T : ScriptableObject
        {
            T settingsInstance;
            try
            {
                settingsInstance = AssetDatabase.LoadAssetAtPath<T> (path);
            }
            catch (System.Exception e)
            {
                // TODO: Add method Log to AssetBucket.LoadScriptable.
                Debug.Log (e);
                settingsInstance = null;
            }

            return settingsInstance;
        }

        /// <summary>
        /// Creates a new scriptable asset file at given path.
        /// </summary>
        /// <typeparam name="T">Type of the scriptable object.</typeparam>
        /// <param name="path">Path to look for a scriptable.</param>
        /// <returns>Asset created.</returns>
        public static T CreateScriptable<T> (string path)
        where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T> ();
            SaveScriptable (instance, path);
            return instance;
        }

        /// <summary>
        /// Deletes an asset at the specific path.
        /// </summary>
        /// <param name="path">Path to look for a scriptable.</param>
        public static void DeleteFile (string path)
        {
            if (!File.Exists (path)) return;

            var attributes = File.GetAttributes (path);
            var bitwise = attributes & FileAttributes.ReadOnly;
            if (bitwise == FileAttributes.ReadOnly)
                File.SetAttributes (
                    path: path,
                    fileAttributes: attributes & ~FileAttributes.ReadOnly
                );

            File.Delete (path);
        }

        /// <summary>
        /// Saves the scriptable instance at the given path.
        /// </summary>
        /// <typeparam name="T">Type of the scriptable object.</typeparam>
        /// <param name="instance">Instance to save.</param>
        /// <param name="path">Path where to save the instance.</param>
        public static void SaveScriptable<T> (T instance, string path)
        where T : ScriptableObject
        {
            //  Creating or finding directories.
            char[] separator = {'/'};
            string[] pathSections = path.Split (separator);
            string directory = string.Empty;
            for (int i = 0; i < pathSections.Length - 1; i++)
            {
                directory = string.Concat (directory, pathSections[i]);
                if (!Directory.Exists (directory))
                    Directory.CreateDirectory (directory);
                directory = string.Concat (directory, "/");
            }

            //  Creation of scriptable asset.
            try
            {
                AssetDatabase.CreateAsset (instance, path);
                AssetDatabase.SaveAssets ();
            }
            finally
            {
                AssetDatabase.Refresh ();
            }
        }
    }
}
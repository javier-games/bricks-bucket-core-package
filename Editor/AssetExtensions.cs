using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Monogum.BricksBucket.Core.Editor
{
    /// <!-- AssetExtensions -->
    ///
    /// <summary>
    /// Extensions for assets.
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class AssetExtensions
    {
        /// <summary>
        /// Returns the path of the asset in project.
        /// </summary>
        /// <param name="asset">Object to get its path from.</param>
        /// <returns>Path in project if the asset</returns>
        public static string GetAssetPath (this Object asset)
        {
            var path = AssetDatabase.GetAssetPath (asset);
            if (!string.IsNullOrEmpty (path)) return path;

#if UNITY_2018_3_OR_NEWER
            //  Determinate whether the asset is a prefab.
            if (PrefabUtility.IsPartOfPrefabAsset (asset))
                path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot (
                    instanceComponentOrGameObject: asset
                );
#else
            //  Determinate whether the asset is a prefab.
            var prefabRoot = PrefabUtility.GetPrefabObject(asset);
            if (prefabRoot != null)
                path = AssetDatabase.GetAssetPath(prefabRoot);
#endif

            if (!string.IsNullOrEmpty (path)) return path;

            //  Determinate if the game object is on scene.
            GameObject gameObject = null;
            switch (asset)
            {
                case GameObject gameObj:
                    gameObject = gameObj;
                    break;
                case Component component:
                    gameObject = component.gameObject;
                    break;
            }

            return gameObject != null ? gameObject.scene.path : null;
        }

        /// <summary>
        /// Adds a sub asset to this object.
        /// </summary>
        /// <param name="asset">Asset origin.</param>
        /// <param name="subAsset">Sub asset to add.</param>
        public static void AddSubAsset (this Object asset, Object subAsset)
        {
            var assetPath = asset.GetAssetPath ();
            Debug.Assert (assetPath != null);
            AssetDatabase.AddObjectToAsset (subAsset, assetPath);
            AssetDatabase.ImportAsset (assetPath);
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh ();
        }

        /// <summary>
        /// Try to destroy this immediately.
        /// </summary>
        /// <param name="asset">Asset to destroy.</param>
        public static void TryDestroyImmediate (this Object asset)
        {
            try
            {
                if (asset != null) Object.DestroyImmediate (asset, true);
            }
            catch (System.Exception e)
            {
                // TODO: Add Log Method to AssetExtensions.TryDestroyImmediate.
                Debug.LogException (e, asset);
            }
        }

        /// <summary>
        /// Destroy unreferenced sub assets.
        /// </summary>
        /// <param name="asset">Asset origin to destroy its sub assets.</param>
        /// <param name="candidates">List of all sub assets.</param>
        public static void DestroyUnreferencedSubAssets (
            this Object asset, IEnumerable<Object> candidates
        )
        {
            var path = AssetDatabase.GetAssetPath (asset);

            if (string.IsNullOrEmpty (path)) return;

            var mainAsset = AssetDatabase.LoadMainAssetAtPath (path);
            var allAssets = AssetDatabase.LoadAllAssetsAtPath (path);
            var referencedAssets = new HashSet<Object> ();

            //  Add Referenced sub assets
            BucketEditor.AddReferencedSubAssets (referencedAssets, allAssets,
                mainAsset);

            //  Destroy unreferenced assets.
            var unreferencedSubAssets = candidates.Except (referencedAssets);
            foreach (var unreferencedAsset in unreferencedSubAssets)
                unreferencedAsset.TryDestroyImmediate ();
            AssetDatabase.ImportAsset (path);
        }
    }
}
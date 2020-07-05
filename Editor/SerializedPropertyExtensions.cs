using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

using Object = UnityEngine.Object;

namespace BricksBucket.Core.Editor
{
    public static class SerializedPropertyExtensions
    {
        /// <summary> Enumerates Child Properties. </summary>
        /// <param name="serializedObject"></param>
        /// <returns> Simple iteration over a collection. </returns>
        public static IEnumerable<SerializedProperty>
            EnumerateChildProperties (this SerializedObject serializedObject)
        {
            var iterator = serializedObject.GetIterator ();
            if (!iterator.NextVisible (enterChildren: true)) yield break;
            while (iterator.NextVisible (enterChildren: false))
                yield return iterator;
        }

        /// <summary> Enumerates Child Properties. </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<SerializedProperty>
            EnumerateChildProperties (this SerializedProperty property)
        {
            var iterator = property.Copy ();
            var end = iterator.GetEndProperty ();
            if (!iterator.NextVisible (enterChildren: true)) yield break;
            do
            {
                if (SerializedProperty.EqualContents (iterator, end))
                    yield break;

                yield return iterator;
            } while (iterator.NextVisible (enterChildren: false));
        }

        /// <summary> Find Parent Property. </summary>
        /// <param name="property"></param>
        /// <returns> Serialized Property. </returns>
        public static SerializedProperty
            FindParentProperty (this SerializedProperty property)
        {
            var serializedObject = property.serializedObject;
            var propertyPath = property.propertyPath;
            var propertyKeys = BucketEditor.ParsePropertyPath (propertyPath).
                ToArray ();
            var propertyKeyCount = propertyKeys.Length;

            // Parent is serialized object.
            if (propertyKeyCount == 1) return null;

            var lastPropertyKey = propertyKeys[propertyKeyCount - 1];
            // Parent is an array, drop [Array,data,N] from path.
            if (lastPropertyKey is int)
            {
                var parentKeys = propertyKeys.Take (propertyKeyCount - 3);
                var parentPath = BucketEditor.JoinPropertyPath (parentKeys);
                return serializedObject.FindProperty (parentPath);
            }
            else
                // Parent is a structure, drop [name] from path.
            {
                var parentKeys = propertyKeys.Take (propertyKeyCount - 1);
                var parentPath = BucketEditor.JoinPropertyPath (parentKeys);
                return serializedObject.FindProperty (parentPath);
            }
        }

        /// <summary> Gets the object of a serialized property. </summary>
        /// <param name="property"></param>
        /// <returns> The object. </returns>
        public static object GetObject (this SerializedProperty property)
        {
            var obj = property.serializedObject.targetObject;
            var path = BucketEditor.ParseValuePath (property);
            return BucketEditor.FindObject (obj, path);
        }

        /// <summary> Returns the type of the object. </summary>
        /// <param name="property"> Property to look for. </param>
        /// <returns> Type of object. </returns>
        public static Type GetObjectType (this SerializedProperty property)
        {
            var obj = property.GetObject ();
            return obj?.GetType ();
        }


        /// <summary> Sets the object of a serialized property. </summary>
        /// <param name="property"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns> The object. </returns>
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static bool SetObjectValue (
            this SerializedProperty property,
            Type type,
            object value
        )
        {
            var obj = property.serializedObject.targetObject;
            var path = BucketEditor.ParseValuePath (property);
            return BucketEditor.SetObjectValue (type, obj, path, value);
        }


        /// <summary> Compares the type of a property. </summary>
        /// <param name="property"> Main Property. </param>
        /// <param name="typeToCompare"> Type to compare. </param>
        /// <returns> Whether the property is the type to compare. </returns>
        public static bool
            CompareType (this SerializedProperty property, Type typeToCompare)
        {
            var propertyType = property.GetObjectType ();
            return propertyType == typeToCompare;
        }

        /// <summary> Returns whether this is array or list. </summary>
        /// <param name="property"></param>
        /// <returns> Whether this is array or list or not. </returns>
        public static bool IsArrayOrList (this SerializedProperty property)
        {
            return (
                property.propertyType == SerializedPropertyType.Generic &&
                property.isArray
            );
        }

        /// <summary> Returns whether this is an structure. </summary>
        /// <param name="property"></param>
        /// <returns> Whether this is an structure or not. </returns>
        public static bool IsStructure (this SerializedProperty property)
        {
            return (
                property.propertyType == SerializedPropertyType.Generic &&
                property.isArray == false &&
                property.hasChildren
            );
        }

        /// <summary> Finds References SubAssets. </summary>
        /// <param name="property"></param>
        /// <returns> HashSet of sub assets. </returns>
        public static HashSet<Object>
            FindReferencedSubAssets (this SerializedProperty property)
        {
            var propertyAsset = property.serializedObject.targetObject;
            var assetPath = AssetDatabase.GetAssetPath (propertyAsset);
            var allSubAssets = AssetDatabase.LoadAllAssetsAtPath (assetPath);
            var referencedSubAssets = new HashSet<Object> ();
            BucketEditor.AddReferencedSubAssets (
                referencedAssets: referencedSubAssets,
                allAssets: allSubAssets,
                property: property
            );
            return referencedSubAssets;
        }

        /// <summary> Destroys Unreferenced Sub Assets. </summary>
        /// <param name="serializedObject"></param>
        /// <param name="candidates"></param>
        public static void DestroyUnreferencedSubAssets
        (
            this SerializedObject serializedObject,
            IEnumerable<Object> candidates
        )
        {
            var targetObject = serializedObject.targetObject;
            targetObject.DestroyUnreferencedSubAssets (candidates);
        }

        /// <summary> Returns whether has referenced sub assets. </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool
            HasReferenceSubAssets (this SerializedProperty property) =>
            FindReferencedSubAssets (property).Any ();

        /// <summary> Resizes the array. </summary>
        /// <param name="property"></param>
        /// <param name="arraySize"></param>
        public static void
            ResizeArray (this SerializedProperty property, int arraySize)
        {
            while (property.arraySize < arraySize)
                property.InsertArrayElementAtIndex (property.arraySize);
            while (property.arraySize > arraySize)
                property.DeleteArrayElementAtIndex (property.arraySize - 1);
        }

        /// <summary> Replaces the property array for new values. </summary>
        /// <param name="property"> Property with elements to replace. </param>
        /// <param name="newElements"> New elements. </param>
        public static void
            ReplaceArray (
                this SerializedProperty property, Object[] newElements
            )
        {
            property.arraySize = 0;
            property.serializedObject.ApplyModifiedProperties ();
            property.arraySize = newElements.Length;
            for (var i = 0; i < newElements.Length; i++)
            {
                var subProperty = property.GetArrayElementAtIndex (i);
                subProperty.objectReferenceValue = newElements[i];
            }

            property.serializedObject.ApplyModifiedProperties ();
        }

        /// <summary> Returns the value path. </summary>
        /// <param name="property"> Path value. </param>
        public static string GetValuePath (this SerializedProperty property) =>
            property.propertyPath.Replace (".Array.data[", "[");

        /// <summary> Returns string value. </summary>
        /// <param name="property"> Property to parse. </param>
        /// <returns> String Value. </returns>
        public static string ToStringValue (this SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return property.stringValue;

                case SerializedPropertyType.Character:
                case SerializedPropertyType.Integer:
                    return property.type == "char"
                        ? Convert.ToChar (property.intValue).ToString ()
                        : property.intValue.ToString ();

                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue != null
                        ? property.objectReferenceValue.ToString ()
                        : "null";

                case SerializedPropertyType.Boolean:
                    return property.boolValue.ToString ();

                case SerializedPropertyType.Enum:
                    return property.enumNames[property.enumValueIndex];

                default:
                    return string.Empty;
            }
        }
    }
}
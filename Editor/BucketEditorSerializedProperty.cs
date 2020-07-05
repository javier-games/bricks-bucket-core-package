using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace BricksBucket.Core.Editor
{
    /* SERIALIZED PROPERTY BUCKET */
    public static partial class BucketEditor
    { 
        /// <summary> Parse the property path. </summary>
        /// <param name="propertyPath"></param>
        internal static IEnumerable<object>
        ParsePropertyPath (string propertyPath) =>
            ParseValuePath (propertyPath);

        /// <summary> Parse the property path. </summary>
        /// <param name="property"></param>
        internal static IEnumerable<object>
        ParsePropertyPath (SerializedProperty property) =>
            ParsePropertyPath (property.propertyPath);

        /// <summary> Parse the value path. </summary>
        /// <param name="property"></param>
        internal static IEnumerable<object>
        ParseValuePath (SerializedProperty property) =>
            ParseValuePath (property.GetValuePath());


        /// <summary> Joins Value Path. </summary>
        /// <param name="keys"></param>
        internal static string JoinValuePath (IEnumerable<object> keys)
        {
            var builder = new StringBuilder ();
            foreach (var key in keys)
            {
                switch (key)
                {
                    case string s:
                    {
                        if (builder.Length > 0)
                            builder.Append ('.');
                        builder.Append (s);
                        continue;
                    }
                    case int i:
                        builder.Append ('[');
                        builder.Append (i);
                        builder.Append (']');
                        continue;
                    default:
                        throw new Exception ($"Invalid key: {key}");
                }
            }
            return builder.ToString ();
        }

        /// <summary> Joins Property Path. </summary>
        /// <param name="keys"></param>
        internal static string JoinPropertyPath (IEnumerable<object> keys)
        {
            return JoinValuePath (keys);
        }
        
        /// <summary>
        /// Parse value path.
        /// </summary>
        /// <param name="path">Path to parse.</param>
        private static IEnumerable<object> ParseValuePath (string path)
        {
            var keys = path.Split ('.');
            foreach (var key in keys)
            {
                //  For element identifier.
                if (key.IsElementIdentifier ())
                {
                    var subKeys = key.Split ('[', ']');
                    yield return subKeys[0];
                    foreach (var subKey in subKeys.Skip (1))
                    {
                        if (string.IsNullOrEmpty (subKey))
                            continue;

                        int index = int.Parse (subKey);
                        yield return index;
                    }

                    //  Continue the key iteration.
                    continue;
                }

                //  For element index.
                if (key.IsElementIndex ())
                {
                    var subKeys = key.Split ('[', ']');
                    foreach (var subKey in subKeys)
                    {
                        if (string.IsNullOrEmpty (subKey))
                            continue;

                        int index = int.Parse (subKey);
                        yield return index;
                    }

                    //  Continue the key iteration.
                    continue;
                }

                //  For member identifier.
                if (!key.IsMemberIdentifier ())
                    throw new Exception ($"Invalid path: {path}");
                yield return key;
            }
        }
        
        
        #region Exceptions

        /// <summary> Unsupported Value Type. </summary>
        /// <param name="property"></param>
        /// <returns> Exception. </returns>
        public static Exception
        UnsupportedValueType (SerializedProperty property)
        {
            var serializedObject = property.serializedObject;
            var targetObject = serializedObject.targetObject;
            var targetType = targetObject.GetType ();
            var targetTypeName = targetType.Name;
            var valueTypeName = property.propertyType.ToString ();
            var propertyPath = property.propertyPath;
            return new Exception (
                $"Unsupported value type {valueTypeName} for " + 
                $"{targetTypeName}.{propertyPath}");
        }

        /// <summary> Unsupported Value Type. </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns> Exception. </returns>
        public static Exception
        UnsupportedValue (SerializedProperty property, object value)
        {
            var serializedObject = property.serializedObject;
            var targetObject = serializedObject.targetObject;
            var targetType = targetObject.GetType ();
            var targetTypeName = targetType.Name;
            var propertyPath = property.propertyPath;
            return new Exception (
                $"Unsupported value {value} for " +
                $"{targetTypeName}.{propertyPath}"
            );
        }

        /// <summary> Unsupported Value Type. </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns> Exception. </returns>
        public static Exception
        UnsupportedValue
        (SerializedProperty property, object value, string expected)
        {
            var serializedObject = property.serializedObject;
            var targetObject = serializedObject.targetObject;
            var targetType = targetObject.GetType ();
            var targetTypeName = targetType.Name;
            var propertyPath = property.propertyPath;

            value = value == null ? "null" : $"'{value}'";
            return new Exception (
                $"Unsupported value {value} for " + 
                $"{targetTypeName}.{propertyPath}, expected {expected}");
        }

        /// <summary> Exception when a field is not found. </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns> Exception. </returns>
        public static Exception
        FieldNotFoundException (Type type, string field) =>
            new KeyNotFoundException ($"Property {type}.{field} not found");

        #endregion

        
        #region Regex
        
        /// <summary>
        /// Whether this is an element identifier.
        /// </summary>
        /// <param name="text">String to validate.</param>
        /// <returns>Whether this is an element identifier or not.</returns>
        public static bool IsElementIdentifier (this string text) =>
            Regex.IsMatch (text, @"^[_a-zA-Z][_a-zA-Z0-9]*(\[[0-9]*\])+$");

        /// <summary>
        /// Whether this is an element index.
        /// </summary>
        /// <param name="text">String to validate.</param>
        /// <returns>Whether this is an element index or not.</returns>
        public static bool IsElementIndex (this string text) =>
            Regex.IsMatch (text, @"^[_a-zA-Z][_a-zA-Z0-9]*(\[[0-9]*\])+$");

        /// <summary>
        /// Whether this is a member identifier.
        /// </summary>
        /// <param name="text">String to validate.</param>
        /// <returns>Whether this is a member identifier or not.</returns>
        public static bool IsMemberIdentifier (this string text) =>
            Regex.IsMatch (text, @"^[_a-zA-Z][_a-zA-Z0-9]*$");

        #endregion
    }
}
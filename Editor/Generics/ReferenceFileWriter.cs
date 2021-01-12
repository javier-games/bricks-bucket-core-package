using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Generics.Editor
{
    /// <!-- HardwiredFileWriter -->
    /// <summary>
    /// Has methods to write the dynamic environment.
    /// </summary>
    public static class ReferenceFileWriter
    {
        #region Fields

        /// <summary>
        /// Original Bricks Bucket Namespace.
        /// </summary>
        private const string BricksBucketNameSpace =
            "BricksBucket.Core.Generic";
        
        /// <summary>
        /// Name of the reference script.
        /// </summary>
        private const string ReferenceClassName = "DynReference";
        
        /// <summary>
        /// Name of the hardwired script.
        /// </summary>
        private const string HardwiredClassName = "ComponentRegistry";
        
        /// <summary>
        /// Extensions of scripts.
        /// </summary>
        private const string Extension = ".cs";
        
        /// <summary>
        /// Writer for AbstractComponentRegistry.cs
        /// </summary>
        private static StreamWriter _writer;
        
        #endregion

        #region Methods

        /// <summary>
        /// Creates the dynamic environment classes.
        /// </summary>
        /// <param name="localPath">Local path where to put the scripts</param>
        /// <param name="nameSpace">Namespace of the environment.</param>
        public static void CreateClasses(string localPath,  string nameSpace)
        {
            // Runtime
            var hardwiredPath =
                 "/" + localPath + "/" +
                HardwiredClassName +
                Extension;
            
            if (!File.Exists(hardwiredPath))
            {
                ConstructPath(hardwiredPath);
            }
            
            _writer?.Close();
            _writer = null;
            _writer = new StreamWriter(
                Application.dataPath + hardwiredPath,
                false
            );

            var content = Template
                .Replace(
                    "{OLD_NAMESPACE}", BricksBucketNameSpace)
                .Replace("{NEW_NAMESPACE}", nameSpace)
                .Replace(
                    "{REGISTRY_CLASS_NAME}", HardwiredClassName)
                .Replace(
                    "{REFERENCE_CLASS_NAME}", ReferenceClassName)
                .Replace("{PATH}", localPath)
                .Replace("{DATE}", $"{DateTime.Now:F}")
                .Replace("{TYPES}", string.Empty)
                .Replace("{ACTIONS}", string.Empty)
                .Replace("{FUNCTIONS}", string.Empty);

            _writer.Write(content);
            _writer.Close();
            
            // Editor.
            var editorPath = 
                "/" + localPath + "/Editor/" +
                ReferenceClassName + "Drawer" +
                Extension;

            if (!File.Exists(editorPath))
            {
                ConstructPath(editorPath);
            }

            _writer?.Close();
            _writer = null;
            _writer = new StreamWriter(
                Application.dataPath + editorPath,
                false
            );

            content = EditorTemplate
                .Replace(
                    "{OLD_NAMESPACE}", BricksBucketNameSpace)
                .Replace("{NEW_NAMESPACE}", nameSpace)
                .Replace(
                    "{REFERENCE_CLASS_NAME}", ReferenceClassName);

            _writer.Write(content);
            _writer.Close();
            
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Constructs the local directories of a path file.
        /// </summary>
        /// <param name="localPath"></param>
        private static void ConstructPath(string localPath)
        {
            var directories = ("Assets/" + localPath).Split('/');
            var limit = directories.Length > 0
                ? (directories[directories.Length - 1].Contains(".")
                    ? directories.Length - 1
                    : directories.Length)
                : 0;

            var path = string.Empty;
            for (var i = 0; i < limit; i++)
            {
                path += directories[i];

                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }
                
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += "/";
            }
            AssetDatabase.Refresh();
        }

        #endregion

        #region Private Consts

        /// <summary>
        /// Script template for editor drawer of reference. 
        /// </summary>
        private const string EditorTemplate = @"
using UnityEditor;

namespace {NEW_NAMESPACE}
{
    [CustomPropertyDrawer(typeof({REFERENCE_CLASS_NAME}))]
    public class {REFERENCE_CLASS_NAME}Drawer :
        {OLD_NAMESPACE}.Editor.AbstractReferenceDrawer <{REFERENCE_CLASS_NAME}>{ }
}
";

        /// <summary>
        /// Template of the hardwired script.
        /// </summary>
        private const string Template = @"
using System;
using System.Collections.Generic;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantNameQualifier

namespace {NEW_NAMESPACE}
{
	/// <summary>
	/// Registered types.
	/// 
	/// Since iOS cannot support System.Reflection, AbstractReference has to
	/// have this static class to cast values.
	/// 
	/// 
	/// <autogenerated>
	/// 
	/// This code was generated by a tool.
	/// Changes to this file may cause incorrect behavior and will
	/// be lost if the code is regenerated.
	/// 
	/// </autogenerated>
	/// 
	/// ----------------------------------------------------------
	/// Code generated on {DATE}
	/// ----------------------------------------------------------
	///
	/// By Javier García.
	/// </summary>
    public sealed class {REGISTRY_CLASS_NAME} : {OLD_NAMESPACE}.AbstractComponentRegistry
	{
		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.NameSpace""/>
		public override string NameSpace => ""{NEW_NAMESPACE}"";

        /// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.Path""/>
        public override string Path => ""{PATH}"";

		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.TypesList""/>
        protected override List<Type> TypesList { get; } = new List<Type>
		{
            {TYPES}
        };

		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.Set""/>
        protected override
			Dictionary<string, Dictionary<string, Action<object, object>>> Set
		{
			get;
		} = new Dictionary<string, Dictionary<string, Action<object, object>>>
        {
            {ACTIONS}
		};

		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.Get""/>
        protected override
			Dictionary<string, Dictionary<string, Func<object, object>>> Get
		{
			get;
		} = new Dictionary<string, Dictionary<string, Func<object, object>>>
		{
            {FUNCTIONS}
        };
	}

	[System.Serializable]
	public sealed class {REFERENCE_CLASS_NAME} :
		{OLD_NAMESPACE}.AbstractReference<{REGISTRY_CLASS_NAME}>
	{
		private static {REGISTRY_CLASS_NAME} _hardwired =
            new {REGISTRY_CLASS_NAME}();
        
		public override {OLD_NAMESPACE}.IComponentRegistry ComponentRegistry => 
			_hardwired ?? (_hardwired = new {REGISTRY_CLASS_NAME}());
	}
}";
        
        /// <summary>
        /// Template for an element in a list of types.
        /// </summary>
        private const string TypeElement = @"
            typeof({0})";
        
        /// <summary>
        /// Template for a region on a actions dictionary.
        /// </summary>
        private const string ActionRegionTemplate = @"                
            {
				#region {OBJECT_NAME}

                ""{OBJECT_NAME}"",
                new Dictionary<string, Action<object, object>>
                {
                    {ACTIONS}
                }

                #endregion
            }";
        
        /// <summary>
        /// Template for an action on a dictionary.
        /// </summary>
        private const string ActionTemplate = @"
                    {
                        ""{PROPERTY_NAME}"",
                        (component, value) =>
                            (component as {OBJECT_NAME}).{PROPERTY_NAME} =
                                ({PROPERTY_TYPE}) value
                    }";
        
        /// <summary>
        /// Template for a region on a functions dictionary.
        /// </summary>
        private const string FunctionRegionTemplate = @"                
            {
				#region {OBJECT_NAME}

                ""{OBJECT_NAME}"",
                new Dictionary<string, Func<object, object>>
                {
                    {FUNCTIONS}
                }

                #endregion
            }";
        
        /// <summary>
        /// Template for a function on a dictionary.
        /// </summary>
        private const string FunctionTemplate = @"
                    {
                        ""{PROPERTY_NAME}"",
                        (component) =>
                            (component as {OBJECT_NAME}).{PROPERTY_NAME}
                    }";

        #endregion
        
        
    }
}
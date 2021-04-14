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
    public static class HardwiredFileWriter
    {
        #region Fields

        /// <summary>
        /// Original Bricks Bucket Namespace.
        /// </summary>
        private const string BricksBucketNameSpace =
            "Monogum.BricksBucket.Core.Generics";

        /// <summary>
        /// Writer for AbstractComponentRegistry.cs
        /// </summary>
        private static StreamWriter _writer;
        
        #endregion

        #region Methods

        /// <summary>
        /// Inits the file.
        /// </summary>
        /// <param name="hardwired"></param>
        public static void ResetFile(IAutogeneratedScript hardwired)
        {
            ResetFile(
                hardwired.Path,
                hardwired.Extension,
                hardwired.NameSpace,
                hardwired.ClassName
            );
        }

        /// <summary>
        /// Resets the File with the given info.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <param name="extension">Extension of the file</param>
        /// <param name="nameSpace">Namespace of the file.</param>
        /// <param name="className">Name of the class.</param>
        public static void ResetFile(
            string path, string extension, string nameSpace, string className)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }
            
            var data = new ScriptData()
            {
                Path = path,
                Extension = extension,
                NameSpace = nameSpace,
                ClassName = className
            };
            
            WriteFile(data, null);
        }

        /// <summary>
        /// Re writes the content of the file.
        /// </summary>
        /// <param name="componentRegistry"></param>
        public static void ReWriteFile(AbstractComponentRegistry componentRegistry)
        {
            if (string.IsNullOrWhiteSpace(componentRegistry.Path))
            {
                return;
            }
            
            var data = new ScriptData()
            {
                Path = componentRegistry.Path,
                Extension = componentRegistry.Extension,
                NameSpace = componentRegistry.NameSpace,
                ClassName = componentRegistry.ClassName
            };
            
            //  Saving current types list.
            var registeredTypes = new List<Type>();
            foreach (var oldType in componentRegistry.ComponentTypes)
            {
                registeredTypes.Add(oldType);
            }
            
            WriteFile(data, registeredTypes);
        }

        /// <summary>
        /// Register a new type.
        /// </summary>
        /// <param name="type">Type to add.</param>
        /// <param name="scriptData">ComponentRegistry class where to write.</param>
        /// <param name="registry"></param>
        public static void RegisterType(
            Type type,
            IAutogeneratedScript scriptData,
            IComponentRegistry registry
        )
        {
            
            if (string.IsNullOrWhiteSpace(scriptData.Path))
            {
                return;
            }
            
            var data = new ScriptData()
            {
                Path = scriptData.Path,
                Extension = scriptData.Extension,
                NameSpace = scriptData.NameSpace,
                ClassName = scriptData.ClassName
            };

            //  Saving current types list.
            var registeredTypes = new List<Type>();
            foreach (var oldType in registry.ComponentTypes)
            {
                registeredTypes.Add(oldType);
            }

            if (type != null && !registeredTypes.Contains(type))
                registeredTypes.Add(type);

            WriteFile(data, registeredTypes);
        }

        /// <summary>
        /// Writes a new file with a template for an abstract componentRegistry.
        /// </summary>
        /// <param name="hardwired"></param>
        /// <param name="registeredTypes"></param>
        private static void WriteFile(
            IAutogeneratedScript hardwired, List<Type> registeredTypes
        )
        {
            registeredTypes?.Sort((x, y) => string.Compare(
                x.FullName,
                y.FullName,
                StringComparison.InvariantCulture
            ));

            //  Initializing a new writer.
            var localPath = 
                hardwired.Path + "/" +
                hardwired.ClassName + "." +
                hardwired.Extension;
            var path = Application.dataPath + "/" + localPath;
            
            if (!File.Exists(path))
            {
                ConstructPath(localPath);
            }
            
            _writer?.Close();
            _writer = null;
            _writer = new StreamWriter(
                path,
                false
            );

            var content = Template
                .Replace("{OLD_NAMESPACE}", BricksBucketNameSpace)
                .Replace("{NEW_NAMESPACE}", hardwired.NameSpace)
                .Replace("{CLASS_NAME}", hardwired.ClassName)
                .Replace("{PATH}", hardwired.Path)
                .Replace("{EXTENSION}", hardwired.Extension)
                .Replace("{DATE}", $"{DateTime.Now:F}")
                .Replace("{TYPES}", GetTypes(registeredTypes))
                .Replace("{ACTIONS}", GetActionsContent(registeredTypes))
                .Replace("{FUNCTIONS}", GetFunctionsContent(registeredTypes))
                .Replace("{PROPERTY_TYPES}",
                    GetTypesPropertiesContent(registeredTypes));

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

        /// <summary>
        /// Gets the string of types on a string.
        /// </summary>
        /// <param name="registeredTypes">List of current registered types.
        /// </param>
        /// <returns>Empty if there is any type.</returns>
        private static string GetTypes(IReadOnlyList<Type> registeredTypes)
        {
            var typesList = string.Empty;
            if (registeredTypes == null || registeredTypes.Count == 0)
            {
                return typesList;
            }
            
            for (var i = 0; i < registeredTypes.Count; i++)
            {
                typesList += string.Format(TypeElement,
                    registeredTypes[i]);
                if (i < registeredTypes.Count - 1)
                    typesList += ",\n";
            }
            return typesList;
        }

        /// <summary>
        /// Whether the given property is valid.
        /// </summary>
        /// <param name="propertyInfo">Property to check.</param>
        /// <returns><value>False</value>> if the property does not
        /// complains with the requirements.</returns>
        private static bool IsValidProperty(PropertyInfo propertyInfo) =>
            propertyInfo.CanRead
            && propertyInfo.CanWrite
            && !propertyInfo.IsDefined(typeof(ObsoleteAttribute), true)
            && propertyInfo.Name != "runInEditMode";

        /// <summary>
        /// Gets the dictionaries of Actions in a string.
        /// </summary>
        /// <param name="registeredTypes">List of current registered types.
        /// </param>
        /// <returns>Empty if there is any type.</returns>
        private static string GetActionsContent(
            IReadOnlyList<Type> registeredTypes)
        {
            var setDictionary = string.Empty;
            if (registeredTypes == null || registeredTypes.Count == 0)
            {
                return setDictionary;
            }


            for (var i = 0; i < registeredTypes.Count; i++)
            {

                var subContent = string.Empty;
                var propertiesInfo = registeredTypes[i].GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                );
                var propertiesToAdd = 0;
                foreach (var propertyInfo in propertiesInfo)
                    if (IsValidProperty(propertyInfo))
                        propertiesToAdd++;
                var propertiesAdded = 0;

                //  Writing Properties.
                for (var j = 0; j < propertiesInfo.Length; j++)
                {
                    if (!IsValidProperty(propertiesInfo[j]))
                        continue;
                    subContent += ActionTemplate
                        .Replace("{PROPERTY_NAME}", propertiesInfo[j].Name)
                        .Replace(
                            "{OBJECT_NAME}",
                            registeredTypes[i].FullName?.Replace("+","."))
                        .Replace(
                            "{PROPERTY_TYPE}",
                            propertiesInfo[j].PropertyType.FullName?
                                .Replace("+",".")
                    );
                    
                    propertiesAdded++;
                    if (propertiesAdded < propertiesToAdd)
                        subContent += ",";
                }

                //  Writing Region Component.
                setDictionary += ActionRegionTemplate
                    .Replace("{OBJECT_NAME}", registeredTypes[i].FullName)
                    .Replace("{ACTIONS}", subContent);
                
                if (i < registeredTypes.Count - 1)
                    setDictionary += ",\n";
            }
            return setDictionary;
        }

        /// <summary>
        /// Gets the dictionaries of Actions in a string.
        /// </summary>
        /// <param name="registeredTypes">List of current registered types.
        /// </param>
        /// <returns>Empty if there is any type.</returns>
        private static string GetFunctionsContent(
            IReadOnlyList<Type> registeredTypes)
        {
            var getDictionary = string.Empty;
            if (registeredTypes == null || registeredTypes.Count == 0)
            {
                return getDictionary;
            }

            for (var i = 0; i < registeredTypes.Count; i++)
            {

                var subContent = string.Empty;
                var propertiesInfo = registeredTypes[i].GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                );
                var propertiesToAdd = 0;
                foreach (var propertyInfo in propertiesInfo)
                    if (IsValidProperty(propertyInfo))
                        propertiesToAdd++;
                var propertiesAdded = 0;

                //  Writing Properties.
                for (var j = 0; j < propertiesInfo.Length; j++)
                {
                    if (!IsValidProperty(propertiesInfo[j]))
                        continue;
                    
                    subContent += FunctionTemplate
                        .Replace("{PROPERTY_NAME}", propertiesInfo[j].Name)
                        .Replace("{OBJECT_NAME}", registeredTypes[i].FullName);
                    
                    propertiesAdded++;
                    if (propertiesAdded < propertiesToAdd)
                        subContent += ",";
                }

                //  Writing Region Component.
                getDictionary += FunctionRegionTemplate
                    .Replace("{OBJECT_NAME}", registeredTypes[i].FullName)
                    .Replace("{FUNCTIONS}", subContent);
                    
                if (i < registeredTypes.Count - 1)
                    getDictionary += ",\n";
            }
            return getDictionary;
        }

        /// <summary>
        /// Gets the dictionaries of Actions in a string.
        /// </summary>
        /// <param name="registeredTypes">List of current registered types.
        /// </param>
        /// <returns>Empty if there is any type.</returns>
        private static string GetTypesPropertiesContent(
            IReadOnlyList<Type> registeredTypes)
        {
            var getDictionary = string.Empty;
            if (registeredTypes == null || registeredTypes.Count == 0)
            {
                return getDictionary;
            }

            for (var i = 0; i < registeredTypes.Count; i++)
            {

                var subContent = string.Empty;
                var propertiesInfo = registeredTypes[i].GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                );
                var propertiesToAdd = 0;
                foreach (var propertyInfo in propertiesInfo)
                    if (IsValidProperty(propertyInfo))
                        propertiesToAdd++;
                var propertiesAdded = 0;

                //  Writing Properties.
                for (var j = 0; j < propertiesInfo.Length; j++)
                {
                    if (!IsValidProperty(propertiesInfo[j]))
                        continue;

                    subContent += TypesTemplate
                        .Replace(
                            "{PROPERTY_TYPE}", 
                            propertiesInfo[j].PropertyType.FullName
                                .Replace('+', '.'))
                        .Replace("{PROPERTY_NAME}",
                            propertiesInfo[j].Name);

                    propertiesAdded++;
                    if (propertiesAdded < propertiesToAdd)
                        subContent += ",";
                }

                //  Writing Region Component.
                getDictionary += TypesRegionTemplate
                    .Replace("{OBJECT_NAME}", registeredTypes[i].FullName)
                    .Replace("{PROPERTY_TYPES}", subContent);
                    
                if (i < registeredTypes.Count - 1)
                    getDictionary += ",\n";
            }
            return getDictionary;
        }
        
        #endregion
        
        
        #region Nested Classes
        
        /// <summary>
        /// Class where to put the data for auto generated componentRegistry files.
        /// </summary>
        public class ScriptData : IAutogeneratedScript
        {
            public string NameSpace { get; set; }
            public string Path { get; set; }
            public string ClassName { get; set; }
            public string Extension { get; set; }
        }

        #endregion

        
        #region Private Consts

        /// <summary>
        /// Template of the componentRegistry script.
        /// </summary>
        private const string Template =
@"using System;
using System.Collections.Generic;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantNameQualifier
// ReSharper disable StringLiteralTypo

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
    public sealed class {CLASS_NAME} : {OLD_NAMESPACE}.AbstractComponentRegistry
	{
		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.NameSpace""/>
		public override string NameSpace => ""{NEW_NAMESPACE}"";

        /// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.Path""/>
        public override string Path => ""{PATH}"";

		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.ClassName""/>
		public override string ClassName => ""{CLASS_NAME}"";

        /// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.Extension""/>
        public override string Extension => ""{EXTENSION}"";

		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.ComponentTypesList""/>
        protected override List<Type> ComponentTypesList { get; } = 
            new List<Type>
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

		/// <inheritdoc cref=""{OLD_NAMESPACE}.AbstractComponentRegistry.PropertyType""/>
        protected override
			Dictionary<string, Dictionary<string, Type>> PropertyType
		{
			get;
		} = new Dictionary<string, Dictionary<string, Type>>
		{
            {PROPERTY_TYPES}
        };

        #region Methods
#if UNITY_EDITOR

        /// <summary>
        /// Rebuilds the componentRegistry file.
        /// </summary>
        [UnityEditor.MenuItem(""Tools/BricksBucket/ComponentRegistry/{NEW_NAMESPACE} {CLASS_NAME}/Rebuild"")]
        public static void Rebuild()
        {
            {OLD_NAMESPACE}.Editor.HardwiredFileWriter.ReWriteFile(new {CLASS_NAME}());
        }

        /// <summary>
        /// Resets the componentRegistry file.
        /// </summary>
        [UnityEditor.MenuItem(""Tools/BricksBucket/ComponentRegistry/{NEW_NAMESPACE} {CLASS_NAME}/Reset"")]
        public static void Reset()
        {
            {OLD_NAMESPACE}.Editor.HardwiredFileWriter.ResetFile(new {CLASS_NAME}());
        }
        
#endif
        #endregion

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
        
        /// <summary>
        /// Template for a region on a functions dictionary.
        /// </summary>
        private const string TypesRegionTemplate = @"                
            {
				#region {OBJECT_NAME}

                ""{OBJECT_NAME}"",
                new Dictionary<string, Type>
                {
                    {PROPERTY_TYPES}
                }

                #endregion
            }";
        
        /// <summary>
        /// Template for a function on a dictionary.
        /// </summary>
        private const string TypesTemplate = @"
                    {
                        ""{PROPERTY_NAME}"",
                        typeof({PROPERTY_TYPE})
                    }";

        #endregion
    }
}
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BricksBucket.Core.Editor
{
    /* REFLECTION BUCKET */
	public static partial class BucketEditor
	{
        /// <summary> Get all fields with attribute of type T. </summary>
        /// <typeparam name="T"> Type of attributes. </typeparam>
        /// <returns> Collection of all attributes. </returns>
        public static ComponentFieldInfo[]
        GetFieldsWithAttribute<T> () where T : Attribute
        {
            var components = GetAllComponentsInScenes<MonoBehaviour> ();

            var fields = new List<ComponentFieldInfo> ();

            foreach (var component in components)
            {
                if (component == null)
                    continue;

                Type type = component.GetType ();
                FieldInfo[] fieldInfos = type.GetFields (
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance
                );

                var attributes = fieldInfos.Where (
                    field => field.IsDefined (typeof (T), false)
                );

                foreach (var attribute in attributes)
                    fields.Add (new ComponentFieldInfo (attribute, component));
            }

            return fields.ToArray ();
        }

        /// <summary> Get all methods with attribute of type T. </summary>
        /// <typeparam name="T"> Type of attributes. </typeparam>
        /// <returns> Collection of all attributes. </returns>
        public static ComponentMethodInfo[]
        GetMethodsWithAttribute<T> () where T : Attribute
        {
            var components = GetAllComponentsInScenes<MonoBehaviour> ();

            var methods = new List<ComponentMethodInfo> ();

            foreach (var component in components)
            {
                if (component == null)
                    continue;

                var type = component.GetType ();
                MethodInfo[] methodInfos = type.GetMethods (
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic
                );

                var attributes = methodInfos.Where (
                    field => field.IsDefined (typeof (T), false)
                );

                foreach (var attribute in attributes)
                    methods.Add (new ComponentMethodInfo (attribute, component));
            }

            return methods.ToArray ();
        }

        /// <summary>
        /// Get all Components on loaded scenes, even all those
        /// that has been disabled.
        /// </summary>
        /// <typeparam name="T"> Type of components. </typeparam>
        /// <returns> Array of components. </returns>
        public static T[] GetAllComponentsInScenes <T>()
        {
            var components = new List<T> ();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt (i);
                var roots = scene.GetRootGameObjects ();
                foreach (var root in roots)
                {
                    var childrenComponents = root.GetComponentsInChildren<T> (
                        includeInactive: true
                    );
                    foreach (var component in childrenComponents)
                        components.Add (component);
                }
            }

            return components.ToArray ();
        }

        /// <summary> Finds the field info. </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns> Field Info. </returns>
        private static FieldInfo FindFieldInfo (this Type type, string field)
        {
            const BindingFlags bindingFlags =
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic;
            var fieldInfo = type.GetField (field, bindingFlags);
            if (fieldInfo != null)
                return fieldInfo;

            var baseType = type.BaseType;
            // ReSharper disable once TailRecursiveCall
            return baseType == null ? null : FindFieldInfo (baseType, field);
        }

        /// <summary> Finds an object in path. </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns> The object. </returns>
        public static object
        FindObject (object obj, IEnumerable<object> path)
        {
            foreach (var key in path)
            {
                switch (key)
                {
                    case string s:
                    {
                        var objType = obj.GetType ();
                        var field = s;
                        var fieldInfo = objType.FindFieldInfo (field);
                        if (fieldInfo == null)
                            throw FieldNotFoundException (objType, field);
                        obj = fieldInfo.GetValue (obj);
                        continue;
                    }
                    case int elementIndex:
                    {
                        var array = (IList) obj;
                        obj = array[elementIndex];
                        continue;
                    }
                }
            }
            return obj;
        }

        /// <summary> Sets the object value in the given path. </summary>
        /// <param name="type"> Type of the object to assign. </param>
        /// <param name="obj"> Object with field to set. </param>
        /// <param name="path"> Complete Path where to find field. </param>
        /// <param name="value"> New value. </param>
        public static bool SetObjectValue (
            Type type,
            object obj,
            IEnumerable<object> path,
            object value
        ) {
            bool success = true;
            foreach (var key in path)
            {
                if (!(key is string)) continue;
                var objType = obj.GetType ();
                var field = (string) key;
                var fieldInfo = objType.FindFieldInfo (field);
                if (fieldInfo == null)
                {
                    success = false;
                    continue;
                }
                try
                {
                    fieldInfo.SetValue (
                        obj: obj,
                        value: Convert.ChangeType (value, type));
                }
                catch { success = false; }
            }
            return success;
        }

        #region Classes

        /// <!-- ComponentFieldInfo -->
        /// 
        /// <summary>
        /// Structure to store Field Info and Component reference.
        /// </summary>
        /// 
        /// <!-- By Javier García | @jvrgms | 2020 -->
        public readonly struct ComponentFieldInfo
        {
            /// <summary> Info of the property. </summary>
            public readonly FieldInfo field;

            /// <summary> Reference to the component. </summary>
            public readonly Component component;

            /// <summary> Creates a new instance of this structure. </summary>
            /// <param name="field"> Info of the property. </param>
            /// <param name="component"> Component </param>
            public ComponentFieldInfo (FieldInfo field, Component component)
            {
                this.field = field;
                this.component = component;
            }
        }

        /// <!-- ComponentMethodInfo -->
        /// 
        /// <summary>
        /// Structure to store Method Info and Component reference.
        /// </summary>
        /// 
        /// <!-- By Javier García | @jvrgms | 2020 -->
        public readonly struct ComponentMethodInfo
        {
            /// <summary> Info of the property. </summary>
            public readonly MethodInfo method;

            /// <summary> Reference to the component. </summary>
            public readonly Component component;

            /// <summary> Creates a new instance of this structure. </summary>
            /// <param name="method"> Info of the property. </param>
            /// <param name="component"> Component </param>
            public ComponentMethodInfo (MethodInfo method, Component component)
            {
                this.method = method;
                this.component = component;
            }
        }

        #endregion
	}
}
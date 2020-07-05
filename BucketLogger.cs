namespace BricksBucket.Core
{
    /* LOGS BUCKET */
    public static partial class Bucket
    {
        // TODO: Fix Log Methods.
    }
}

/*
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BricksBucket
{

    /// <summary>
    ///
    /// Debug Utils.
    ///
    /// <para>
    /// Useful utilities for debugging.
    /// - Debug.Log Methods visible only in debug mode and unity editor.
    /// - Filter System to show only desired messages.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2019 </para>
    ///
    /// </summary>
    public static class DebugUtils
    {

        /// <summary> Log option for debug. </summary>
        public static LogOption Option { get; set; } = LogOption.None;


        #region Filter System

        /// <summary> Gets the debug filter. </summary>
        /// <value>The debug filter.</value>
        public static int Filter { get; set; } = 0;

        /// <summary> Sets completely the debug filter. </summary>
        /// <param name="levels">Filters.</param>
        public static void SetFilter (params LogLayer[] levels)
        {
            Filter = 0;
            foreach (LogLayer level in levels)
                AddLevelToFilter (level);
        }

        /// <summary> Gets the array of levels in filter. </summary>
        /// <returns>The levels.</returns>
        public static LogLayer[] GetFilteredLevels ()
        {

            int levelsCount = 0;
            for (int i = 0; i <= 6; i++)
                if (IsInFilter ((LogLayer) i))
                    levelsCount++;

            LogLayer[] levels = new LogLayer[levelsCount];
            for (int i = 0; i <= 6; i++)
                if (IsInFilter ((LogLayer) i))
                    levels[i] = (LogLayer) i;

            return levels;
        }

        /// <summary> Adds a new filter. </summary>
        /// <param name="level">Filter.</param>
        public static void AddLevelToFilter (LogLayer level)
        {
            Filter |= 1 << (int) level;
        }

        /// <summary> Removes a level from filter. </summary>
        /// <param name="level">Level.</param>
        public static void RemoveLevelFromFilter (LogLayer level)
        {
            if (IsInFilter (level))
                Filter &= ~(1 << (int) level);
        }

        /// <summary> Defines if a filter belongs to the debug filter. </summary>
        /// <returns><c>true</c> if is in debug filter.</returns>
        /// <param name="filter">Filter.</param>
        public static bool IsInFilter (LogLayer filter)
        {
            return (Filter & (1 << (int) filter)) > 0;
        }

        #endregion



        #region Private Methods

        /// <summary> Convert an object to an understandable string. </summary>
        /// <returns>The string.</returns>
        /// <param name="s">S.</param>
        private static string DebuggableString (object s)
        {
            //For debug purposes the array is checked to evidential null objects.
            return s != null ? s.ToString () : "null";
        }

        /// <summary> Convert an array to an understandable string array. </summary>
        /// <returns>The string.</returns>
        /// <param name="array">Array.</param>
        private static string[] DebuggableString (params object[] array)
        {
            string[] stringArray = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                stringArray[i] = DebuggableString (array[i]);
            return stringArray;
        }

        #endregion



        #region Log Methods

        /// <summary> Logs data to Unity Console. </summary>
        /// <param name="layer"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <param name="format"></param>
        /// <param name="data"></param>
        [System.Diagnostics.Conditional ("DEBUG")]
        public static void ExtendedLog (
            LogLayer layer,
            LogType type,
            Object context,
            string format,
            params object[] data)
        {
            if (!IsInFilter (layer))
                return;

            string message = string.IsNullOrWhiteSpace (format) ?
                string.Concat (DebuggableString (data)) :
                string.Format (format, DebuggableString ());

            Debug.LogFormat (type, Option, context, message);
        }

        /// <summary> Logs internal data to Unity Console. </summary>
        /// <param name="layer"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <param name="format"></param>
        /// <param name="data"></param>
        [System.Diagnostics.Conditional ("DEBUG")]
        internal static void InternalExtendedLog (
            LogLayer layer,
            LogType type,
            Object context,
            string format,
            params object[] data)
        {
            if (IsInFilter (LogLayer.Internal))
                ExtendedLog (layer, type, context, format, data);
        }

        /// <summary> LogType: Log. </summary>
        public static class Log
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, object[] data) =>
                ExtendedLog (layer, LogType.Log, null, null, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (params object[] data) =>
                OnLayer(LogLayer.Debug, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (params object[] data) =>
                OnLayer(LogLayer.Physics, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional("DEBUG")]
            public static void Graphics(params object[] data) =>
                OnLayer(LogLayer.Graphics, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional("DEBUG")]
            public static void Logistics(params object[] data) =>
                OnLayer(LogLayer.Logistics, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (params object[] data) =>
                OnLayer(LogLayer.Interface, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (params object[] data) =>
                OnLayer(LogLayer.Mechanics, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (params object[] data) =>
                OnLayer(LogLayer.Services, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (params object[] data) =>
                OnLayer(LogLayer.DataBase, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (params object[] data) =>
                OnLayer(LogLayer.Network, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            public static void Editor (params object[] data) =>
                OnLayer(LogLayer.Editor, data);
        }

        /// <summary> LogType: Log and Formatted. </summary>
        public static class LogFormat
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, Object context, string format, object[] data) =>
                ExtendedLog (layer, LogType.Log, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Debug, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Physics, context, format, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Graphics, context, format, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Logistics, context, format, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Interface, context, format, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Mechanics, context, format, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Services, context, format, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.DataBase, context, format, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Network, context, format, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            public static void Editor (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Editor, context, format, data);
        }

        /// <summary> LogType: Warning. </summary>
        public static class LogWarning
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, object[] data) =>
                ExtendedLog (layer, LogType.Warning, null, null, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (params object[] data) =>
                OnLayer(LogLayer.Debug, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (params object[] data) =>
                OnLayer(LogLayer.Physics, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (params object[] data) =>
                OnLayer(LogLayer.Graphics, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (params object[] data) =>
                OnLayer(LogLayer.Logistics, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (params object[] data) =>
                OnLayer(LogLayer.Interface, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (params object[] data) =>
                OnLayer(LogLayer.Mechanics, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (params object[] data) =>
                OnLayer(LogLayer.Services, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (params object[] data) =>
                OnLayer(LogLayer.DataBase, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (params object[] data) =>
                OnLayer(LogLayer.Network, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            public static void Editor (params object[] data) =>
                OnLayer(LogLayer.Editor, data);
        }

        /// <summary> LogType: Warning and Formatted. </summary>
        public static class LogWarningFormat
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, Object context, string format, object[] data) =>
                ExtendedLog (layer, LogType.Warning, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Debug, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Physics, context, format, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Graphics, context, format, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Logistics, context, format, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Interface, context, format, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Mechanics, context, format, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Services, context, format, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.DataBase, context, format, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Network, context, format, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            public static void Editor (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Editor, context, format, data);
        }

        /// <summary> LogType: Error. </summary>
        public static class LogError
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, object[] data) =>
                ExtendedLog (layer, LogType.Warning, null, null, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (params object[] data) =>
                OnLayer(LogLayer.Debug, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (params object[] data) =>
                OnLayer(LogLayer.Physics, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (params object[] data) =>
                OnLayer(LogLayer.Graphics, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (params object[] data) =>
                OnLayer(LogLayer.Logistics, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (params object[] data) =>
                OnLayer(LogLayer.Interface, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (params object[] data) =>
                OnLayer(LogLayer.Mechanics, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (params object[] data) =>
                OnLayer(LogLayer.Services, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (params object[] data) =>
                OnLayer(LogLayer.DataBase, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (params object[] data) =>
                OnLayer(LogLayer.Network, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            public static void Editor (params object[] data) =>
                OnLayer(LogLayer.Editor, data);
        }

        /// <summary> LogType: Error and Formatted. </summary>
        public static class LogErrorFormat
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, Object context, string format, object[] data) =>
                ExtendedLog (layer, LogType.Warning, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Debug, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Physics, context, format, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Graphics, context, format, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Logistics, context, format, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Interface, context, format, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Mechanics, context, format, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Services, context, format, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.DataBase, context, format, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Network, context, format, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            public static void Editor (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Editor, context, format, data);
        }

        /// <summary> LogType: Assert. </summary>
        public static class LogAssert
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, object[] data) =>
                ExtendedLog (layer, LogType.Warning, null, null, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (params object[] data) =>
                OnLayer(LogLayer.Debug, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (params object[] data) =>
                OnLayer(LogLayer.Physics, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (params object[] data) =>
                OnLayer(LogLayer.Graphics, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (params object[] data) =>
                OnLayer(LogLayer.Logistics, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (params object[] data) =>
                OnLayer(LogLayer.Interface, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (params object[] data) =>
                OnLayer(LogLayer.Mechanics, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (params object[] data) =>
                OnLayer(LogLayer.Services, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (params object[] data) =>
                OnLayer(LogLayer.DataBase, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (params object[] data) =>
                OnLayer(LogLayer.Network, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            public static void Editor (params object[] data) =>
                OnLayer(LogLayer.Editor, data);
        }

        /// <summary> LogType: Assert and Formatted. </summary>
        public static class LogAssertFormat
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, Object context, string format, object[] data) =>
                ExtendedLog (layer, LogType.Warning, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Debug, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Physics, context, format, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Graphics, context, format, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Logistics, context, format, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Interface, context, format, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Mechanics, context, format, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Services, context, format, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.DataBase, context, format, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Network, context, format, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            public static void Editor (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Editor, context, format, data);
        }

        /// <summary> LogType: Exception. </summary>
        public static class LogException
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, object[] data) =>
                ExtendedLog (layer, LogType.Warning, null, null, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (params object[] data) =>
                OnLayer(LogLayer.Debug, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (params object[] data) =>
                OnLayer(LogLayer.Physics, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (params object[] data) =>
                OnLayer(LogLayer.Graphics, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (params object[] data) =>
                OnLayer(LogLayer.Logistics, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (params object[] data) =>
                OnLayer(LogLayer.Interface, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (params object[] data) =>
                OnLayer(LogLayer.Mechanics, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (params object[] data) =>
                OnLayer(LogLayer.Services, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (params object[] data) =>
                OnLayer(LogLayer.DataBase, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (params object[] data) =>
                OnLayer(LogLayer.Network, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            public static void Editor (params object[] data) =>
                OnLayer(LogLayer.Editor, data);
        }

        /// <summary> LogType: Exception and Formatted. </summary>
        public static class LogExceptionFormat
        {
            //  Logs an extended log just for this log type.
            internal static void OnLayer(LogLayer layer, Object context, string format, object[] data) =>
                ExtendedLog (layer, LogType.Warning, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Debugging (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Debug, context, format, data);

            /// <summary> Logs physics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Physics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Physics, context, format, data);

            /// <summary> Logs graphics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Graphics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Graphics, context, format, data);

            /// <summary> Logs logistics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Logistics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Logistics, context, format, data);

            /// <summary> Logs UI / UX data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Interface (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Interface, context, format, data);

            /// <summary> Logs mechanics data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Mechanics (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Mechanics, context, format, data);

            /// <summary> Logs services data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Services (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Services, context, format, data);

            /// <summary> Logs data base data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void DataBase (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.DataBase, context, format, data);

            /// <summary> Logs network data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            [System.Diagnostics.Conditional ("DEBUG")]
            public static void Network (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Network, context, format, data);

            /// <summary> Logs Editor data to Unity Console. </summary>
            /// <param name="data"></param>
            /// <param name="context"></param>
            /// <param name="format"></param>
            public static void Editor (Object context, string format, params object[] data) =>
                OnLayer(LogLayer.Editor, context, format, data);
        }

        #endregion



        #region Editor

        /// <summary> Draw bounds in scene. </summary>
        /// <param name="mesh"></param>
        /// <param name="color"></param>
        public static void DrawDebugBounds (MeshFilter mesh, Color color)
        {
            #if UNITY_EDITOR
            if (mesh == null)
                return;
            var renderer = mesh.GetComponent<MeshRenderer> ();
            DrawDebugBounds (renderer, color);
            #endif
        }

        /// <summary> Draw bounds in scene. </summary>
        /// <param name="renderer"></param>
        /// <param name="color"></param>
        public static void DrawDebugBounds (MeshRenderer renderer, Color color)
        {
            #if UNITY_EDITOR
            var bounds = renderer.bounds;
            DrawDebugBounds (bounds, color);
            #endif
        }

        /// <summary> Draw bounds in scene. </summary>
        /// <param name="bounds"></param>
        /// <param name="color"></param>
        public static void DrawDebugBounds (Bounds bounds, Color color)
        {
            #if UNITY_EDITOR

            var center = bounds.center;
            var extents = bounds.extents;
            var diff = center - extents;
            var summ = center + extents;

            //  Getting corners.
            var frontTopLeft = new Vector3 (diff.x, summ.y, diff.z);
            var frontTopRight = new Vector3 (summ.x, summ.y, diff.z);
            var frontBottomLeft = new Vector3 (diff.x, diff.y, diff.z);
            var frontBottomRight = new Vector3 (summ.x, diff.y, diff.z);
            var backTopLeft = new Vector3 (diff.x, summ.y, summ.z);
            var backTopRight = new Vector3 (summ.x, summ.y, summ.z);
            var backBottomLeft = new Vector3 (diff.x, diff.y, summ.z);
            var backBottomRight = new Vector3 (summ.x, diff.y, summ.z);

            //  Drawing edges.
            Debug.DrawLine (frontTopLeft, frontTopRight, color);
            Debug.DrawLine (frontTopRight, frontBottomRight, color);
            Debug.DrawLine (frontBottomRight, frontBottomLeft, color);
            Debug.DrawLine (frontBottomLeft, frontTopLeft, color);

            Debug.DrawLine (backTopLeft, backTopRight, color);
            Debug.DrawLine (backTopRight, backBottomRight, color);
            Debug.DrawLine (backBottomRight, backBottomLeft, color);
            Debug.DrawLine (backBottomLeft, backTopLeft, color);

            Debug.DrawLine (frontTopLeft, backTopLeft, color);
            Debug.DrawLine (frontTopRight, backTopRight, color);
            Debug.DrawLine (frontBottomRight, backBottomRight, color);
            Debug.DrawLine (frontBottomLeft, backBottomLeft, color);
            #endif
        }

        /// <summary> Draws a text in scene. </summary>
        /// <param name="text"></param>
        /// <param name="worldPos"></param>
        /// <param name="color"></param>
        public static void DrawString (string text, Vector3 worldPos, Color ? color = null)
        {
            #if UNITY_EDITOR

            //  Setting color.
            var defaultColor = GUI.color;
            Handles.BeginGUI ();

            if (color.HasValue)
                GUI.color = color.Value;

            //  Getting variables.
            var view = SceneView.currentDrawingSceneView;
            var screenPos = view.camera.WorldToScreenPoint (worldPos);
            var size = GUI.skin.label.CalcSize (new GUIContent (text));

            //  Drawing label.
            GUI.Label (
                position: new Rect (
                    x: screenPos.x - (size.x / 2),
                    y: -screenPos.y + view.position.height + 4,
                    width: size.x,
                    height: size.y
                ),
                text: text
            );

            //  Reseting color.
            Handles.EndGUI ();
            GUI.color = defaultColor;

            #endif
        }

        /// <summary> Draws a vector in scene. </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="headLength"></param>
        /// <param name="headAngle"></param>
        public static void DrawVector (Vector3 position, Vector3 direction, float headLength = 0.25f, float headAngle = 20.0f)
        {
            #if UNITY_EDITOR

            //  Getting variables.
            var forward = Vector3.forward;
            var rotation = Quaternion.LookRotation (direction);
            var right = Quaternion.Euler (0, 180 + headAngle, 0);
            var left = Quaternion.Euler (0, 180 - headAngle, 0);

            //  Draws de vector.
            Debug.DrawRay (position, direction);
            Debug.DrawRay (
                start: position + direction,
                dir: right * rotation * forward * headLength
            );
            Debug.DrawRay (
                start: position + direction,
                dir: left * rotation * forward * headLength
            );
            #endif
        }

        #endregion
    }
}
*/
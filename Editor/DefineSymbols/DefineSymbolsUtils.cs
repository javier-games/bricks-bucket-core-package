using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace BricksBucket.Core.Editor.DefineSymbols
{
    /// <!-- DefineSymbolsUtils -->
    ///
    /// <summary>
    /// <para>
    /// Defines the different compiler types to Defined Symbols.
    /// </para>
    /// <para>
    /// Based in the UnityDefineManager by @karl-.
    /// https://github.com/karl-/UnityDefineManager
    /// </para>
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class DefineSymbolsUtils
    {

        #region Constants

        /*
         *  Constants from unity.
         *  Do not modify this.
         *  http://forum.unity3d.com/threads/93901-global-define/page2
         *
         */
        private const string CSharpPath = "Assets/csc.rsp";
        private const string EditorPath = "Assets/gmcs.rsp";

        private const string AssetsFolder = "Assets";
        private const string CSharpExtension = "*.cs";
        private const string MetaExtension = ".meta";
        private const string DefineDeclaration = "-define:";
        private const char   SemicolonChar = ';';

        #endregion



        #region Public Static Methods

        /// <summary> Get the defined symbols. </summary>
        /// <param name="compiler"></param>
        /// <returns> Array of defined symbols. </returns>
        public static string[] GetDefines (Compiler compiler)
        {
            switch (compiler)
            {
                case Compiler.C_SHARP:
                    return ParseRspFile (CSharpPath);
                case Compiler.EDITOR:
                    return ParseRspFile (EditorPath);
                default:
                    return null;
            }
        }

        /// <summary> Modifies the current defined symbols. </summary>
        /// <param name="compiler"></param>
        /// <param name="defs"></param>
        public static void SetDefines (Compiler compiler, string[] defs)
        {
            switch (compiler)
            {
                case Compiler.C_SHARP:
                WriteDefines (CSharpPath, defs);
                break;

                case Compiler.EDITOR:
                WriteDefines (EditorPath, defs);
                break;
            }

            string first = Directory.GetFiles (
                path: AssetsFolder,
                searchPattern: CSharpExtension,
                searchOption: SearchOption.AllDirectories
            ).FirstOrDefault ();

            if (!string.IsNullOrEmpty (first))
                AssetDatabase.ImportAsset (first);
        }

        /// <summary> Parse defined symbols from main file. </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] ParseRspFile (string path)
        {
            if (!File.Exists (path))
                return new string[0];

            string[] lines = File.ReadAllLines (path);
            List<string> defs = new List<string> ();

            foreach (string line in lines)
                if (line.StartsWith (
                    value: DefineDeclaration,
                    comparisonType: System.StringComparison.Ordinal))
                    defs.AddRange (
                        line.Replace (
                            oldValue: DefineDeclaration,
                            newValue: string.Empty
                        ).Split (SemicolonChar)
                    );

            return defs.ToArray ();
        }

        /// <summary> Write current defines to the main file. </summary>
        /// <param name="path"></param>
        /// <param name="defs"></param>
        public static void WriteDefines (string path, string[] defs)
        {
            if (defs == null || (defs.Length < 1 && File.Exists (path)))
            {
                File.Delete (path);
                File.Delete (path + MetaExtension);
                AssetDatabase.Refresh ();
                return;
            }

            var sb = new StringBuilder ();

            sb.Append (DefineDeclaration);

            for (int i = 0; i < defs.Length; i++)
            {
                sb.Append (defs[i]);
                if (i < defs.Length - 1)
                    sb.Append (SemicolonChar);
            }

            using (var writer = new StreamWriter (path, false))
            {
                writer.Write (sb);
            }
        }

        #endregion
    }
}
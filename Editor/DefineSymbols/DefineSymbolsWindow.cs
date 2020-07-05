using UnityEditor;
using UEditor = UnityEditor.Editor;

namespace BricksBucket.Core.Editor.DefineSymbols
{
    /// <!-- DefineSymbolsUtils -->
    ///
    /// <summary>
    /// <para>
    /// Editor Window to show scripting defines symbols.
    /// </para>
    /// <para>
    /// Based in the UnityDefineManager by @karl-.
    /// https://github.com/karl-/UnityDefineManager
    /// </para>
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public class ScriptingDefineWindow : EditorWindow
    {


        #region Class Members

        private UEditor _editor;
        private DefineSymbols _asset;

        private const string MenuPath =
            "Tools/Bricks Bucket/Define Symbols Editor";
        private const string Title =
            "Define Symbol Editor";

        #endregion



        #region EditorWindow Methods

        //  Called on Init.
        [MenuItem (MenuPath)]
        private static void Init ()
        {
            GetWindow<ScriptingDefineWindow> (
                utility: true,
                title: Title,
                focus: true
            );
        }

        //  Called on enable.
        private void OnEnable ()
        {
            _asset = CreateInstance<DefineSymbols> ();
            _editor = UEditor.CreateEditor (_asset);
        }

        //  Called on disable.
        private void OnDisable ()
        {
            DestroyImmediate (_editor);
            DestroyImmediate (_asset);
        }

        //  Called on GUI.
        private void OnGUI ()
        {
            _editor.OnInspectorGUI ();
        }

        #endregion
    }
}
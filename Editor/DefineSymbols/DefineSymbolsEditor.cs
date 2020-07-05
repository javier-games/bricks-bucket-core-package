using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UEditor = UnityEditor.Editor;

namespace BricksBucket.Core.Editor.DefineSymbols
{
    /// <!-- DefineSymbolsEditor -->
    ///
    /// <summary>
    /// <para>
    /// Editor for Scripting Define Object.
    /// </para>
    /// <para>
    /// Based in the UnityDefineManager by @karl-.
    /// https://github.com/karl-/UnityDefineManager
    /// </para>
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    [CustomEditor (typeof (DefineSymbols))]
    public class DefineSymbolsEditor : UEditor
    {

        #region Fields

        private const int CompilerCount = 3;
        // ReSharper disable once IdentifierTypo
        private ReorderableList _reorderableList;
        private BuildTargetGroup _currentTargetGroup;

        private SerializedProperty _compilerProperty;
        private SerializedProperty _buildTargetProperty;
        private SerializedProperty _definesProperty;
        private SerializedProperty _isAppliedProperty;

        #endregion


        #region MonoBehaviour Methods

        //  Called on enable.
        private void OnEnable ()
        {
            //  Initialize properties.
            _compilerProperty = serializedObject.FindProperty ("_compiler");
            SetCompilerTarget ((Compiler) _compilerProperty.intValue);

            //  Initialize re order-able list.
            _reorderableList = new ReorderableList (serializedObject, _definesProperty);
            _reorderableList.drawHeaderCallback += OnDrawHeader;
            _reorderableList.drawElementCallback += OnDrawListElement;
        }

        //  Called on disable.
        /*void OnDisable ()
        {
            //  Ask for save.
            if (_isAppliedProperty != null && _isAppliedProperty.serializedObject != null)
                if (!_isAppliedProperty.boolValue)
                {
                    if (EditorUtility.DisplayDialog (
                        title: "Unsaved Changes",
                        message: "Would you like to save changes to the scripting defines?",
                        ok: "Yes",
                        cancel: "No"
                    ))
                        ApplyDefines ();
                }
        }*/

        #endregion



        #region Override Methods

        /// <summary> Called On Inspector GUI. </summary>
        public override void OnInspectorGUI ()
        {

            serializedObject.Update ();

            Color oldColor = GUI.backgroundColor;

            GUILayout.Space (2);

            GUILayout.BeginHorizontal ();

            for (int i = 0; i < CompilerCount; i++)
            {
                if (i == _compilerProperty.intValue)
                    GUI.backgroundColor = Color.gray;

                GUIStyle st;
                switch (i)
                {
                    case 0:
                    st = EditorStyles.miniButtonLeft;
                    break;
                    case CompilerCount - 1:
                    st = EditorStyles.miniButtonRight;
                    break;
                    default:
                    st = EditorStyles.miniButtonMid;
                    break;
                }

                if (GUILayout.Button (((Compiler) i).ToString (), st))
                {
                    _compilerProperty.intValue = i;
                    SetCompilerTarget ((Compiler) i);
                }

                GUI.backgroundColor = oldColor;
            }

            GUILayout.EndHorizontal ();

            if (_compilerProperty.intValue == (int) Compiler.PLATFORM)
            {
                var cur = (BuildTargetGroup) _buildTargetProperty.intValue;

                GUILayout.Space (3);

                EditorGUI.BeginChangeCheck ();
                cur = (BuildTargetGroup) EditorGUILayout.EnumPopup (cur);
                if (EditorGUI.EndChangeCheck ())
                    SetBuildTarget (cur);
            }


            EditorGUI.BeginChangeCheck ();

            GUILayout.BeginVertical (new GUIStyle
            {
                margin = new RectOffset (4, 4, 4, 4)
            });

            _reorderableList.DoLayoutList ();
            _isAppliedProperty.boolValue &= !EditorGUI.EndChangeCheck ();

            GUILayout.EndVertical ();




            GUILayout.BeginHorizontal ();

            GUILayout.FlexibleSpace ();

            bool wasEnabled = GUI.enabled;

            GUI.enabled = !_isAppliedProperty.boolValue;

            if (GUILayout.Button ("Apply", EditorStyles.miniButton))
                ApplyDefines ();

            GUI.enabled = wasEnabled;

            GUILayout.EndHorizontal ();

            serializedObject.ApplyModifiedProperties ();
        }

        #endregion



        #region Methods

        //  Change of compiler.
        private void SetCompilerTarget (Compiler compiler)
        {
            _compilerProperty.intValue = (int) compiler;

            _definesProperty = serializedObject.FindProperty ("_defines");
            _isAppliedProperty = serializedObject.FindProperty ("_isApplied");

            if (_compilerProperty.intValue == (int) Compiler.PLATFORM)
            {
                _buildTargetProperty = serializedObject.FindProperty ("_buildTarget");
                _currentTargetGroup = (BuildTargetGroup) _buildTargetProperty.intValue;

                SetBuildTarget (_currentTargetGroup == BuildTargetGroup.Unknown
                    ? BuildPipeline.GetBuildTargetGroup (EditorUserBuildSettings.activeBuildTarget)
                    : _currentTargetGroup);
            }
            else
            {
                var defs = DefineSymbolsUtils.GetDefines ((Compiler) _compilerProperty.intValue);

                _definesProperty.arraySize = defs.Length;

                for (int i = 0; i < defs.Length; i++)
                    _definesProperty.GetArrayElementAtIndex (i).stringValue = defs[i];

                _isAppliedProperty.boolValue = true;
                serializedObject.ApplyModifiedProperties ();
            }
        }

        //  Set of building target.
        private void SetBuildTarget (BuildTargetGroup buildTarget)
        {
            _currentTargetGroup = buildTarget;
            _buildTargetProperty.intValue = (int) buildTarget;

            var defs = GetScriptingDefineSymbols ((BuildTargetGroup) _buildTargetProperty.enumValueIndex);
            _definesProperty.arraySize = defs.Length;
            for (int i = 0; i < defs.Length; i++)
                _definesProperty.GetArrayElementAtIndex (i).stringValue = defs[i];

            _isAppliedProperty.boolValue = true;
            serializedObject.ApplyModifiedProperties ();
        }


        static string[] GetScriptingDefineSymbols (BuildTargetGroup group)
        {
            string res = PlayerSettings.GetScriptingDefineSymbolsForGroup (group);
            return res.Split (';');
        }

        //  Save current changes.
        private void ApplyDefines ()
        {
            string[] arr = new string[_definesProperty.arraySize];

            for (int i = 0, c = arr.Length; i < c; i++)
                arr[i] = _definesProperty.GetArrayElementAtIndex (i).stringValue;

            if (_compilerProperty.intValue == (int) Compiler.PLATFORM)
                PlayerSettings.SetScriptingDefineSymbolsForGroup (_currentTargetGroup, string.Join (";", arr));
            else
                DefineSymbolsUtils.SetDefines ((Compiler) _compilerProperty.intValue, arr);

            _isAppliedProperty.boolValue = true;

            serializedObject.ApplyModifiedProperties ();

            GUI.FocusControl (string.Empty);
        }

        //  Called on DrawHeaderCallback.
        private void OnDrawHeader (Rect rect)
        {
            var cur = ((Compiler) _compilerProperty.intValue).ToString ();

            if (_compilerProperty.intValue == (int) Compiler.PLATFORM)
                cur += " " + ((BuildTargetGroup) (_buildTargetProperty.intValue));

            GUI.Label (rect, cur, EditorStyles.boldLabel);
        }

        //   Called on DrawElementCallback.
        private void OnDrawListElement (Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex (index);

            EditorGUIUtility.labelWidth = 4;
            EditorGUI.PropertyField (new Rect (rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element);
            EditorGUIUtility.labelWidth = 0;
        }
        
        #endregion
    }
}
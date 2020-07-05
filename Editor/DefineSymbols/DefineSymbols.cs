using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Editor.DefineSymbols
{
    /// <!-- DefineSymbols -->
    ///
    /// <summary>
    /// <para>
    /// Stores the information of defined symbols.
    /// </para>
    /// <para>
    /// Based in the UnityDefineManager by @karl-.
    /// https://github.com/karl-/UnityDefineManager
    /// </para>
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public class DefineSymbols : ScriptableObject
    {
        #region Class Members

        [SerializeField]
        protected Compiler m_compiler;               //  Compiler.

        [SerializeField]
        protected BuildTargetGroup m_buildTarget;    //  Platform.

        [SerializeField]
        protected string[] m_defines;                //  Symbols.

        [SerializeField]
        protected bool m_isApplied;                  //  Flag to apply.

        #endregion
    }
}

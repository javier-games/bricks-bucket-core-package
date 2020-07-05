namespace BricksBucket.Core.Editor.DefineSymbols 
{
    /// <!-- Compiler -->
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
    public enum Compiler
    {
        C_SHARP = 0,     //  Global define symbols for C# scripts.
        EDITOR = 1,     //  Global define symbols for Editor scripts.
        PLATFORM = 2    //  Global define symbols per platform.
    }
}

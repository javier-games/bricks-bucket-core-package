namespace Monogum.BricksBucket.Core.Generics
{
    public interface IAutogeneratedScript
    {
        /// <summary>
        /// Namespace of the script.
        /// </summary>
        /// <returns>Empty if has not namespace.</returns>
        string NameSpace { get; }
        
        /// <summary>
        /// Path of the file. Path with out Assets
        /// folder, Assets folder is considered as the root.
        /// </summary>
        /// <returns>Empty if has not been written.</returns>
        string Path { get; }

        /// <summary>
        /// Name of the class.
        /// </summary>
        /// <returns>Empty if has not been written.</returns>
        string ClassName { get; }

        /// <summary>
        /// Extension of the file with out point.
        /// </summary>
        /// <returns>Empty if has not been written.</returns>
        string Extension { get; }
    }
}
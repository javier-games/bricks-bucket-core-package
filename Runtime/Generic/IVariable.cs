namespace BricksBucket.Core.Generic
{
    /// <!-- IVariable -->
    /// <summary>
    /// Interface for dynamic type variable.
    /// </summary>
    public interface IVariable
    {
        /// <summary>
        /// Type of variable.
        /// </summary>
        /// <returns>Type of the variable.</returns>
        DataType Type { get; }

        /// <summary>
        /// Returns this variable to an object of desired type.
        /// </summary>
        /// <param name="desiredType">Desired Type.</param>
        /// <returns>Value of the variable.</returns>
        /// <exception cref="System.Exception">Throw exception fi the data type
        /// property is corrupted.</exception>
        object Get(System.Type desiredType);

        /// <summary>
        /// Sets the value of the variable with the specified value.
        /// </summary>
        void Set(object value);
    }
}
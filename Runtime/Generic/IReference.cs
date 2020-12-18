using UnityEngine;

namespace BricksBucket.Core.Generic
{
    /// <!-- IReference -->
    /// <summary>
    /// Interface for a dynamic reference.
    /// </summary>
    public interface IReference
    {
        /// <summary>
        /// Reference to the instance object.
        /// </summary>
        /// <returns>A reference.</returns>
        Object Component { get; }
        
        /// <summary>
        /// Name of the property of the component.
        /// </summary>
        /// <returns>Null if has not been assigned.</returns>
        string Property { get; }
        
        /// <summary>
        /// Value of the property.
        /// </summary>
        /// <returns>Null if has not been assigned.</returns>
        IVariable Variable { get; }
        
        /// <summary>
        /// Instance of hardwired class.
        /// </summary>>
        /// <returns>Null if has not been assigned.</returns>
        IHardwiredRegistry Hardwired { get; }

        /// <summary>
        /// Set the object reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        void SetReference(Object reference);

        /// <summary>
        /// Sets the property.
        /// </summary>
        void SetProperty(string propertyName);

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        object GetValue();

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        void SetValue(object propertyValue);
    }
}
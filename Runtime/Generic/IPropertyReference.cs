using UnityEngine;
using Object = UnityEngine.Object;

namespace BricksBucket.Core.Generic
{
    /// <!-- IReference -->
    /// <summary>
    /// Interface for a dynamic reference.
    /// </summary>
    public interface IPropertyReference
    {
        /// <summary>
        /// Reference to the instance object.
        /// </summary>
        /// <returns>A reference.</returns>
        Component Component { get; }
        
        /// <summary>
        /// Name of the property of the component.
        /// </summary>
        /// <returns>Null if has not been assigned.</returns>
        string Property { get; }
        
        /// <summary>
        /// Instance of hardwired class.
        /// </summary>>
        /// <returns>Null if has not been assigned.</returns>
        IComponentRegistry ComponentRegistry { get; }

        /// <summary>
        /// Set the object reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        void SetComponent(Component reference);

        /// <summary>
        /// Sets the property.
        /// </summary>
        void SetProperty(string property);

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        object GetValue();

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        void SetValue(object value);
    }
}
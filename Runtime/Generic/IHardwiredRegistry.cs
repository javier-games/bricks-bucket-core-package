using System;
using System.Collections.Generic;

namespace BricksBucket.Core.Generic
{
    /// <!-- IHardwiredRegistry -->
    /// <summary>
    /// Interface for a hardwired registry of properties.
    /// </summary>
    public interface IHardwiredRegistry
    {
        /// <summary>
        /// Path of the hardwired script inheritor.
        /// </summary>
        /// <returns>Empty if has not inheritor.</returns>
        string Path { get; }
        
        /// <summary>
        /// Namespace of the hardwired script inheritor.
        /// </summary>
        /// <returns>Empty if has not inheritor.</returns>
        string NameSpace { get; }

        /// <summary>
        /// Returns the array of all registered types.
        /// </summary>
        /// <value>The array.</value>
        IEnumerable<Type> Array { get; }
        
        /// <summary>
        /// Whether the list of registered types contains the given component.
        /// </summary>
        /// <returns><value>TRUE</value>, if the component is contained.
        /// </returns>
        /// <param name="component">Component to look for.</param>
        bool ContainsComponent(string component);
        
        /// <summary>
        /// Whether the list of registered types contains the given property
        /// for the given component.</summary>
        /// <returns><value>TRUE</value>, if the component and the property
        /// are contained.</returns>
        /// <param name="component">Component of the property to look for.
        /// </param>
        /// <param name="property">Property to look for.</param>
        bool ContainsProperty(string component, string property);
        
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="component">Component of the property.</param>
        /// <param name="property">Property name.</param>
        /// <returns>The value.</returns>
        object GetValue(object component, string property);

        /// <summary>
        /// Sets the value of the given property of the given component.
        /// </summary>
        /// <param name="component">Component of the property.</param>
        /// <param name="property">Property name.</param>
        /// <param name="value">Value.</param>
        void SetValue(object component, string property, object value);
    }
}
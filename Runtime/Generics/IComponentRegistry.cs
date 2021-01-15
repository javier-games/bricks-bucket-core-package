using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Monogum.BricksBucket.Core.Generics
{
    /// <!-- IComponentRegistry -->
    /// <summary>
    /// Interface for a hardwired registry of properties.
    /// </summary>
    public interface IComponentRegistry
    {
        /// <summary>
        /// Returns the collection of all registered types.
        /// </summary>
        /// <value>The enumerable collection of elements.</value>
        IEnumerable<Type> ComponentTypes { get; }
        
        /// <summary>
        /// Whether the list of registered types contains the given component.
        /// </summary>
        /// <returns><value>TRUE</value>, if the component is contained.
        /// </returns>
        /// <param name="component">Component to look for.</param>
        bool ContainsComponent(Object component);
        
        /// <summary>
        /// Whether the list of registered types contains the given property
        /// for the given component.</summary>
        /// <returns><value>TRUE</value>, if the component and the property
        /// are contained.</returns>
        /// <param name="component">Component of the property to look for.
        /// </param>
        /// <param name="property">Property to look for.</param>
        bool ContainsProperty(Object component, string property);
        
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="component">Component of the property.</param>
        /// <param name="property">Property name.</param>
        /// <returns>The value.</returns>
        object GetValue(Object component, string property);

        /// <summary>
        /// Sets the value of the given property of the given component.
        /// </summary>
        /// <param name="component">Component of the property.</param>
        /// <param name="property">Property name.</param>
        /// <param name="value">Value.</param>
        void SetValue(Object component, string property, object value);
    }
}
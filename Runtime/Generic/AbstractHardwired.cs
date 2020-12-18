using System;
using System.Collections.Generic;

// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace BricksBucket.Core.Generic
{
	/// <!-- AbstractHardwired -->
	/// <summary>
	/// Since iOS cannot support System.Reflection, AbstractReference has to
	/// have this static class to cast values.
	/// </summary>
	public abstract class AbstractHardwired : IHardwiredRegistry
	{
		/// <summary>
		/// Namespace of the hardwired script inheritor.
		/// </summary>
		/// <returns>Empty if has not inheritor.</returns>
		public virtual string NameSpace => string.Empty;
		
		/// <summary>
		/// Path of the hardwired script inheritor.
		/// </summary>
		/// <returns>Empty if has not inheritor.</returns>
		public virtual string Path => string.Empty;

		/// <summary>
		/// Collection of all registered types.
		/// </summary>
		protected virtual List<Type> TypesList { get; } = new List<Type>();

		/// <summary>
		/// Dictionary of actions to set values.
		/// </summary>
		protected virtual Dictionary<string,
			Dictionary<string, Action<object, object>>> Set
		{
			get;
		}
			= new Dictionary<string,
				Dictionary<string,
					Action<object, object>
				>
			>();

		/// <summary>
		/// Dictionary of functions to return values.
		/// </summary>
		protected virtual
			Dictionary<string, Dictionary<string, Func<object, object>>> Get
		{
			get;
		}
			=
			new Dictionary<string,
				Dictionary<string,
					Func<object, object>
				>
			>();

		/// <summary>
		/// Returns the array of all registered types.
		/// </summary>
		/// <value>The array.</value>
		public IEnumerable<Type> Array => TypesList.ToArray();

		/// <summary>
		/// Whether the list of registered types contains the given component.
		/// </summary>
		/// <returns><value>TRUE</value>, if the component is contained.
		/// </returns>
		/// <param name="component">Component to look for.</param>
		public bool ContainsComponent(string component) =>
			Set.ContainsKey(component) &&
			Get.ContainsKey(component);

		/// <summary>
		/// Whether the list of registered types contains the given property
		/// for the given component.</summary>
		/// <returns><value>TRUE</value>, if the component and the property
		/// are contained.</returns>
		/// <param name="component">Component of the property to look for.
		/// </param>
		/// <param name="property">Property to look for.</param>
		public bool
			ContainsProperty(string component, string property) =>
			Set[component].ContainsKey(property) &&
			Get[component].ContainsKey(property);

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="component">Component of the property.</param>
		/// <param name="property">Property name.</param>
		/// <returns>The value.</returns>
		public object GetValue(object component, string property)
		{
			try
			{
				return Get[component.GetType().ToString()]
					[property](component);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(exception);
				return null;
			}
		}

		/// <summary>
		/// Sets the value of the given property of the given component.
		/// </summary>
		/// <param name="component">Component of the property.</param>
		/// <param name="property">Property name.</param>
		/// <param name="value">Value.</param>
		public void SetValue(object component, string property,
			object value)
		{
			try
			{
				Set[component.GetType().ToString()]
					[property](component, value);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(exception);
			}
		}
	}
}
using UnityEngine;

namespace Monogum.BricksBucket.Core.Generics
{
    /// <summary>
    /// Dynamic reference to component properties.
    /// </summary>
    /// <typeparam name="TRegistry">Type of the hardwired script inheritor.
    /// </typeparam>
    /// <typeparam name="TValue">Type of the value of the property.</typeparam>
    [System.Serializable]
    public abstract class AbstractPropertyReference<TRegistry, TValue> :
        IPropertyReference
        where TRegistry : AbstractComponentRegistry, new()
    {

        #region Fields

        /// <summary>
        /// Instance of hardwired class.
        /// </summary>
        private static TRegistry _hardwired = new TRegistry();

        /// <summary>
        /// Reference to the instance object.
        /// </summary>
        [SerializeField]
        private Component component;

        /// <summary>
        /// Name of the property of the component.
        /// </summary>
        [SerializeField]
        private string property;

        #endregion


        #region Accessors

        /// <summary>
        /// Value of the property.
        /// </summary>
        public virtual TValue Value { get; set; }

        /// <summary>
        /// Reference to the component that contains the property.
        /// </summary>
        /// <returns>Reference to the component that contains the property.
        /// </returns>
        public Component Component
        {
            get => component;
            protected set => component = value;
        }

        /// <summary>
        /// Name of the property of the component.
        /// </summary>
        /// <returns>Null if has not been assigned.</returns>
        public string Property
        {
            get => property;
            protected set => property = value;
        }

        /// <summary>
        /// Instance of hardwired class.
        /// </summary>>
        /// <returns>Null if has not been assigned.</returns>
        public virtual IComponentRegistry ComponentRegistry =>
            _hardwired ??= new TRegistry();


        #endregion


        #region Class Implementation

        /// <summary>
        /// Initializes a new instance of the AbstractReference class.
        /// </summary>
        protected AbstractPropertyReference()
        {
            Component = null;
            Property = string.Empty;
        }

        /// <summary>
        /// Set the object reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        public void SetComponent(Component reference)
        {
            if (Component == reference) return;
            Component = reference;
            Property = string.Empty;
            UpdateValue(GetValue());
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        public void SetProperty(string propertyName)
        {
            Property = propertyName;
            UpdateValue(GetValue());
        }

        /// <summary>
        /// Updates the variable.
        /// </summary>
        public virtual void UpdateValue(object currentPropertyValue) { }

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        public object GetValue()
        {
            var valid =
                _hardwired.ContainsComponent(Component) &&
                _hardwired.ContainsProperty(Component, Property);
            if (!valid) return null;

            try
            {
                return _hardwired.GetValue(Component, Property);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        public void SetValue(object propertyValue)
        {
            var valid =
                _hardwired.ContainsComponent(Component) &&
                _hardwired.ContainsProperty(Component, Property);
            if (!valid) return;

            try
            {
                _hardwired.SetValue(Component, Property, propertyValue);
            }
            catch
            {
                // ignored
            }
        }

        #endregion

    }
}
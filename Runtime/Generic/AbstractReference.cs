using UnityEngine;

namespace BricksBucket.Core.Generic {

    /// <summary>
    /// Dynamic reference to component properties.
    /// </summary>
    /// <typeparam name="THardwired">Type of the hardwired script inheritor.
    /// </typeparam>
    [System.Serializable]
    public abstract class AbstractReference<THardwired> : IReference
    where THardwired : AbstractHardwired, new()
    {
        
        #region Fields

        /// <summary>
        /// Instance of hardwired class.
        /// </summary>
        private static THardwired _hardwired = new THardwired();

        /// <summary>
        /// Reference to the instance object.
        /// </summary>
        [SerializeField]
        private Object component;
        
        /// <summary>
        /// Name of the property of the component.
        /// </summary>
        [SerializeField]
        private string property;
        
        /// <summary>
        /// Value of the property.
        /// </summary>
        [SerializeField]
        private Variable value;

        #endregion


        #region Accessors

        /// <summary>
        /// Reference to the instance object.
        /// </summary>
        /// <returns>A reference.</returns>
        public Object Component {
            get => component;
            protected set => component = value;
        }

        /// <summary>
        /// Name of the property of the component.
        /// </summary>
        /// <returns>Null if has not been assigned.</returns>
        public string Property {
            get => property;
            protected set => property = value;
        }

        /// <summary>
        /// Value of the property.
        /// </summary>
        /// <returns>Null if has not been assigned.</returns>
        public virtual IVariable Variable => value;

        /// <summary>
        /// Instance of hardwired class.
        /// </summary>>
        /// <returns>Null if has not been assigned.</returns>
        public virtual IHardwiredRegistry Hardwired =>
            _hardwired ?? (_hardwired = new THardwired());


        #endregion
        

        #region Class Implementation

        /// <summary>
        /// Initializes a new instance of the AbstractReference class.
        /// </summary>
        protected AbstractReference () {
            Component = null;
            Property = string.Empty;
            value = new Variable();
        }

        /// <summary>
        /// Set the object reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        public void SetReference (Object reference)
        {
            if (Component == reference) return;
            Component = reference;
            Property = string.Empty;
            value.Type = DataType.NULL;
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        public void SetProperty (string propertyName) {
            Property = propertyName;
            UpdatedDynVar ();
        }

        /// <summary>
        /// Updates the variable.
        /// </summary>
        public void UpdatedDynVar () {
            if (GetValue () != null)
                value.Set (GetValue ());
        }

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        public object GetValue () {
            if (Component == null || string.IsNullOrEmpty (Property)) {
                return null;
            }

            try { return _hardwired.GetValue (Component, Property); }
            catch (System.Exception) {
                return null;
            }
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        public void SetValue (object propertyValue) {

            if (Component == null || string.IsNullOrEmpty (Property))
                return;

            try
            {
                _hardwired.SetValue (Component, Property, propertyValue);
            }
            catch (System.Exception)
            {
                // ignored
            }
        }

        #endregion

    }
}
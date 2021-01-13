namespace Monogum.BricksBucket.Core.Generics
{
    public class AbstractValue: IValue
    {
        /// <summary>
        /// Returns the value of the variable to an object of a T type.
        /// </summary>
        /// <typeparam name="T">Desired type.</typeparam>
        /// <returns>The value of the variable of the type T.</returns>
        public T Get<T> () { return (T)Get (typeof (T)); }
        
        /// <summary>
        /// Returns this variable to an object of desired type.
        /// </summary>
        /// <param name="desiredType">Desired Type.</param>
        /// <returns>Value of the variable.</returns>
        public virtual object Get(System.Type desiredType) { return null; }

        /// <summary>
        /// Sets the value of the variable with the specified value.
        /// </summary>
        public virtual void Set(object value) { }
    }
}
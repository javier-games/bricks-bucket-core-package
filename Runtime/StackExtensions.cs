using System.Collections.Generic;

namespace Monogum.BricksBucket.Core
{
    /// <!-- ListExtensions -->
    /// 
    /// <summary>
    /// Collection of extension methods for the
    /// <see href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.collections.stack">
    /// Generic.Stack</see> class.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.collections.stack">
    /// Generic.Stack</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class StackExtensions
    {
        /// <summary>
        /// Removes an element from the stack.
        /// </summary>
        /// <typeparam name="T">Type of the stack.</typeparam>
        /// <param name="stack">Stack collection.</param>
        /// <param name="index">Index to remove.</param>
        public static void RemoveAt<T> (this Stack<T> stack, int index)
        {
            if (stack.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (stack);

            if (!stack.HasIndex (index))
                throw Bucket.IndexOutOfRangeException (stack, index);

            Stack<T> aux = new Stack<T> ();
            int times = stack.Count - index - 1;

            for (int i = 0; i < times; i++) aux.Push (stack.Pop ());

            stack.Pop ();

            for (int i = 0; i < times; i++) stack.Push (aux.Pop ());
        }

        /// <summary>
        /// Removes the specified element from stack.
        /// </summary>
        /// <typeparam name="T">Type of stack.</typeparam>
        /// <param name="stack">Stack Collection.</param>
        /// <param name="element">Element to remove.</param>
        public static void Remove<T> (this Stack<T> stack, T element)
        {
            if (stack.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (stack);

            int index = stack.Find (element);
            if (index >= 0)
                stack.RemoveAt (index);
            else
                throw Bucket.ElementNotFoundException (stack, element);
        }

        /// <summary>
        /// Finds the index of an element on stack.
        /// </summary>
        /// <typeparam name="T">Type of stack.</typeparam>
        /// <param name="stack">Stack Collection.</param>
        /// <param name="element">Element to find.</param>
        /// <returns> -1 if an elements does not found. </returns>
        public static int Find<T> (this Stack<T> stack, T element)
        {
            return stack.ToArray ().Find (element);
        }
    }
}
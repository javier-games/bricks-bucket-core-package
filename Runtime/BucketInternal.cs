using System;

namespace Monogum.BricksBucket.Core
{
    /* INTERNAL BUCKET */
    public static partial class Bucket
	{
        /// <summary> Value out of range. </summary>
        /// <param name="collection"> Collection with exception. </param>
        /// <param name="index"> Index out of range. </param>
        /// <returns> Exception. </returns>
        internal static Exception IndexOutOfRangeException (
            object collection,
            int index
        )
        {
            return new Exception (
                string.Format (
                    format: "The collection {0} does not contains index {1}.",
                     new[] {collection, index}
                )
            );
        }

        /// <summary> Value out of range. </summary>
        /// <param name="collection"> Collection with exception. </param>
        /// <param name="element"> Element not found. </param>
        /// <returns> Exception. </returns>
        internal static Exception
            ElementNotFoundException<T> (object collection, T element)
        {
            return new Exception (
                string.Format (
                    format: "The collection {0} does not contains element {1}.",
                    new[] {collection, element}
                )
            );
        }

        /// <summary>Null or empty collection.</summary>
        /// <param name="collection"> Collection null or empty. </param>
        /// <returns>Exception.</returns>
        internal static Exception NullOrEmptyException (object collection)
        {
            return new Exception (
                string.Format (
                    format: "The collection {0} is null or empty.",
                    collection
                )
            );
        }

        /// <summary>Prefab is a null reference.</summary>
        /// <returns>Exception.</returns>
        internal static Exception NullPrefabException () =>
            new Exception ("Empty prefab reference.");

        /// <summary>Instance is a null reference.</summary>
        /// <returns>Exception.</returns>
        internal static Exception NullInstanceException () =>
            new Exception ("Empty instance reference.");
	}
}
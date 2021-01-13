using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Monogum.BricksBucket.Core
{
    /// <!-- EnumerableExtensions -->
    /// 
    /// <summary>
    /// Collection of extension methods for the
    /// <see href=
    /// "https://docs.microsoft.com/dotnet/api/system.collections.ienumerable">
    /// System.IEnumerable</see> interface.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.microsoft.com/dotnet/api/system.collections.ienumerable">
    /// System.IEnumerable</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class EnumerableExtensions
    {
        /// <summary> Verify whether the index is valid. </summary>
        /// <typeparam name="T"> Type of the enumerable. </typeparam>
        /// <param name="enumerable"> Enumerable Collection. </param>
        /// <param name="index"> Index to validate. </param>
        /// <returns> Whether the index is valid. </returns>
        public static bool HasIndex<T> (
            this IEnumerable<T> enumerable, int index
        ) =>
            index < enumerable.Count () && index >= 0;

        /// <summary>
        /// Whether is a null or empty enumerable.
        /// <para>
        /// IEnumerable is relatively slow. Use Array or List as possible.
        /// </para>
        /// </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="enumerable"> List Collection </param>
        /// <returns> Whether is a null or empty list. </returns>
        public static bool IsNullOrEmpty<T> (this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return true;
            return !enumerable.Any ();
        }

        /// <summary>
        /// Returns the element at the specified index.
        /// </summary>
        /// <typeparam name="T">Type of Collection.</typeparam>
        /// <param name="enumerable">Enumerable Collection.</param>
        /// <param name="index">Index of element to change.</param>
        /// <returns>The element at the specified index.</returns>
        public static T GetElement<T> (
            this IEnumerable<T> enumerable, int index
        ) =>
            enumerable.ElementAt (index);

        /// <summary> Returns random element from collection </summary>
        /// <typeparam name="T"> Type of enumerable. </typeparam>
        /// <param name="enumerable"> Enumerable Collection. </param>
        /// <returns> Random element in enumerable. </returns>
        public static T GetRandom<T> (this IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable as T[] ?? enumerable.ToArray ();
            return enumerable1.ElementAt (
                Random.Range (0, enumerable1.Count ()));
        }

        /// <summary> Returns the index of the item. </summary>
        /// <typeparam name="T"> Type of enumerable. </typeparam>
        /// <param name="enumerable"> Enumerable Collection. </param>
        /// <param name="item"> Item to find. </param>
        /// <returns> Index of item. -1 if none found </returns>
        public static int IndexOf<T> (this IEnumerable<T> enumerable, T item)
        {
            var @is = enumerable as T[] ?? enumerable.ToArray ();
            if (@is.IsNullOrEmpty ()) return -1;

            var index = 0;
            foreach (var i in @is)
            {
                if (Equals (i, item)) return index;
                ++index;
            }

            return -1;
        }

        /// <summary> Compares an enumerable with other. </summary>
        /// <typeparam name="T"> Type of the collection. </typeparam>
        /// <param name="enumerable"> Enumerable collection. </param>
        /// <param name="other"> Enumerable to compare. </param>
        /// <returns> Whether two collections are the same. </returns>
        public static bool ContentsMatch<T> (
            this IEnumerable<T> enumerable,
            IEnumerable<T> other
        )
        {
            var elements = enumerable as T[] ?? enumerable.ToArray ();
            var enumerable1 = other as T[] ?? other.ToArray ();

            if (elements.IsNullOrEmpty () && enumerable1.IsNullOrEmpty ())
                return true;

            if (elements.IsNullOrEmpty () || enumerable1.IsNullOrEmpty ())
                return false;

            var firstCount = elements.Count ();
            var secondCount = enumerable1.Count ();

            return firstCount == secondCount && elements.All (
                element => enumerable1.Contains (element)
            );
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Monogum.BricksBucket.Core
{
    /// <!-- ListExtensions -->
    /// 
    /// <summary>
    /// Collection of extension methods for the
    /// <see href="
    /// https://docs.microsoft.com/dotnet/api/system.collections.generic.list-1
    /// ">Generic.List</see> class.
    /// </summary>
    ///
    /// <seealso href="
    /// https://docs.microsoft.com/dotnet/api/system.collections.generic.list-1
    /// ">Generic.List</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class ListExtensions
    {
        /// <summary> Verify whether the index is valid. </summary>
        /// <typeparam name="T"> Type of the list. </typeparam>
        /// <param name="list"> List Collection. </param>
        /// <param name="index"> Index to validate. </param>
        /// <returns> Whether the index is valid. </returns>
        public static bool HasIndex<T> (this IList<T> list, int index) =>
            index < list.Count && index >= 0;

        /// <summary> Whether is a null or empty list. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="list"> List Collection </param>
        /// <returns> Whether is a null or empty list. </returns>
        public static bool IsNullOrEmpty<T> (this IList<T> list)
        {
            if (list == null) return true;
            return list.Count == 0;
        }

        /// <summary> Swaps the values of index A and index B </summary>
        /// <typeparam name="T"> Type of the list. </typeparam>
        /// <param name="list"> List Collection. </param>
        /// <param name="a"> Index A. </param>
        /// <param name="b"> Index B. </param>
        public static void Swap<T> (this IList<T> list, int a, int b)
        {
            if (!list.HasIndex (a))
                throw Bucket.IndexOutOfRangeException (list, a);

            if (!list.HasIndex (b))
                throw Bucket.IndexOutOfRangeException (list, b);

            T value = list[a];
            list[a] = list[b];
            list[b] = value;
        }

        /// <summary> Scramble the elements in this list. </summary>
        /// <typeparam name="T"> Type of list. </typeparam>
        /// <param name="list"> List Collection. </param>
        public static void Shuffle<T> (this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int r = Random.Range (0, i);
                list.Swap (i, r);
            }
        }

        /// <summary> Returns a random element from collection. </summary>
        /// <typeparam name="T"> Type of list. </typeparam>
        /// <param name="list"> List Collection. </param>
        /// <returns> Random element in list. </returns>
        public static T GetRandom<T> (this IList<T> list) =>
            list[Random.Range (0, list.Count)];

        /// <summary> Returns a sample of indexes in the list. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="list"> List Collection. </param>
        /// <param name="size"> Size of the sample. </param>
        /// <param name="random"> Whether to select random elements. </param>
        /// <returns> Sample of indexes. </returns>
        public static int[] GetIndexesSample<T> (
            this IList<T> list,
            int size,
            bool random = false
        )
        {
            if (size < 0) throw Bucket.IndexOutOfRangeException (list, size);

            if (list.IsNullOrEmpty ()) return null;

            int[] sample = new int[size];

            if (random)
                for (int i = 0; i < size; i++)
                    sample[i] = Random.Range (0, list.Count);
            else
                for (int i = 0; i < size; i++)
                {
                    int index = i;
                    if (index >= list.Count) index %= list.Count;
                    sample[i] = index;
                }

            return sample;
        }

        /// <summary> Returns a sample of indexes in the list. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="list"> List Collection. </param>
        /// <param name="size"> Size of the sample. </param>
        /// <param name="random"> Whether to select random elements. </param>
        /// <returns> Sample of indexes. </returns>
        public static T[]
            GetSample<T> (this IList<T> list, int size, bool random = false)
        {
            if (size < 0) throw Bucket.IndexOutOfRangeException (list, size);

            if (list.IsNullOrEmpty ()) return null;

            T[] sample = new T[size];

            if (random)
                for (int i = 0; i < size; i++)
                    sample[i] = list.GetRandom ();
            else
                for (int i = 0; i < size; i++)
                {
                    int index = i;
                    if (index >= list.Count) index %= list.Count;
                    sample[i] = list[index];
                }

            return sample;
        }

        /// <summary> Loops the list with the specified increment. </summary>
        /// <typeparam name="T"> Type of list. </typeparam>
        /// <param name="list"> List Collection. </param>
        /// <param name="index"> Index to loop. </param>
        /// <param name="increment"> Increment to apply. </param>
        /// <returns> Next element in the loop. </returns>
        public static T
            Loop<T> (this List<T> list, ref int index, int increment = 1)
        {
            if (list.IsNullOrEmpty ()) throw Bucket.NullOrEmptyException (list);

            index = index.Loop (0, list.Count - 1, increment);
            return list[index];
        }
    }
}
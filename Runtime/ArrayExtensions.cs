using System;
using System.Collections.Generic;

using Array = System.Array;
using Random = UnityEngine.Random;

namespace Monogum.BricksBucket.Core
{
    /// <!-- ArrayExtensions -->
    /// 
    /// <summary>
    /// Collection of extension methods for the
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.array">
    /// System.Array</see> class.
    /// </summary>
    ///
    /// <seealso href=
    /// "https://docs.microsoft.com/en-us/dotnet/api/system.array">
    /// System.Array</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class ArrayExtensions
    {
        /// <summary> Verify whether the index is valid. </summary>
        /// <typeparam name="T"> Type of the array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        /// <param name="index"> Index to validate. </param>
        /// <returns> Whether the index is valid. </returns>
        public static bool HasIndex<T> (this T[] array, int index) =>
            index < array.Length && index >= 0;

        /// <summary> Whether is a null or empty array. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array Collection </param>
        /// <returns> Whether is a null or empty array. </returns>
        public static bool IsNullOrEmpty<T> (this T[] array)
        {
            if (array == null) return true;
            return array.Length == 0;
        }

        /// <summary> Whether an array has the specified element. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array collection. </param>
        /// <param name="element"> Element to validate. </param>
        /// <returns> -1 if an elements does not found. </returns>
        public static bool Contains<T> (this T[] array, T element) =>
            array.Find (element) >= 0;

        /// <summary>
        /// Whether there is a match in the array for the predicate.
        /// </summary>
        /// <param name="array">Array Collection.</param>
        /// <param name="predicate">Predicate look for.</param>
        /// <typeparam name="T">Type of the elements of the array.</typeparam>
        /// <returns><value>False</value> if can not find a match.</returns>
        public static bool Exists<T> (this T[] array, Predicate<T> predicate) =>
            Array.Exists (array, predicate);

        /// <summary> Finds an element in an array. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array collection. </param>
        /// <param name="element"> Element to find. </param>
        /// <returns> -1 if an elements does not found. </returns>
        public static int Find<T> (this T[] array, T element)
        {
            if (array.IsNullOrEmpty ()) return -1;

            for (int i = 0; i < array.Length; i++)
                if (EqualityComparer<T>.Default.Equals (array[i], element))
                    return i;

            return -1;
        }

        /// <summary>
        /// Finds an element in array.
        /// </summary>
        /// <param name="array">Array collection.</param>
        /// <param name="predicate">Predicate to find an element.</param>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <returns>-1 if an elements does not found.</returns>
        public static int Find<T> (this T[] array, Predicate<T> predicate) =>
            array.Find (Array.Find (array, predicate));

        /// <summary> Swaps the values of index A and index B </summary>
        /// <typeparam name="T"> Type of the array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        /// <param name="a"> Index A. </param>
        /// <param name="b"> Index B. </param>
        public static void Swap<T> (this T[] array, int a, int b)
        {
            if (!array.HasIndex (a))
                throw Bucket.IndexOutOfRangeException (array, a);

            if (!array.HasIndex (b))
                throw Bucket.IndexOutOfRangeException (array, b);

            Bucket.Swap (ref array[a], ref array[b]);
        }

        /// <summary> Scramble the elements in this array. </summary>
        /// <typeparam name="T"> Type of the array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        public static void Shuffle<T> (this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int r = Random.Range (0, i);
                array.Swap (r, i);
            }
        }

        /// <summary> Returns a random element from array. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        /// <returns> Random element in array. </returns>
        public static T GetRandom<T> (this T[] array) =>
            array[Random.Range (0, array.Length)];

        /// <summary> Returns a sample of indexes in the array. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        /// <param name="size"> Size of the sample. </param>
        /// <param name="random"> Whether to select random elements. </param>
        /// <returns> Sample of indexes. </returns>
        public static int[] GetIndexesSample<T> (
            this T[] array,
            int size,
            bool random = false
        )
        {
            if (size < 0) throw Bucket.IndexOutOfRangeException (array, size);

            if (array.IsNullOrEmpty ()) return null;

            var sample = new int[size];

            if (random)
                for (int i = 0; i < size; i++)
                    sample[i] = Random.Range (0, array.Length);
            else
                for (int i = 0; i < size; i++)
                {
                    int index = i;
                    if (index >= array.Length) index %= array.Length;
                    sample[i] = index;
                }

            return sample;
        }

        /// <summary> Returns a sample of elements in the array. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        /// <param name="size"> Size of the sample. </param>
        /// <param name="random"> Whether to select random elements. </param>
        /// <returns> Sample of elements. </returns>
        public static T[] GetSample<T> (
            this T[] array,
            int size,
            bool random = false
        )
        {
            if (size < 0) throw Bucket.IndexOutOfRangeException (array, size);

            if (array.IsNullOrEmpty ()) return null;

            T[] sample = new T[size];

            if (random)
                for (int i = 0; i < size; i++)
                    sample[i] = array.GetRandom ();
            else
                for (int i = 0; i < size; i++)
                {
                    int index = i;
                    if (index >= array.Length) index %= array.Length;
                    sample[i] = array[index];
                }

            return sample;
        }

        /// <summary> Loops the array with the specified increment. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array Collection. </param>
        /// <param name="index"> Index to loop. </param>
        /// <param name="increment"> Increment to apply. </param>
        /// <returns> Next element in the loop. </returns>
        public static T Loop<T> (
            this T[] array,
            ref int index,
            int increment = 1
        )
        {
            if (array.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (array);

            var value = array[index];
            index = index.Loop (0, array.Length - 1, increment);
            return value;
        }
    }
}
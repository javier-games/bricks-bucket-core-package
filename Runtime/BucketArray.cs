namespace BricksBucket.Core
{
    /* ARRAY BUCKET */
	public static partial class Bucket
	{
        /// <summary> Returns a sequence of numbers in a range. </summary>
        /// <param name="from"> First number of the range. </param>
        /// <param name="to"> End number of the range. </param>
        /// <param name="random"> Whether the sequence must be shuffle. </param>
        /// <returns> Sequence of numbers. </returns>
        public static int[] GetSequence (int from, int to, bool random = false)
        {
            if (to < from) Bucket.Swap (ref from, ref to);

            int[] sequence = new int[to - from];

            for (int i = 0; i < sequence.Length; i++) sequence[i] = from + i;

            if (random) sequence.Shuffle ();

            return sequence;
        }

        /// <summary> Removes an element at the specified index. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array collection. </param>
        /// <param name="index"> Index to remove from collection. </param>
        public static void RemoveAt<T> (ref T[] array, int index)
        {
            if (array.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (array);

            if (!array.HasIndex (index))
                throw Bucket.IndexOutOfRangeException (array, index);

            for (int i = index; i < array.Length - 1; i++)
                array.Swap (i, i + 1);

            System.Array.Resize (ref array, array.Length - 1);
        }

        /// <summary> Removes the specified element. </summary>
        /// <typeparam name="T"> Type of array. </typeparam>
        /// <param name="array"> Array collection. </param>
        /// <param name="element"> Element remove from collection. </param>
        /// <returns> Whether the value is not any more in the array. </returns>
        public static void Remove<T> (ref T[] array, T element)
        {
            if (array.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (array);

            int index = array.Find (element);
            if (index >= 0)
                RemoveAt (ref array, index);
            else
                throw Bucket.ElementNotFoundException (array, element);
        }

        /// <summary> Add a new element at specified index. </summary>
        /// <typeparam name="T"> Type of collection. </typeparam>
        /// <param name="array"> Array where to add the element. </param>
        /// <param name="index"> Index where to colocate element. </param>
        /// <param name="element"> Element to add. </param>
        public static void AddAt<T> (ref T[] array, int index, T element)
        {
            if (array.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (array);

            if (!array.HasIndex (index))
                throw Bucket.IndexOutOfRangeException (array, index);

            Add (ref array, element);

            for (int i = array.Length - 1; i > index; i--)
                array.Swap (i, i - 1);
        }

        /// <summary> Add a new element at the last position. </summary>
        /// <typeparam name="T"> Type of collection. </typeparam>
        /// <param name="array"> Array where to add the element. </param>
        /// <param name="element"> Element to add. </param>
        public static void Add<T> (ref T[] array, T element)
        {
            if (array.IsNullOrEmpty ())
                throw Bucket.NullOrEmptyException (array);

            System.Array.Resize (ref array, array.Length + 1);
            array[array.Length - 1] = element;
        }
	}
}

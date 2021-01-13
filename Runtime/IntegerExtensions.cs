using UnityEngine;

using Enum = System.Enum;

namespace Monogum.BricksBucket.Core
{
	/// <!-- IntegerExtensions -->
	///
	/// <summary>
	/// Collection of extension methods for the <see href=
	/// "https://docs.microsoft.com/en-us/dotnet/api/system.int32">
	/// System.Int32 </see> structure.
	/// </summary>
	///
	/// <seealso href=
	/// "https://docs.microsoft.com/en-us/dotnet/api/system.int32">
	/// System.Int32</seealso>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public static class IntegerExtensions
	{

		/// <summary>
		/// Loops the number with the specified increment.
		/// </summary>
		/// <param name="current">Current value to loop.</param>
		/// <param name="from">Lowest value to take.</param>
		/// <param name="to">Highest value to take.</param>
		/// <param name="increment">Increment to apply.</param>
		/// <returns>Next value on the loop.</returns>
		public static int
			Loop (this ref int current, int from, int to, int increment = 1)
		{
			if (from > to) Bucket.Swap (ref from, ref to);

			int range = to - from + 1;

			if (range == 1) return from;

			if (current < from || current > to) current = from;

			if (increment.Absolute () > range) increment %= range;

			current += increment;

			if (current > to) return current - range;

			if (current < from) return range + current;

			return current;
		}

		/// <summary>
		/// Absolute Value.
		/// </summary>
		/// <param name="x">Number to absolute.</param>
		/// <returns>Absolute value.</returns>
		public static int Absolute (this int x) => Mathf.Abs (x);

		/// <summary>
		/// Evaluates if value is between min and max.
		/// </summary>
		/// <param name="x">Number to evaluate.</param>
		/// <param name="min">Min value to compare.</param>
		/// <param name="max">Max value to compare.</param>
		/// <returns>Whether value is in range.</returns>
		public static bool InRange (this int x, int min, int max) =>
			(x > min && x < max) ||
			(x >= min && x > max) ||
			(x > min && x <= max);

		/// <summary>
		/// Converts an Enum Constant to its int value.
		/// </summary>
		/// <param name="enum">Enum Constant to convert.</param>
		/// <returns>Returns int value of an Enum constant.</returns>
		public static int GetInt (this Enum @enum) =>
			(int) Enum.Parse (@enum.GetType (), @enum.ToString ());

		/// <summary>
		/// Add as layer to this numbers an int value.
		/// </summary>
		/// <param name="mask">Int value as mask.</param>
		/// <param name="layer">Int value as Layer.</param>
		public static void AddLayer (this ref int mask, Enum layer) =>
			mask.AddLayer (GetInt (layer));

		/// <summary>
		/// Add as layer to this numbers an int value.
		/// </summary>
		/// <param name="mask">Int value as mask.</param>
		/// <param name="layer">Int value as Layer.</param>
		public static void AddLayer (this ref int mask, int layer) =>
			mask |= 1 << layer;

		/// <summary>
		/// Whether an int value layer is in mask.
		/// </summary>
		/// <param name="mask">Int value as mask.</param>
		/// <param name="layer">Int value as Layer.</param>
		public static bool HasLayer (this int mask, Enum layer) =>
			mask.HasLayer (GetInt (layer));

		/// <summary>
		/// Whether an int value layer is in mask.
		/// </summary>
		/// <param name="mask">Integer value as mask.</param>
		/// <param name="layer">Integer value as Layer.</param>
		public static bool HasLayer (this int mask, int layer) =>
			(mask & (1 << layer)) > 0;

		/// <summary>
		/// Removes an int layer reference from mask.
		/// </summary>
		/// <param name="mask">Int value as mask.</param>
		/// <param name="layer">Int value as Layer.</param>
		public static void RemoveLayer (this ref int mask, Enum layer) =>
			mask.RemoveLayer (GetInt (layer));

		/// <summary>
		/// Removes an int layer reference from mask.
		/// </summary>
		/// <param name="mask">Int value as mask.</param>
		/// <param name="layer">Int value as Layer.</param>
		public static void RemoveLayer (this ref int mask, int layer)
		{
			if (mask.HasLayer (layer)) mask &= ~(1 << layer);
		}
	}
}
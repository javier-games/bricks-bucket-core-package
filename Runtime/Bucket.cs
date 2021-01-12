namespace Monogum.BricksBucket.Core
{
	/// <!-- Bucket -->
	///
	/// <summary>
	/// Collection of utilities useful during the development. 
	/// </summary>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public static partial class Bucket
	{
		/// <summary>
		/// Swaps the value between references A and B.
		/// </summary>
		/// <typeparam name="T">Type of references.</typeparam>
		/// <param name="a">Reference A.</param>
		/// <param name="b">Reference B.</param>
		public static void Swap<T> (ref T a, ref T b)
		{
			var x = a;
			a = b;
			b = x;
		}
	}
}
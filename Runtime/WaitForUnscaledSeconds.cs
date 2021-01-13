using UnityEngine;

namespace Monogum.BricksBucket.Core
{
	/// <!-- WaitForUnscaledSeconds -->
	///
	/// <summary>
	/// Suspends the coroutine execution for the given amount of seconds
	/// without using scaled time.
	/// </summary>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public class WaitForUnscaledSeconds : CustomYieldInstruction
	{
		#region Class Members

		/// <summary> Time to wait. </summary>
		private readonly float _waitTime;

		#endregion

		
		#region Accessors

		/// <summary> Whether keep waiting or stops. </summary>
		public override bool keepWaiting =>
			Time.realtimeSinceStartup < _waitTime;

		#endregion

		
		#region Constructor

		/// <summary>
		/// Suspends the coroutine execution for the given amount of
		/// seconds without using scaled time.
		/// </summary>
		/// <param name="waitTime"> Time in seconds to wait. </param>
		public WaitForUnscaledSeconds (float waitTime) =>
			_waitTime = Time.realtimeSinceStartup + waitTime;

		#endregion
	}
}
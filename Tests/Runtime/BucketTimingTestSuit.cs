using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BricksBucket.Core.Tests
{
	/// <!-- BucketTimingTestSuit -->
	///
	/// <summary>
	/// Test suit for the Bucket timing methods.
	/// </summary>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public class BucketTimingTestSuit
	{
		
		#region Fields

		/// <summary>
		/// Tolerance for al the tests.
		/// </summary>
		private const float Tolerance = 0.01f;

		#endregion

		#region Tests

		/// <summary>
		/// Tests the timing methods.
		/// </summary>
		[UnityTest]
		public IEnumerator TimingTest ()
		{
			const string testTitle = "BucketTimingTestSuit.TimingTest";
			const float time = 1000f;
			
			Bucket.StartTimeTest (testTitle, true);
			yield return new WaitForSeconds (time / 1000f);
			var elapsed = Bucket.GetElapsedMilliseconds (testTitle);
			Bucket.StopTimeTest (testTitle);
			
			Assert.IsTrue( 
				Mathf.Abs (elapsed - time) / time < time * Tolerance,
				"Wrong timing tests methods."
			);
		}
		
		/// <summary>
		/// Tests the timing methods with Pause.
		/// </summary>
		[UnityTest]
		public IEnumerator PauseTimingTest ()
		{
			const string testTitle = "BucketTimingTestSuit.PauseTimingTest";
			const float time = 1000f;
			
			Bucket.StartTimeTest (testTitle, true);
			yield return new WaitForSeconds (time / 1000f);
			Bucket.PauseTimeTest ();
			yield return new WaitForSeconds (time / 1000f);
			Bucket.StartTimeTest (testTitle, true);
			yield return new WaitForSeconds (time / 1000f);
			var elapsed = Bucket.GetElapsedMilliseconds (testTitle);
			Bucket.StopTimeTest (testTitle);
			
			Assert.IsTrue( 
				Mathf.Abs (elapsed - time * 2) / time < time * Tolerance,
				"Wrong timing pause test method."
			);
		}

		#endregion
	}
}
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace BricksBucket.Core.Tests.Runtime
{
    public class BucketCoroutinesTestSuit
    {
        #region Fields

        /// <summary>
        /// Tolerance for al the tests.
        /// </summary>
        private const float Tolerance = 0.1f;

        #endregion

        
        #region Tests
        
        /// <summary>
        /// Test for delayed action for just a frame.
        /// </summary>
        [UnityTest]
        public IEnumerator DelayActionOneFrameTest ()
        {
            var time = Time.time;
            var frame = Time.frameCount;
            yield return Bucket.DelayAction (
                action: () =>
                {
                    // Test for time.
                    var deltaTime = Time.time - time;
                    Assert.IsFalse (
                        Mathf.Abs (deltaTime) <= 0,
                        "Wrong Delay Action method, " +
                        "delta time less or equals to zero."
                    );
                    
                    // Test for frame.
                    var deltaFrame = Time.frameCount - frame;
                    Assert.IsTrue (
                        deltaFrame == 1,
                        "Wrong Delay Action method, " +
                        $"delay for {deltaFrame} frames."
                    );
                });
        }

        /// <summary>
        /// Test for delayed action with time scaled.
        /// </summary>
        [UnityTest]
        public IEnumerator DelayActionScaledTimeTest ()
        {
            // Test for the half of the time.
            yield return ScaledAsset (scale: 2f, delay: 1f);
            yield return null;

            // Test for the half of the time and zero delay.
            yield return ScaledAsset (scale: 2f, delay: 1f);
            yield return null;

            // Test for the double of time.
            yield return ScaledAsset (scale: 0.5f, delay: 1f);
            yield return null;

            // Test for the double of time and zero delay.
            yield return ScaledAsset (scale: 0.5f, delay: 1f);
            yield return null;

            // Test for one second unchanged scale.
            yield return ScaledAsset (scale: 1f, delay: 1f);
            yield return null;

            // Time Scale Reset.
            Time.timeScale = 1;
        }

        /// <summary>
        /// Test for delayed action with time unscaled.
        /// </summary>
        [UnityTest]
        public IEnumerator DelayActionUnscaledTimeTest ()
        {
            // Test for the half of the time.
            yield return UnscaledAssert (scale: 2f, delay: 1f);
            yield return null;

            // Test for the half of the time and zero delay.
            yield return UnscaledAssert (scale: 2f, delay: 0f);
            yield return null;

            // Test for the double of time.
            yield return UnscaledAssert (scale: 0.5f, delay: 1f);
            yield return null;

            // Test for the double of time.
            yield return UnscaledAssert (scale: 0.5f, delay: 0f);
            yield return null;

            // Test for one second unchanged scale.
            yield return UnscaledAssert (scale: 1f, delay: 1f);
            yield return null;

            // Time Scale Reset.
            Time.timeScale = 1;
        }

        #endregion

        
        #region Private Methods

        /// <summary>
        /// Assert coroutine for a Delayed Action Scaled Time.
        /// </summary>
        /// <param name="scale">Scale of time.</param>
        /// <param name="delay">Delay of the action.</param>
        private static IEnumerator ScaledAsset (float scale, float delay)
        {
            Time.timeScale = scale;
            var time = Time.unscaledTime;
            yield return Bucket.DelayAction (
                action: () =>
                {
                    var deltaExpected = delay / scale;
                    var delta = Time.unscaledTime - time;
                    var diff = Mathf.Abs (delta - deltaExpected);

                    Assert.IsTrue (
                        diff < Tolerance,
                        "Wrong Scaled Delay Action method.\n" +
                        $"\tDelta Expected: {deltaExpected}\n" +
                        $"\tDelta: {delta}\n" +
                        $"\tDifference: {diff}"
                    );
                },
                delay,
                scaledTime: true
            );
        }

        /// <summary>
        /// Assert coroutine for a Delayed Action Unscaled Time.
        /// </summary>
        /// <param name="scale">Scale of time.</param>
        /// <param name="delay">Delay of the action.</param>
        private static IEnumerator UnscaledAssert (float scale, float delay)
        {
            Time.timeScale = scale;
            var time = Time.time;
            yield return Bucket.DelayAction (
                action: () =>
                {
                    var deltaExpected = scale * delay;
                    var delta = Time.time - time;
                    var diff = Mathf.Abs (delta - deltaExpected);

                    Assert.IsTrue (
                        diff < Tolerance,
                        "Wrong Unscaled Delay Action method.\n" +
                        $"\tDelta Expected: {deltaExpected}\n" +
                        $"\tDelta: {delta}\n" +
                        $"\tDifference: {diff}"
                    );
                },
                delay,
                scaledTime: false
            );
        }

        #endregion
    }
}

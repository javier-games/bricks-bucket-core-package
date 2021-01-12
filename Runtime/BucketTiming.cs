using System.Diagnostics;
using System.Collections.Generic;

namespace Monogum.BricksBucket.Core
{
    /* TIME BUCKET */
    public static partial class Bucket
    {
        #region Fields

        /// <summary> History of tests. </summary>
        private static readonly Dictionary<string, TimeTest> Tests =
            new Dictionary<string, TimeTest> ();

        /// <summary> Name of the last test. </summary>
        private static string _lastStaticTest = string.Empty;

        #endregion


        #region Static Methods

        /// <summary>
        /// Starts or resume a test of time.
        /// </summary>
        /// <param name="title">Title of the test.</param>
        /// <param name="useMillis">Whether use milliseconds.</param>
        [Conditional ("DEBUG")]
        public static void StartTimeTest (string title, bool useMillis = false)
        {
            if (string.IsNullOrWhiteSpace (title))
                throw new System.Exception (
                    "An empty or null title is invalid for a time test."
                );

            if (Tests.ContainsKey (title))
                Tests[title].timer.Start ();
            else
            {
                _lastStaticTest = title;
                Tests[_lastStaticTest] =
                    new TimeTest (title, useMillis);
            }
        }

        /// <summary>
        /// Pauses the specified time time.
        /// </summary>
        /// <param name="title">Name of the test to pause.</param>
        [Conditional ("DEBUG")]
        public static void PauseTimeTest (string title = null)
        {
            title = string.IsNullOrWhiteSpace (title) ? _lastStaticTest : title;

            if (!Tests.ContainsKey (title))
            {
                // TODO: Replace Log method in TimeDiagnose.PauseTimeTest.
                UnityEngine.Debug.LogWarning(
                    "The test " + title + " has not being initialized."
                );
                return;
            }

            Tests[title].timer.Stop ();
        }

        /// <summary>
        /// Ends the specified time test.
        /// </summary>
        /// <param name="title">Name of the test.</param>
        [Conditional ("DEBUG")]
        public static void EndTimeTest (string title = null)
        {
            title = string.IsNullOrWhiteSpace (title) ? _lastStaticTest : title;

            if (!Tests.ContainsKey (title))
            {
                // TODO: Replace Log method in TimeDiagnose.PauseTimeTest.
                UnityEngine.Debug.LogWarning(
                    "The test " + title + " has not being initialized."
                );
                return;
            }

            Tests[title].End ();
            Tests.Remove (title);
            _lastStaticTest = string.Empty;
        }
        
        #endregion

        
        #region Nested Classes

        /// <!-- TimeTest -->
        ///
        /// <summary>
        /// Data Structure to Store Time Test Data.
        /// </summary>
        /// 
        /// <!-- By Javier García | @jvrgms | 2020 -->
        private readonly struct TimeTest
        {
            #region Fields

            /// <summary> Timer to accurately measure elapsed time. </summary>
            public readonly Stopwatch timer;

            /// <summary> Title of test. </summary>
            private readonly string _testTitle;

            /// <summary> Whether use milliseconds or seconds. </summary>
            private readonly bool _precise;

            #endregion

            #region Methods

            /// <summary>
            /// Creates and Initialize a new test.
            /// </summary>
            /// <param name="testTitle">Name of the test.</param>
            /// <param name="precise">Whether to use second or millis.</param>
            public TimeTest (string testTitle, bool precise = false)
            {
                _testTitle = testTitle;
                _precise = precise;
                timer = Stopwatch.StartNew ();
            }

            /// <summary>
            /// Ends the test.
            /// </summary>
            public void End ()
            {
                var ms = timer.ElapsedMilliseconds;
                var elapsedValue = _precise ? ms : ms / 1000f;
                var symbol = _precise ? "ms" : "s";
                
                // TODO: Replace Log method in TimeTest.End.
                UnityEngine.Debug.LogWarning(
                    "Time Test " + _testTitle + ":" + elapsedValue + symbol
                );
            }
            
            #endregion
        }
        
        #endregion
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace Monogum.BricksBucket.Core
{
    /* COROUTINES BUCKET */
    public static partial class Bucket
    {
        /// <summary>
        /// Invokes the action after one frame.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>IEnumerator.</returns>
        public static IEnumerator DelayAction (Action action)
        {
            yield return null;
            action?.Invoke ();
        }

        /// <summary>
        /// Invoke Action on Delay.
        /// </summary>
        /// <param name="delay"> Seconds to wait. </param>
        /// <param name="action"> Action to execute. </param>
        /// <param name="scaledTime"> Whether to use unscaled time. </param>
        /// <returns> IEnumerator. </returns>
        public static IEnumerator DelayAction (
            Action action, float delay, bool scaledTime = true
        )
        {
            if (scaledTime)
                yield return new WaitForSeconds (delay);
            else
                yield return new WaitForUnscaledSeconds (delay);

            action?.Invoke ();
        }

        /// <summary>
        /// Invokes during the specified duration the OnUpdate callback.
        /// </summary>
        /// <param name="duration">Duration of the coroutine.</param>
        /// <param name="onUpdate">Callback called each time.</param>
        /// <param name="onStart">Callback called on start.</param>
        /// <param name="onComplete">Called when coroutine finishes.</param>
        /// <param name="scaledTime">Whether use scaled time.</param>
        /// <param name="delay">Delay to wait to start process.</param>
        /// <returns></returns>
        public static IEnumerator
            UpdateForSeconds (
                float duration,
                Action<float> onUpdate,
                float delay = 0,
                Action onStart = null,
                Action onComplete = null,
                bool scaledTime = true
            )
        {
            if (delay > 0)
            {
                if (scaledTime)
                {
                    float time = 0;
                    while (time < delay)
                    {
                        yield return null;
                        time += Time.deltaTime;
                    }
                }
                else
                {
                    float time = Time.realtimeSinceStartup;
                    while (Time.realtimeSinceStartup - time < delay)
                        yield return null;
                }
            }

            onStart?.Invoke ();
            onUpdate?.Invoke (0);

            if (duration <= 0)
            {
                onUpdate?.Invoke (1);
                onComplete?.Invoke ();
                yield break;
            }

            if (scaledTime)
            {
                float time = 0;
                while (time < duration)
                {
                    float t = 1 - ((duration - time) / duration);
                    onUpdate?.Invoke (t);

                    yield return null;
                    time += Time.deltaTime;
                }
            }
            else
            {
                float time = Time.realtimeSinceStartup + duration;
                float t = 0;
                while (t < 1)
                {
                    t = 1 - (time - Time.realtimeSinceStartup) / duration;
                    onUpdate?.Invoke (t);
                    yield return null;
                }
            }

            onUpdate?.Invoke (1);
            onComplete?.Invoke ();
        }
    }
}
using UnityEngine;

namespace BricksBucket.Core.Math
{
    /// <!-- ActivationFunction -->
    /// 
    /// <summary>
    /// In artificial neural networks, the activation function of a node defines
    /// the output of that node given an input or set of inputs.
    /// </summary>
    ///
    /// <seealso href="https://en.wikipedia.org/wiki/Activation_function">
    /// Activation Functions</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public static class ActivationFunction
    {
        /// <summary>
        /// Sign function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Sign of x: 1, 0, -1 </returns>
        public static float Sign(float x) =>
            x > 0 ? 1 : x < 0 ? -1 : 0;

        /// <summary>
        /// Binary Step function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Binary Step of x. </returns>
        public static int BinaryStep (float x) =>
            x < 0 ? 0 : 1;

        /// <summary>
        /// Sigmoid function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns>  Sigmoid of x.  </returns>
        public static float Sigmoid (float x) =>
            (1 + Mathf.Exp (-x)).Invert ();
        
        // ReSharper disable CommentTypo
        /// <summary>
        /// Softsign function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Softsign of x. </returns>
        // ReSharper restore CommentTypo
        public static float SoftSign(float x) =>
            x * (1 + x.Absolute()).Invert();

        /// <summary>
        /// Inverse Square Root Unit function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <param name="alpha"></param>
        /// <returns> Inverse Square Root Unit of x. </returns>
        // ReSharper disable once IdentifierTypo
        public static float Isru(float x, float alpha) =>
            x * Mathf.Sqrt(1 + alpha + Mathf.Pow(x, 2)).Invert();

        /// <summary>
        /// Inverse Square Root Linear Unit function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <param name="alpha"></param>
        /// <returns> Inverse Square Root Linear Unit of x. </returns>
        // ReSharper disable once IdentifierTypo
        public static float Isrlu(float x, float alpha) =>
            x < 0 ? Isru(x, alpha) : x;

        /// <summary>
        /// Rectified Linear Unit function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Rectified Linear Unit  of x. </returns>
        public static float ReLu(float x) =>
            x > 0 ? x : 0;

        // ReSharper disable CommentTypo
        /// <summary>
        /// Sinc function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Sinc of x. </returns>
        // ReSharper restore CommentTypo
        // ReSharper disable once IdentifierTypo
        public static float Sinc(float x) =>
            x.Approximately(0) ? 1 : Mathf.Sin(x) * x.Invert();

        /// <summary>
        /// Gaussian function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Gaussian of x. </returns>
        public static float Gaussian(float x) =>
            Mathf.Exp(-Mathf.Pow(x, 2));

        /// <summary>
        /// Hyperbolic Tangent  function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> Hyperbolic Tangent of x </returns>
        public static float Tanh (float x) =>
            (float)System.Math.Tanh(x);

        /// <summary>
        /// Angle whose tangent function.
        /// </summary>
        /// <param name="x"> X float value </param>
        /// <returns> The angle whose tan is x. </returns>
        public static float Atan (float x) =>
            Mathf.Atan (x);

        /// <summary>
        /// Angle whose hyperbolic sin function.
        /// </summary>
        /// <param name="x"> X float value to evaluate. </param>
        /// <returns> The angle whose Sinh is x. </returns>
        public static float Asinh (float x)
        {
            float sqrt = Mathf.Sqrt (Mathf.Pow (x, 2) + 1);
            return Mathf.Log (x + sqrt, Mathf.Exp (1));
        }

    }
}

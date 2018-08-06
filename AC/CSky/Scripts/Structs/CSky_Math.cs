//////////////////////////////////////////////////
/// CSky.
/// Name: Math.
/// Description: Math for CSky.
/// 
//////////////////////////////////////////////////

//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace AC.CSky
{

    /// <summary>
    /// Math float methods.
    /// </summary>
    public struct CSky_Mathf
    {
        /// <summary>
		/// PI/2
		/// </summary>
		public const float k_HalfPI = 1.570796f;

        /// <summary>
        /// PI*2
        /// </summary>
        public const float k_TAU = 6.283185f;

        /// <summary>
        /// 1 / (4*pi)
        /// </summary>
        public const float k_PI14 = 0.079577f;

        /// <summary>
        /// 3 / (16 * pi)
        /// </summary>
        public const float k_PI316 = 0.059683f;

        /// <summary>
        /// Returns smallest value not less than a scalar or each vector component.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        public static float Saturate(float x)
        {
            return Mathf.Max(0.0f, Mathf.Min(1.0f, x));
        }

        /// <summary>
		/// Return B-V(Color Index) in Kelvin Temperature.
		/// </summary>
		/// <returns>The to kelvin.</returns>
		/// <param name="BV">B.</param>
		public static float BVToKelvin(float BV)
        {

            //See: https://en.wikipedia.org/wiki/Color_index
            return 4600f * ((1.0f / ((0.92f * BV) + 1.7f)) + (1.0f / ((0.92f * BV) + 0.62f)));
        }

        /// <summary>
		/// Return B-V(Color Index) in Kelvin Temperature.
		/// </summary>
		/// <returns>The to kelvin.</returns>
		/// <param name="BV">B.</param>
		/// <param name="correctBV">If set to <c>true</c> correct B.</param>
		public static float BVToKelvin(float BV, bool correctBV)
        {

            if (correctBV) BV *= 0.001f;

            //See: https://en.wikipedia.org/wiki/Color_index
            return 4600f * ((1.0f / ((0.92f * BV) + 1.7f)) + (1.0f / ((0.92f * BV) + 0.62f)));
        }

        /// <summary>
		/// Normalize the degress.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		public static float Rev(float x)
        {
            return x - Mathf.Floor(x / 360f) * 360f;
        }
    }

    /// <summary>
    /// Math Vector3 methods.
    /// </summary>
    public struct CSky_MathV3
    {

        /// <summary>
        /// Convert sperical coordinates to cartesian coordinates.
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static Vector3 SphericalToCartesian(float theta, float pi)
        {

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPI = Mathf.Sin(pi);
            float cosPI = Mathf.Cos(pi);

            return new Vector3()
            {
                x = sinTheta * sinPI,
                y = cosTheta,
                z = sinTheta * cosPI
            };
        }


        /// <summary>
        /// Convert sperical coordinates to cartesian coordinates.
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="pi"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector3 SphericalToCartesian(float theta, float pi, float rad)
        {

           // rad = Mathf.Max(1.0f, rad);

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPI = Mathf.Sin(pi);
            float cosPI = Mathf.Cos(pi);

            return new Vector3()
            {
                x = rad * sinTheta * sinPI,
                y = rad * cosTheta,
                z = rad * sinTheta * cosPI
            };
        }
    }
}
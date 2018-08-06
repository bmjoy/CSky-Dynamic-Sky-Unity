//////////////////////////////////////////////////////////
/// CSky.
/// Name: Orbital elements.
/// Description: All orbital elements for 
/// celestial calculations.
/// 
/// References:
/// All calculations are based on Paul Schlyter papers.
/// See: http://www.stjarnhimlen.se/comp/ppcomp.html
/// See: http://stjarnhimlen.se/comp/tutorial.html
/////////////////////////////////////////////////////////


using UnityEngine;

namespace AC.CSky
{

    public struct CSky_OrbitalElements
    {

        /// <summary>
        /// Longitude of the ascending node.
        /// </summary>
        public float N;

        /// <summary>
        /// The Inclination to the ecliptic.
        /// </summary>
        public float i;

        /// <summary>
        /// Argument of perihelion.
        /// </summary>
        public float w;

        /// <summary>
        /// Semi-major axis, or mean distance from sun
        /// </summary>
        public float a;

        /// <summary>
        /// Eccentricity
        /// </summary>
        public float e;

        /// <summary>
        /// Mean anomaly.
        /// </summary>
        public float M;

        public  CSky_OrbitalElements(float N, float i, float w, float a, float e, float M)
        {
            this.N = N;
            this.i = i;
            this.w = w;
            this.a = a;
            this.e = e;
            this.M = M;
        }
    }
}
//////////////////////////////////////////////////////////
/// CSky.
/// Name: Celestials Calculation.
/// Description: Compute planetary positions.
///
/// All calculations are based on Paul Schlyter papers.
/// See: http://www.stjarnhimlen.se/comp/ppcomp.html
/// See: http://stjarnhimlen.se/comp/tutorial.html
/////////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AC.CSky
{


    [System.Serializable]
    public class CSky_CelestialsCalculations
    {


      

        [SerializeField, Range(-90f, 90f)] public float m_Latitude;
        [SerializeField, Range(-180f, 180f)] public float m_Longitude;
        [SerializeField, Range(-12f, 12f)] public float m_UTC;

      
        [HideInInspector]
        public System.DateTime dateTime;


        // Sun distance(r).
        internal float m_SunDistance;

        // True sun longitude.
        internal float m_TrueSunLongitude;

        // Mean sun longitude.
        internal float m_MeanSunLongitude;

        // Sideral time.
        internal float m_SideralTime;

        [HideInInspector]
        public float m_LST;


        /// <summary>
        /// Return Hour - UTC.
        /// </summary>
        public float Hour_UTC_Apply { get { return (float)dateTime.TimeOfDay.TotalHours - m_UTC; } }

        /// <summary>
        /// Return latitude in radians.
        /// </summary>
        public float Latitude_Rad { get { return Mathf.Deg2Rad * m_Latitude; } }


        /// <summary>
        /// Time Scale (d).
        /// </summary>
        private float TimeScale
        {
            get
            {
                return (367 * dateTime.Year - (7 * (dateTime.Year + ((dateTime.Month + 9) / 12))) / 4 +
                    (275 * dateTime.Month) / 9 + dateTime.Day - 730530) + (float)dateTime.TimeOfDay.TotalHours / 24;
            }
        }



        /// <summary>
        /// Obliquity of the ecliptic.
        /// </summary>
        private float Oblecl { get { return Mathf.Deg2Rad * (23.4393f - 3.563e-7f * TimeScale); } }


        /// <summary>
        /// Sun orbital elements.
        /// </summary>
        public CSky_OrbitalElements SunOrbitalElements
        {

            get
            {

                CSky_OrbitalElements elements = new CSky_OrbitalElements()
                {
                    N = 0,                                      // Longitude of the ascending node.
                    i = 0,                                      // The Inclination to the ecliptic.
                    w = 282.9404f + 4.70935e-5f   * TimeScale,  // Argument of perihelion.
                    a = 0,                                      // Semi-major axis, or mean distance from sun.
                    e = 0.016709f - 1.151e-9f     * TimeScale,  // Eccentricity.
                    M = 356.0470f + 0.9856002585f * TimeScale   // Mean anomaly.
                };

                elements.M = CSky_Mathf.Rev(elements.M); // Normalize M degrees.

                return elements;
            }
        }


        /// <summary>
        /// Moon orbital elements.
        /// </summary>
        public CSky_OrbitalElements MoonOrbitalElements
        {

            get
            {

                CSky_OrbitalElements elements = new CSky_OrbitalElements()
                {
                    N = 125.1228f - 0.0529538083f * TimeScale,  // Longitude of the ascending node.
                    i = 5.1454f,                                // The Inclination to the ecliptic.
                    w = 318.0634f + 0.1643573223f * TimeScale,  // Argument of perihelion.
                    a = 60.2666f,                               // Semi-major axis, or mean distance from sun.
                    e = 0.054900f,                              // Eccentricity.
                    M = 115.3654f + 13.0649929509f * TimeScale  // Mean anomaly.
                };

                // Normalize elements.
                elements.N = CSky_Mathf.Rev(elements.N);
                elements.w = CSky_Mathf.Rev(elements.w);
                elements.M = CSky_Mathf.Rev(elements.M);

                return elements;
            }
        }


        public Vector3 GetSunCoords()
        {

            Vector3 result;

            #region |Orbital Elements|

            // Mean anomaly to radians.
            float M_Rad = SunOrbitalElements.M * Mathf.Deg2Rad;

            #endregion

            #region |Eccentric Anomaly|

            // Compute eccentric anomaly.
            float E = SunOrbitalElements.M + Mathf.Rad2Deg * SunOrbitalElements.e * Mathf.Sin(M_Rad) * (1 + SunOrbitalElements.e * Mathf.Cos(M_Rad)); // Debug.Log(E);

            // Eccentric anomaly to radians.
            float E_Rad = Mathf.Deg2Rad * E;

            #endregion

            #region |Rectangular Coordinates|

            // Rectangular coordinates of the sun in the plane of the ecliptic.
            float xv = (Mathf.Cos(E_Rad) - SunOrbitalElements.e); //Debug.Log(xv);
            float yv = (Mathf.Sin(E_Rad) * Mathf.Sqrt(1 - SunOrbitalElements.e * SunOrbitalElements.e)); // Debug.Log(yv);

            // Convert to distance and true anomaly(r = radians, v = degrees).
            float r = Mathf.Sqrt(xv * xv + yv * yv);                           //Debug.Log(r);
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);                     //Debug.Log(v);

            // Get sun distance.
            m_SunDistance = r;

            #endregion

            #region |True Longitude|

            // True sun longitude.
            float lonsun = v + SunOrbitalElements.w;

            // Rev sun longitude
            lonsun = CSky_Mathf.Rev(lonsun); // Debug.Log(lonsun);

            // True sun longitude to radians.
            float lonsun_Rad = Mathf.Deg2Rad * lonsun;

            // Set true sun longitude(radians) for use in others celestials calculations.
            m_TrueSunLongitude = lonsun_Rad;

            #endregion

            #region |Ecliptic And Equatorial Coordinates|

            // Ecliptic rectangular coordinates(radians):
            float xs = r * Mathf.Cos(lonsun_Rad);
            float ys = r * Mathf.Sin(lonsun_Rad);

            // Ecliptic rectangular coordinates rotate these to equatorial coordinates(radians).
            float oblecl_Cos = Mathf.Cos(Oblecl);
            float oblecl_Sin = Mathf.Sin(Oblecl);

            float xe = xs;
            float ye = ys * oblecl_Cos - 0.0f * oblecl_Sin;
            float ze = ys * oblecl_Sin + 0.0f * oblecl_Cos;

            #endregion

            #region |Ascension And Declination|

            // Right ascension(degrees):
            float RA = Mathf.Rad2Deg * Mathf.Atan2(ye, xe) / 15;

            // Declination(radians).
            float Decl = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            #endregion

            #region |Mean Longitude|

            // Mean sun longitude(degrees).
            float L = SunOrbitalElements.w + SunOrbitalElements.M;

            // Rev mean sun longitude.
            L = CSky_Mathf.Rev(L);

            // Set mean sun longitude for use in other celestials calculations.
            m_MeanSunLongitude = L;

            #endregion

            #region Sideral Time.

            // Sideral time(degrees).
            float GMST0 = /*(L + 180) / 15;*/  ((L / 15) + 12);

            m_SideralTime = GMST0 + Hour_UTC_Apply + m_Longitude / 15;

            m_LST = GMST0 + Hour_UTC_Apply + Mathf.Deg2Rad * m_Longitude / 15;

            m_LST *= 15;

            // Hour angle(degrees).
            float HA = (m_SideralTime - RA) * 15; //Debug.Log(HA);

            // Hour angle in radians.
            float HA_Rad = Mathf.Deg2Rad * HA;

            #endregion

            #region |Hour Angle And Declination In Rectangular Coordinates|

            // HA anf Decl in rectangular coordinates(radians).
            float Decl_Cos = Mathf.Cos(Decl);

            // X axis points to the celestial equator in the south.
            float x = Mathf.Cos(HA_Rad) * Decl_Cos;

            // Y axis points to the horizon in the west.
            float y = Mathf.Sin(HA_Rad) * Decl_Cos;

            // Z axis points to the north celestial pole.
            float z = Mathf.Sin(Decl);

            // Rotate the rectangualar coordinates system along of the Y axis(radians).
            float sinLatitude = Mathf.Sin(Latitude_Rad);
            float cosLatitude = Mathf.Cos(Latitude_Rad);

            float xhor = x * sinLatitude - z * cosLatitude;
            float yhor = y;
            float zhor = x * cosLatitude + z * sinLatitude;

            #endregion

            #region Azimuth, Altitude And Zenith[Radians].

            result.x = Mathf.Atan2(yhor, xhor) + Mathf.PI;
            result.y = Mathf.Asin(zhor);
            result.z = CSky_Mathf.k_HalfPI - result.y;

            #endregion

            //Debug.Log(result.x * Mathf.Rad2Deg);
            //Debug.Log(result.y * Mathf.Rad2Deg);

            return result; // Return coordinates in radians.

        }


        public Vector3 GetMoonCoords()
        {

            Vector3 result;


            #region |Orbital Elements|

            // Orbital elements in radians. 
            float N_Rad = Mathf.Deg2Rad * MoonOrbitalElements.N;
            float i_Rad = Mathf.Deg2Rad * MoonOrbitalElements.i;
            float M_Rad = Mathf.Deg2Rad * MoonOrbitalElements.M;

            #endregion


            #region |Eccentric Anomaly|

            // Compute eccentric anomaly.
            float E = MoonOrbitalElements.M + Mathf.Rad2Deg * MoonOrbitalElements.e * Mathf.Sin(M_Rad) * (1 + MoonOrbitalElements.e * Mathf.Cos(M_Rad));

            // Eccentric anomaly to radians.
            float E_Rad = Mathf.Deg2Rad * E;

            #endregion


            #region |Rectangular Coordinates|

            // Rectangular coordinates of the sun in the plane of the ecliptic.
            float xv = MoonOrbitalElements.a * (Mathf.Cos(E_Rad) - MoonOrbitalElements.e);                                                            // Debug.Log(xv);
            float yv = MoonOrbitalElements.a * (Mathf.Sin(E_Rad) * Mathf.Sqrt(1 - MoonOrbitalElements.e * MoonOrbitalElements.e)) * Mathf.Sin(E_Rad); // Debug.Log(yv); 

            // Convert to distance and true anomaly(r = radians, v = degrees).
            float r = Mathf.Sqrt(xv * xv + yv * yv);         // Debug.Log(r);
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);   // Debug.Log(v);

            v = CSky_Mathf.Rev(v);

            // Longitude in radians.
            float l = Mathf.Deg2Rad * (v + MoonOrbitalElements.w);

            float Cos_l = Mathf.Cos(l);
            float Sin_l = Mathf.Sin(l);
            float Cos_N_Rad = Mathf.Cos(N_Rad);
            float Sin_N_Rad = Mathf.Sin(N_Rad);
            float Cos_i_Rad = Mathf.Cos(i_Rad);

            float xeclip = r * (Cos_N_Rad * Cos_l - Sin_N_Rad * Sin_l * Cos_i_Rad);
            float yeclip = r * (Sin_N_Rad * Cos_l + Cos_N_Rad * Sin_l * Cos_i_Rad);
            float zeclip = r * (Sin_l * Mathf.Sin(i_Rad));

            #endregion


            #region Geocentric Coordinates.

            // Geocentric position for the moon and Heliocentric position for the planets.
            float lonecl = Mathf.Rad2Deg * Mathf.Atan2(yeclip, xeclip);

            // Rev lonecl
            lonecl = CSky_Mathf.Rev(lonecl);     // Debug.Log(lonecl);

            float latecl = Mathf.Rad2Deg * Mathf.Atan2(zeclip, Mathf.Sqrt(xeclip * xeclip + yeclip * yeclip));   // Debug.Log(latecl);

            // Get true sun longitude.
            // float lonSun = m_TrueSunLongitude;

            // Ecliptic longitude and latitude in radians.
            float lonecl_Rad = Mathf.Deg2Rad * lonecl;
            float latecl_Rad = Mathf.Deg2Rad * latecl;

            float nr = 1.0f;
            float xh = nr * Mathf.Cos(lonecl_Rad) * Mathf.Cos(latecl_Rad);
            float yh = nr * Mathf.Sin(lonecl_Rad) * Mathf.Cos(latecl_Rad);
            float zh = nr * Mathf.Sin(latecl_Rad);

            // Geocentric posisition.
            float xs = 0.0f;
            float ys = 0.0f;

            // Convert the geocentric position to heliocentric position.
            float xg = xh + xs;
            float yg = yh + ys;
            float zg = zh;

            #endregion

            #region |Equatorial Coordinates|

            // Convert xg, yg in equatorial coordinates.
            float oblecl_Cos = Mathf.Cos(Oblecl);
            float oblecl_Sin = Mathf.Sin(Oblecl);

            float xe = xg;
            float ye = yg * oblecl_Cos - zg * oblecl_Sin;
            float ze = yg * oblecl_Sin + zg * oblecl_Cos;

            #endregion


            #region |Ascension, Declination And Hour Angle|

            // Right ascension.
            float RA = Mathf.Rad2Deg * Mathf.Atan2(ye, xe); //Debug.Log(RA);

            // Normalize right ascension.
            RA = CSky_Mathf.Rev(RA);  //Debug.Log(RA);

            // Declination.
            float Decl = Mathf.Rad2Deg * Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            // Declination in radians.
            float Decl_Rad = Mathf.Deg2Rad * Decl;

            // Hour angle.
            float HA = ((m_SideralTime * 15) - RA); //Debug.Log(HA);

            // Rev hour angle.
            HA = CSky_Mathf.Rev(HA);     //Debug.Log(HA);

            // Hour angle in radians.
            float HA_Rad = Mathf.Deg2Rad * HA;

            #endregion

            #region |Declination in rectangular coordinates|

            // HA y Decl in rectangular coordinates.
            float Decl_Cos = Mathf.Cos(Decl_Rad);
            float xr = Mathf.Cos(HA_Rad) * Decl_Cos;
            float yr = Mathf.Sin(HA_Rad) * Decl_Cos;
            float zr = Mathf.Sin(Decl_Rad);

            // Rotate the rectangualar coordinates system along of the Y axis(radians).
            float sinLatitude = Mathf.Sin(Latitude_Rad);
            float cosLatitude = Mathf.Cos(Latitude_Rad);

            float xhor = xr * sinLatitude - zr * cosLatitude;
            float yhor = yr;
            float zhor = xr * cosLatitude + zr * sinLatitude;

            #endregion


            #region |Azimuth, Altitude And Zenith[Radians]|

            result.x = Mathf.Atan2(yhor, xhor) + Mathf.PI;
            result.y = Mathf.Asin(zhor);
            result.z = CSky_Mathf.k_HalfPI - result.y;

            #endregion

            // Return coordinates in radians.
            return result;

        }


    }
}
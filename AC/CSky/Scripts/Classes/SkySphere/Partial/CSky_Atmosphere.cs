/////////////////////////////////////////////////////
/// CSky
/// Name: Sky Sphere.
/// Description: Atmosphere.
/// 
/////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC.Utility;


namespace AC.CSky
{


	public partial class CSky_SkySphere : MonoBehaviour
	{

 	
		#region |Fields|Settings|

        // Mesh quality.
        [SerializeField] private CSky_Quality3 m_AtmosphereMeshQuality   = CSky_Quality3.High;

        // Shader calculations.
        [SerializeField] private CSky_ShaderQuality m_AtmosphereQuality  = CSky_ShaderQuality.PerVertex;

        // Atmosphere model.
        [SerializeField] private CSky_AtmosphereModel m_AtmosphereShader = CSky_AtmosphereModel.Defautl;
		
		#endregion

        #region |Fields|Rayleigh|

        // Wavelengths.
        [SerializeField, Range(0.0f, 1000f)] private float m_WavelengthR = 680f; // = 650f; // = 680f;
        [SerializeField, Range(0.0f, 1000f)] private float m_WavelengthG = 550f; // = 570f; // = 550f;
        [SerializeField, Range(0.0f, 1000f)] private float m_WavelengthB = 440f; // = 475f; // = 440f;

        // Density.
        [SerializeField, Range(0.0f, 15f)] private float m_AtmosphereThickness = 1.0f;

        // Sun.
        [SerializeField, Range(0.0f, 100f)] private float m_SunBrightness = 30f;
        [SerializeField] private Color m_SunAtmosphereTint = Color.white;
        [SerializeField, Range(0.0f, 1f)] private float m_SunIntensityFactor = 0.25f; // Only in preetham model.

        // Moon.
        [SerializeField] private bool m_MoonRayleigh = true; // Moon affects the atmosphere.

        [SerializeField, Range(0.0f, 1f)] private float m_MoonBrightness = 0.3f;
        [SerializeField] private Color m_MoonAtmosphereTint = new Color(0.1f, 0.1f, 0.1f, 1.0f);
        [SerializeField, Range(0.0f, 1f)] private float m_MoonIntensityFactor = 0.20f;

        #endregion

       

        #region |Fields|Mie|

        // Common.
        [SerializeField, Range(0.0f, 0.10f)] private float m_Mie = 0.010f;
        [SerializeField, Range(0.0f, 1f)] private float m_Turbidity = 0.1f; // Only in preetham model.

        // Sun
        [SerializeField] private Color m_SunMieColor = new Color(1.0f, 0.84f, 0.61f, 1.0f);
        [SerializeField, Range(0.0f, 0.999f)] private float m_SunMieAnisotropy = 0.75f;
        [SerializeField] private float m_SunMieScattering = 0.5f;

        // Moon.
        [SerializeField] private Color m_MoonMieColor = new Color(1.0f, 0.95f, 0.83f, 1.0f);
        [SerializeField, Range(0.0f, 0.999f)] private float m_MoonMieAnisotropy = 0.94f;
        [SerializeField] private float m_MoonMieScattering = 0.2f;

        #endregion


        #region |Fields|Other|

        // Color Correction.
        [SerializeField] private float m_AtmosphereExponent = 1.5f;

        // Height(Only in defautl model).
        [SerializeField, Range(0.0001f, 0.030f)] private float m_CameraHeight = 0.0001f;

        // Horizon.
        [SerializeField, Range(0.0f, 1f)] private float m_HorizonOffset = 0.01f;

        // Zenith (Only in preetham model).
        [SerializeField, Range(0.0f, 8.4e3f)] private float m_RayleighZenithLength = 8.4e3f;
        [SerializeField, Range(0.0f, 1.25e3f)] private float m_MieZenithLength = 1.25e3f;

        #endregion

        #region |Intensity|

        /// <summary>
        /// Return Intensity value for day.
        /// </summary>
        public float DayIntensity
        {
            get
            {
                return CSky_Mathf.Saturate(SunDirection.y + m_SunIntensityFactor);
            }
        }

        /// <summary>
        /// Return intensity value for night.
        /// </summary>
        public float NightIntensity
        {

            get
            {
                return m_MoonRayleigh ? CSky_Mathf.Saturate(MoonDirection.y + m_MoonIntensityFactor) :
                    CSky_Mathf.Saturate(-SunDirection.y + m_MoonIntensityFactor);
            }
        }


        public float MoonPhasesIntensityMultiplier
        {
            get{ return Mathf.Clamp01(Vector3.Dot(-SunDirection, MoonDirection) + 0.3f ); }
        }

        #endregion


        #region |Methods|Update|

        void UpdateAtmosphere()
        {

            // Select atmosphere model.
            switch (m_AtmosphereShader)
            {

                case CSky_AtmosphereModel.Defautl:
                {
                   ComputeDefautlAtmosphere();

                    // Moon Affects.
                    if (m_MoonRayleigh)
                        Shader.SetGlobalInt("CSky_MoonRayleigh", 1);
                    else
                        Shader.SetGlobalInt("CSky_MoonRayleigh", 0);
                }
                break;

                case CSky_AtmosphereModel.Preetham:
                {
                    ComputePreethamAtmosphere();
                }
                break;
            }


            // Set shader quality.
            if (m_AtmosphereQuality == CSky_ShaderQuality.PerPixel)
                Shader.EnableKeyword("CSky_PER_PIXEL_ATMOSPHERE");
            else
                Shader.DisableKeyword("CSky_PER_PIXEL_ATMOSPHERE");


            // Set sun mie.
            Shader.SetGlobalColor("CSky_SunMieColor", m_SunMieColor);
            Shader.SetGlobalVector("CSky_SunBetaMiePhase", BetaMiePhase(m_SunMieAnisotropy, true));
            Shader.SetGlobalFloat("CSky_SunMieScattering", m_SunMieScattering);


            // Set moon mie.
            Shader.SetGlobalColor("CSky_MoonMieColor", m_MoonMieColor);
            Shader.SetGlobalVector("CSky_MoonBetaMiePhase", BetaMiePhase(m_MoonMieAnisotropy, false));
            Shader.SetGlobalFloat("CSky_MoonMieScattering", m_MoonMieScattering * MoonPhasesIntensityMultiplier);


            // Set tint.
            Shader.SetGlobalColor("CSky_SunAtmosphereTint", m_SunAtmosphereTint);


            Shader.SetGlobalColor("CSky_MoonAtmosphereTint", m_MoonRayleigh ? m_MoonAtmosphereTint * m_MoonBrightness *  MoonPhasesIntensityMultiplier : m_MoonAtmosphereTint* m_MoonBrightness );


            // Set exponent.
            Shader.SetGlobalFloat("CSky_AtmosphereExponent", m_AtmosphereExponent);


            // Set horizon offset.
            Shader.SetGlobalFloat("CSky_HorizonOffset", m_HorizonOffset);

        }


        #endregion

        #region |Methods|Compute Atmospheric Scattering|

        void ComputeDefautlAtmosphere()
        {

            #region Calculations.

            // Camera.
            float kCameraHeight = m_CameraHeight; //  0.0001f;

            // Radius.
            float kInnerRadius  = 1.0f;
            float kInnerRadius2 = 1.0f;
            float kOuterRadius  = 1.025f;
            float kOuterRadius2 = kOuterRadius * kOuterRadius;

            // Scale.
            float kScale = (1.0f / (kOuterRadius - 1.0f));
            float kScaleDepth = 0.25f;
            float kScaleOverScaleDepth = kScale / kScaleDepth;

            // Mie.
            float kSunBrightness = m_SunBrightness * EclipseMultiplier;
            float kMie    = m_Mie;
            float kKmESun = kMie * kSunBrightness;
            float kKm4PI  = kMie * 4.0f * Mathf.PI;

            // Rayleigh.
            float kRayleigh = 0.0025f * m_AtmosphereThickness;
            float kKrESun   = kRayleigh * kSunBrightness;
            float kKr4PI    = kRayleigh * 4.0f * Mathf.PI;

            // Get reciprocal wavelength values.
            Vector3 InvWavelength = new Vector3()
            {
                x = 1.0f / Mathf.Pow(m_WavelengthR * 1e-3f, 4.0f),
                y = 1.0f / Mathf.Pow(m_WavelengthG * 1e-3f, 4.0f),
                z = 1.0f / Mathf.Pow(m_WavelengthB * 1e-3f, 4.0f)
            };

            // Fade parameters.
            /* 
            Vector3 fadeParams;
            fadeParams.x = 1;
            fadeParams.y = NightIntensity; // PRESTAR ATENCIÓN.
            fadeParams.z = 1;*/

            #endregion


            #region |Set Global Params|

            // Camera.
            Shader.SetGlobalFloat("CSky_kCameraHeight", kCameraHeight);

            // Radius.
            Shader.SetGlobalFloat("CSky_kInnerRadius", kInnerRadius);
            Shader.SetGlobalFloat("CSky_kInnerRadius2", kInnerRadius2);
            Shader.SetGlobalFloat("CSky_kOuterRadius", kOuterRadius);
            Shader.SetGlobalFloat("CSky_kOuterRadius2", kOuterRadius2);

            // Scale.
            Shader.SetGlobalFloat("CSky_kScale", kScale);
            Shader.SetGlobalFloat("CSky_kScaleDepth", kScaleDepth);
            Shader.SetGlobalFloat("CSky_kScaleOverScaleDepth", kScaleOverScaleDepth);

            // Mie.
            Shader.SetGlobalFloat("CSky_kKmESun", kKmESun);
            Shader.SetGlobalFloat("CSky_kKm4PI", kKm4PI);

            // Rayleigh.
            Shader.SetGlobalFloat("CSky_kKr4PI", kKr4PI);
            Shader.SetGlobalFloat("CSky_kKrESun", kKrESun);

            // Wavelengths.
            Shader.SetGlobalVector("CSky_InvWavelength", InvWavelength);

            // Fade params.
           // Shader.SetGlobalVector("CSky_FadeParams", fadeParams);

            #endregion

        }


        void ComputePreethamAtmosphere()
        {

            #region |Calculations|

            // Wavelengths.
            Vector3 lambda = new Vector3()
            {

                x = m_WavelengthR * 1e-9f,
                y = m_WavelengthG * 1e-9f,
                z = m_WavelengthB * 1e-9f
            };

            Vector3 wavelength = new Vector3()
            {
                x = Mathf.Pow(lambda.x, 4.0f),
                y = Mathf.Pow(lambda.y, 4.0f),
                z = Mathf.Pow(lambda.z, 4.0f)
            };

            // constant factors.
            float n  = 1.0003f;   // Index of air refraction(n);
            float N  = 2.545e25f; // Molecular density(N)
            float pn = 0.035f;    // Depolatization factor for standart air.
            float n2 = n * n;     // Molecular density exponentially squared.

            // Beta Rayleigh
            float ray = (8.0f * Mathf.Pow(Mathf.PI, 3.0f) * Mathf.Pow(n2 - 1.0f, 2.0f) * (6.0f + 3.0f * pn));
            Vector3 theta = 3.0f * N * wavelength * (6.0f - 7.0f * pn);

            Vector3 BetaRay = new Vector3()
            {
                x = (ray / theta.x) * m_AtmosphereThickness*0.5f,
                y = (ray / theta.y) * m_AtmosphereThickness*0.5f,
                z = (ray / theta.z) * m_AtmosphereThickness*0.5f
            };

            // Beta Mie.
            Vector3 k = new Vector3(0.685f, 0.679f, 0.670f);
            float c = (0.2f * m_Turbidity) * 10e-18f;
            float mieFactor = 0.434f * c * Mathf.PI;
            const float v = 4.0f;
            float mie = (m_Mie * 1e+1f); // Adjust.

            Vector3 BetaMie = new Vector3()
            {
                x = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / lambda.x, v - 2.0f) * k.x) * mie,
                y = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / lambda.y, v - 2.0f) * k.y) * mie,
                z = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / lambda.z, v - 2.0f) * k.z) * mie
            };
            #endregion


            #region |Set Global Params|

            Vector3 fadeParams;
            fadeParams.x = DayIntensity;    // Day intensity.
            fadeParams.y = NightIntensity;  // Night intensity.

            // Combined extinction factor fade(sunset/dawn color).
            fadeParams.z = CSky_Mathf.Saturate(Mathf.Clamp01(1.0f - SunDirection.y));

            Shader.SetGlobalVector("CSky_BetaRay", BetaRay);
            Shader.SetGlobalVector("CSky_BetaMie", BetaMie);


            Shader.SetGlobalFloat("CSky_SunE", (m_SunBrightness * 3.0f) * EclipseMultiplier);
            Shader.SetGlobalVector("CSky_FadeParams", fadeParams);


            Shader.SetGlobalFloat("CSky_RayleighZenithLength", m_RayleighZenithLength);
            Shader.SetGlobalFloat("CSky_MieZenithLength", m_MieZenithLength);

            #endregion

        }


        public Vector3 BetaMiePhase(float g, bool HQ)
        {

            Vector3 result;
            {
                float g2 = g * g;
                result.x = HQ ? (1.0f - g2) / (2.0f + g2) : 1.0f - g2;
                result.y = 1.0f + g2;
                result.z = 2.0f * g;
            }
            return result;
        }

        #endregion



        #region |Properties|Settings|

        public CSky_Quality3 AtmosphereMeshQuality
        {
            get { return this.m_AtmosphereMeshQuality; }
            set { this.m_AtmosphereMeshQuality = value; }
        }

        public CSky_ShaderQuality AtmosphereQuality
        {
            get { return this.m_AtmosphereQuality; }
            set { this.m_AtmosphereQuality = value; }
        }

        public CSky_AtmosphereModel AtmosphereShader
        {
            get { return this.m_AtmosphereShader; }
            set { this.m_AtmosphereShader = value; }
        }

        public float AtmosphereExponent
        {
            get { return this.m_AtmosphereExponent; }
            set { this.m_AtmosphereExponent = value; }
        }

        #endregion

        #region |Properties|Rayleigh|

        // Wavelengths.

        public float WavelengthR
        {
            get { return this.m_WavelengthR; }
            set { this.m_WavelengthR = value; }
        }

        public float WavelengthG
        {
            get { return this.m_WavelengthG; }
            set { this.m_WavelengthG = value; }
        }

        public float WavelengthB
        {
            get { return this.m_WavelengthB; }
            set { this.m_WavelengthB = value; }
        }

        // Tickness.

        public float AtmosphereThickness
        {
            get { return this.m_AtmosphereThickness; }
            set { this.m_AtmosphereThickness = value; }
        }


        public float SunBrightness
        {
            get { return this.m_SunBrightness; }
            set { this.m_SunBrightness = value; }
        }

        public Color SunAtmosphereTint
        {
            get { return this.m_SunAtmosphereTint; }
            set { this.m_SunAtmosphereTint = value; }
        }

        public bool MoonRayleigh
        {
            get { return this.m_MoonRayleigh; }
            set { this.m_MoonRayleigh = value; }
        }

        public float MoonBrightness
        {
            get { return this.m_MoonBrightness; }
            set { this.m_MoonBrightness = value; }
        }

        public Color MoonAtmosphereTint
        {
            get { return this.m_MoonAtmosphereTint; }
            set { this.m_MoonAtmosphereTint = value; }
        }

        #endregion


        #region |Mie|

        // Mie.

        public float Mie
        {
            get { return this.m_Mie; }
            set { this.m_Mie = value; }
        }

        public float Turbidity
        {
            get { return this.m_Turbidity; }
            set { this.m_Turbidity = value; }
        }

        public Color SunMieColor
        {
            get { return this.m_SunMieColor; }
            set { this.m_SunMieColor = value; }
        }

        public float SunMieAnisotropy
        {
            get { return this.m_SunMieAnisotropy; }
            set { this.m_SunMieAnisotropy = value; }
        }

        public float SunMieScattering
        {
            get { return this.m_SunMieScattering; }
            set { this.m_SunMieScattering = value; }
        }


        public Color MoonMieColor
        {
            get { return this.m_SunMieColor; }
            set { this.m_SunMieColor = value; }
        }

        public float MoonMieAnisotropy
        {
            get { return this.m_SunMieAnisotropy; }
            set { this.m_SunMieAnisotropy = value; }
        }

        public float MoonMieScattering
        {
            get { return this.m_SunMieScattering; }
            set { this.m_SunMieScattering = value; }
        }

       

        #endregion




	}
}

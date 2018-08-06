/////////////////////////////////////////////////////
/// CSky
/// Name: Sky Sphere.
/// Description: Lighting.
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

 	
		#region |Fields|Celestials|

        // Sun.
        [SerializeField] private Color m_SunLightColor     = new Color(1.0f, 0.84f, 0.61f, 1.0f);
        [SerializeField] private float m_SunLightIntensity = 1.0f;

        [AC_CurveRange(0.0f, 0.0f, 1.0f, 1.0f)]
        [SerializeField] private AnimationCurve m_SunLightIntensityMultiplier = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
        [SerializeField] private float m_SunLightThreshold = 0.20f; // Disable sun light.

        // Moon.
        [SerializeField] private Color m_MoonLightColor = new Color(0.901f, 0.951f, 1.0f, 1.0f);
        [SerializeField] private float m_MoonLightIntensity = 0.25f;

        [AC_CurveRange(0.0f, 0.0f, 1.0f, 1.0f)]
        [SerializeField] private AnimationCurve m_MoonLightIntensityMultiplier = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
        [SerializeField] private bool m_DisableMoonLightInDay = true;

        [AC_CurveRange(0.0f, 0.0f, 1.0f, 1.0f)]
        [SerializeField] private AnimationCurve m_MoonLightIntensityCorrector = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        #endregion


        #region |Fields|Environment|
 

        // UpdateInterval.

        [SerializeField] private float m_EnviroUpdateInterval = 15;


        // Ambient mode.
        [SerializeField] private UnityEngine.Rendering.AmbientMode m_AmbientMode = UnityEngine.Rendering.AmbientMode.Skybox;

        // Sun.
        [SerializeField]
        private Gradient m_AmbientSunSkyColor = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
               new GradientColorKey(new Color(0.443f, 0.552f, 0.737f, 1.0f), 0.0f),
               new GradientColorKey(new Color(0.443f, 0.552f, 0.737f, 1.0f), 0.45f),
               new GradientColorKey(new Color(0.231f, 0.290f, 0.352f, 1.0f), 0.50f),
               new GradientColorKey(new Color(0.047f, 0.094f, 0.180f, 1.0f), 0.55f),
               new GradientColorKey(new Color(0.047f, 0.094f, 0.180f, 1.0f), 1.0f)
            },

            alphaKeys = new GradientAlphaKey[]
            {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
            }
        };


        [SerializeField]
        private Gradient m_AmbientSunEquatorColor = new Gradient()
        {
           colorKeys = new GradientColorKey[]
           {
              new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.0f),
              new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.45f),
              new GradientColorKey(new Color(0.650f, 0.607f, 0.349f, 1.0f), 0.50f),
              new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 0.55f),
              new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 1.0f)
           },

           alphaKeys = new GradientAlphaKey[]
           {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
           }
        };


        [SerializeField]
        private Gradient m_AmbientSunGroundColor = new Gradient()
        {
            colorKeys = new GradientColorKey[]
           {
              new GradientColorKey(new Color(0.466f, 0.435f, 0.415f, 1.0f), 0.0f),
              new GradientColorKey(new Color(0.355f, 0.305f, 0.269f, 1.0f), 0.45f),
              new GradientColorKey(new Color(0.227f, 0.156f, 0.101f, 1.0f), 0.50f),
              new GradientColorKey(new Color(0.0f, 0.0f, 0.0f, 1.0f), 0.55f),
              new GradientColorKey(new Color(0.0f, 0.0f, 0.0f, 1.0f), 1.0f)
           },

           alphaKeys = new GradientAlphaKey[]
           {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
           }
        };


        // Moon.
        [SerializeField]
        private Gradient m_AmbientMoonSkyColor = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
               new GradientColorKey(new Color(0.443f, 0.552f, 0.737f, 1.0f), 0.0f),
               new GradientColorKey(new Color(0.443f, 0.552f, 0.737f, 1.0f), 0.45f),
               new GradientColorKey(new Color(0.231f, 0.290f, 0.352f, 1.0f), 0.50f),
               new GradientColorKey(new Color(0.047f, 0.094f, 0.180f, 1.0f), 0.55f),
               new GradientColorKey(new Color(0.047f, 0.094f, 0.180f, 1.0f), 1.0f)
            },

            alphaKeys = new GradientAlphaKey[]
            {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
            }
        };

        [SerializeField]
        private Gradient m_AmbientMoonEquatorColor = new Gradient()
        {
           colorKeys = new GradientColorKey[]
           {
              new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.0f),
              new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.45f),
              new GradientColorKey(new Color(0.650f, 0.607f, 0.349f, 1.0f), 0.50f),
              new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 0.55f),
              new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 1.0f)
           },

           alphaKeys = new GradientAlphaKey[]
           {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
           }
        };

        [SerializeField]
        private Gradient m_AmbientMoonGroundColor = new Gradient()
        {
           colorKeys = new GradientColorKey[]
           {
              new GradientColorKey(new Color(0.466f, 0.435f, 0.415f, 1.0f), 0.0f),
              new GradientColorKey(new Color(0.355f, 0.305f, 0.269f, 1.0f), 0.45f),
              new GradientColorKey(new Color(0.227f, 0.156f, 0.101f, 1.0f), 0.50f),
              new GradientColorKey(new Color(0.0f, 0.0f, 0.0f, 1.0f), 0.55f),
              new GradientColorKey(new Color(0.0f, 0.0f, 0.0f, 1.0f), 1.0f)
           },

           alphaKeys = new GradientAlphaKey[]
           {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
           }
        };

        // Intensity.
        [SerializeField, Range(0.0f, 8.0f)] private float m_AmbientIntensity = 1.0f;


        // Reflection.
        [SerializeField] private bool m_SendSkybox;

        #endregion

        #region |Fields|Fog|

        // Fog.
        [SerializeField] private bool m_EnableUnityFog = false;
        [SerializeField] private FogMode m_UnityFogMode = FogMode.ExponentialSquared;

        [SerializeField]
        private Gradient m_UnitySunFogColor = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.0f),
                new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.45f),
                new GradientColorKey(new Color(0.650f, 0.607f, 0.349f, 1.0f), 0.50f),
                new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 0.55f),
                new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 1.0f)
            },

            alphaKeys = new GradientAlphaKey[]
            {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
            }
        };


        [SerializeField]
        private Gradient m_UnityMoonFogColor = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.0f),
                new GradientColorKey(new Color(0.901f, 0.956f, 0.968f, 1.0f), 0.45f),
                new GradientColorKey(new Color(0.650f, 0.607f, 0.349f, 1.0f), 0.50f),
                new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 0.55f),
                new GradientColorKey(new Color(0.121f, 0.239f, 0.337f, 1.0f), 1.0f)
            },

            alphaKeys = new GradientAlphaKey[]
            {
               new GradientAlphaKey(1.0f, 0.0f),
               new GradientAlphaKey(1.0f, 1.0f)
            }
        };

        [SerializeField] private float m_UnityFogDensity       = 0.001f;
        [SerializeField] private float m_UnityFogStartDistance = 0.0f;
        [SerializeField] private float m_UnityFogEndDistance   = 300f;

        #endregion

        #region |States|

        public bool SunLightEnable
        {
            get { return !CheckBellowHorizon(CSky_Mathf.Saturate(SunDirection.y + 0.30f), m_SunLightThreshold); }
        }


        public bool MoonLightEnable
        {
            get
            {

                if (!SunLightEnable)
                {

                    return !CheckBellowHorizon(CSky_Mathf.Saturate(MoonDirection.y + 0.30f), m_SunLightThreshold);
                }

                return false;
            }
        }


        public bool CheckBellowHorizon(float theta, float threshold = 0.20f)
        {
            return (Mathf.Abs(theta) < threshold) ? true : false;
        }

        #endregion

        #region |Timers|

        private float m_EnvironmentRefreshTimer = 0.0f;

        #endregion


        #region |Methods|Update|

        void UpdateCelestialsLighting(Vector3 sunLightPos, Vector3 moonLightPos)
        {

            m_SunLight.transform.localPosition =  sunLightPos; 
            m_SunLight.transform.LookAt(m_Transform, Vector3.forward);

            m_SunLight.light.enabled   = SunLightEnable;
            m_SunLight.light.color     = m_SunLightColor;
            m_SunLight.light.intensity = m_SunLightIntensity * m_SunLightIntensityMultiplier.Evaluate(EvaluateTimeBySunAboveHorizon) * EclipseMultiplier; 


            m_MoonLight.transform.localPosition = moonLightPos; 
            m_MoonLight.transform.LookAt(m_Transform);
            m_MoonLight.light.color     = m_MoonLightColor;
            m_MoonLight.light.intensity = m_MoonLightIntensity * NightIntensity * m_MoonLightIntensityMultiplier.Evaluate(EvaluateTimeByMoonAboveHorizon);
            m_MoonLight.light.intensity *= MoonPhasesIntensityMultiplier;

            if(m_DisableMoonLightInDay)
            {
                m_MoonLight.light.intensity *= m_MoonLightIntensityCorrector.Evaluate(EvaluateTimeBySun);
                m_MoonLight.light.enabled = MoonLightEnable;
            }

        }


        void UpdateEnvironmentLighting()
        {


            float updateRate = 1.0f / m_EnviroUpdateInterval;

			m_EnvironmentRefreshTimer += Time.deltaTime;

		    if(m_EnvironmentRefreshTimer >= updateRate) 
		    {
			
	

                RenderSettings.ambientMode = m_AmbientMode;

                switch (m_AmbientMode)
                {

                    case UnityEngine.Rendering.AmbientMode.Skybox:
                    
                        RenderSettings.ambientIntensity = m_AmbientIntensity;

                        if(Application.isPlaying)
                            DynamicGI.UpdateEnvironment();
                    
                    break;

                    case UnityEngine.Rendering.AmbientMode.Trilight:
                    

                        RenderSettings.ambientSkyColor     = m_AmbientSunSkyColor.Evaluate(EvaluateTimeBySun);
                        RenderSettings.ambientEquatorColor = m_AmbientSunEquatorColor.Evaluate(EvaluateTimeBySun); 
                        RenderSettings.ambientGroundColor  = m_AmbientSunGroundColor.Evaluate(EvaluateTimeBySun); 

                        if(m_MoonRayleigh)
                        {
                            RenderSettings.ambientSkyColor     += m_AmbientMoonSkyColor.Evaluate(EvaluateTimeByMoon) * MoonPhasesIntensityMultiplier;
                            RenderSettings.ambientEquatorColor += m_AmbientMoonEquatorColor.Evaluate(EvaluateTimeByMoon)*MoonPhasesIntensityMultiplier; 
                            RenderSettings.ambientGroundColor  += m_AmbientMoonGroundColor.Evaluate(EvaluateTimeByMoon)*MoonPhasesIntensityMultiplier; 
                        }
                    
                    break;

                    case UnityEngine.Rendering.AmbientMode.Flat:
                    

                        RenderSettings.ambientSkyColor = m_AmbientSunSkyColor.Evaluate(EvaluateTimeBySun);

                        if(m_MoonRayleigh)
                        {

                            RenderSettings.ambientSkyColor += m_AmbientMoonSkyColor.Evaluate(EvaluateTimeByMoon)*MoonPhasesIntensityMultiplier;
                        }

                    
                    break;
                }


                if(m_SendSkybox)
                {

                    RenderSettings.skybox = m_Resources.skyboxMaterial;

                    switch (m_AtmosphereShader)
                    {
                        case CSky_AtmosphereModel.Defautl:
                            RenderSettings.skybox.shader = m_Resources.defautlSkyboxShader;
                        break;

                        case CSky_AtmosphereModel.Preetham:
                            RenderSettings.skybox.shader = m_Resources.PreethamSkyboxShader;
                        break;
                    }

                    if (m_MoonRayleigh)
                        Shader.SetGlobalColor("CSky_GroundColor", m_AmbientSunGroundColor.Evaluate(EvaluateTimeBySun) + (m_AmbientMoonGroundColor.Evaluate(EvaluateTimeByMoon)*MoonPhasesIntensityMultiplier));
                    else
                        Shader.SetGlobalColor("CSky_GroundColor", m_AmbientSunGroundColor.Evaluate(EvaluateTimeBySun));
                }

            }

        }


        void UpdateUnityFog()
        {

            RenderSettings.fog = m_EnableUnityFog;

            if (m_EnableUnityFog)
            {
                RenderSettings.fogMode = m_UnityFogMode;

                RenderSettings.fogColor = m_UnitySunFogColor.Evaluate(EvaluateTimeBySun);

                if (m_MoonRayleigh)
                    RenderSettings.fogColor += m_UnityMoonFogColor.Evaluate(EvaluateTimeByMoon) * MoonPhasesIntensityMultiplier;

                if (m_UnityFogMode == FogMode.Linear)
                {
                    RenderSettings.fogStartDistance = m_UnityFogStartDistance;
                    RenderSettings.fogEndDistance = m_UnityFogEndDistance;
                }
                else
                {
                    RenderSettings.fogDensity = m_UnityFogDensity;
                }
            }

                    

        }

        #endregion


        #region |Properties|Celestials|

        public Color SunLightColor
        {
            get { return this.m_SunLightColor; }
            set { this.m_SunLightColor = value; }
        }

        public float SunLightIntensity
        {
            get { return this.m_SunLightIntensity; }
            set { this.m_SunLightIntensity = value; }
        }

        public float SunLightThreshold
        {
            get { return this.m_SunLightThreshold; }
            set { this.m_SunLightThreshold = value; }
        }

        public Color MoonLightColor
        {
            get { return this.m_MoonLightColor; }
            set { this.m_MoonLightColor = value; }
        }

        public float MoonLightIntensity
        {
            get { return this.m_MoonLightIntensity; }
            set { this.m_MoonLightIntensity = value; }
        }


        #endregion

        #region |Properties|Environment|

        // Ambient.

        public UnityEngine.Rendering.AmbientMode AmbientMode
        {
            get { return this.m_AmbientMode; }
            set { this.m_AmbientMode = value; }
        }

        public Gradient AmbientSunSkyColor
        {
            get { return this.m_AmbientSunSkyColor; }
            set { this.m_AmbientSunSkyColor = value; }
        }

        public Gradient AmbientSunEquatorColor
        {
            get { return this.m_AmbientSunEquatorColor; }
            set { this.m_AmbientSunEquatorColor = value; }
        }

        public Gradient AmbientSunGroundColor
        {
            get { return this.m_AmbientSunGroundColor; }
            set { this.m_AmbientSunGroundColor = value; }
        }


        public Gradient AmbientMoonSkyColor
        {
            get { return this.m_AmbientMoonSkyColor; }
            set { this.m_AmbientMoonSkyColor = value; }
        }

        public Gradient AmbientMoonEquatorColor
        {
            get { return this.m_AmbientMoonEquatorColor; }
            set { this.m_AmbientMoonEquatorColor = value; }
        }

        public Gradient AmbientMoonGroundColor
        {
            get { return this.m_AmbientMoonGroundColor; }
            set { this.m_AmbientMoonGroundColor = value; }
        }



        public float AmbientIntensity
        {
            get { return this.m_AmbientIntensity; }
            set { this.m_AmbientIntensity = value; }
        }

        // Reflection.

        public bool SendSkybox
        {
            get { return this.m_SendSkybox; }
            set { this.m_SendSkybox = value; }
        }

        #endregion

        #region |Properties|Unity Fog|

        public bool EnableUnityFog
        {
            get { return this.m_EnableUnityFog; }
            set { this.m_EnableUnityFog = value; }
        }

        public FogMode UnityFogMode
        {
            get { return this.m_UnityFogMode; }
            set { this.m_UnityFogMode = value; }
        }

        public Gradient UnityFogColor
        {
            get { return this.m_UnitySunFogColor; }
            set { this.m_UnitySunFogColor = value; }
        }

        public float UnityFogDensity
        {
            get { return this.m_UnityFogDensity; }
            set { this.m_UnityFogDensity = value; }
        }

        public float UnityFogStartDistance
        {
            get { return this.m_UnityFogStartDistance; }
            set { this.m_UnityFogStartDistance = value; }
        }

        public float UnityFogEndDistance
        {
            get { return this.m_UnityFogEndDistance; }
            set { this.m_UnityFogEndDistance = value; }
        }

        #endregion

	}
}

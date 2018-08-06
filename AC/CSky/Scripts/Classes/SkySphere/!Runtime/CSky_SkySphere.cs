/////////////////////////////////////////////////////
/// CSky
/// Name: Sky Sphere.
/// Description: Sky sphere main.
/// 
/////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC.Utility;

namespace AC.CSky
{


	[ExecuteInEditMode]
	public partial class CSky_SkySphere : MonoBehaviour
	{


		#region |Fields|General Intensity Affected|

		[SerializeField] private float m_SolarEclipseThreshold = 0.2f;

		#endregion


		#region |Fields|Color Correction|

        [SerializeField] private bool m_HDR = false; // Enable HDR.
        [SerializeField] private float m_Exposure = 1.3f;  // Exposure.

		#endregion




		#region |General Intensity Affected|

 		/// <summary>
        /// 
        /// </summary>
        public float EclipseMultiplier
        {
            get { return Mathf.Lerp(0.1f, 1f, DistanceBetweenSunAndMoon / m_SolarEclipseThreshold); }
        }

		#endregion




        #region |Evaluate Time|

        /// <summary>
        /// Evaluate time for curves and gradients in full sun cycle.
        /// </summary>
        public float EvaluateTimeBySun{ get { return (1.0f - SunDirection.y) * 0.5f; } }


        /// <summary>
        /// Evaluate time for curves and gradients in above horizon sun cycle.
        /// </summary>
        public float EvaluateTimeBySunAboveHorizon { get { return (1.0f - SunDirection.y); } }


        /// <summary>
        /// Evaluate time for curves and gradient in bellow horizon sun cycle.
        /// </summary>
        public float EvaluateTimeBySunBellowHorizon { get { return (1.0f - (-SunDirection.y)); } }


        /// <summary>
        /// Evaluate time for curves and gradients in full moon cycle.
        /// </summary>
        public float EvaluateTimeByMoon { get { return (1.0f - MoonDirection.y) * 0.5f; } }

        /// <summary>
        /// Evaluate time for curves and gradient in above horizon moon cycle.
        /// </summary>
        public float EvaluateTimeByMoonAboveHorizon { get { return (1.0f - MoonDirection.y); } }

        /// <summary>
        /// Evaluate time for curves and gradient in bellow horizon moon cycle.
        /// </summary>
        public float EvaluateTimeByMoonBellowHorizon { get { return (1.0f - (-MoonDirection.y)); } }

        #endregion


		#region |Methods|Init|

		void Awake()
		{
			m_Transform = this.transform; // Cache transform.
		}


		void Start()
		{

			if(!CheckComponents)
        	{

                // Build the sky sphere.
            	BuildSkySphere();              

                // Initialize dynamic GI.
                if(m_AmbientMode == UnityEngine.Rendering.AmbientMode.Skybox)
            	    DynamicGI.UpdateEnvironment(); 
        	}

			InitCamera();             // Initialize camera.
			UpdateScaleAndPosition(); // Initialize scale and position.

		}


		void InitCamera()
        {

            if(m_Camera != null)
            {

				// Set clear flags mode.
                m_Camera.clearFlags = CameraClearFlags.Color;

				// Set background color.
                m_Camera.backgroundColor = Color.black;
            }

        }

		#endregion

		#region |Methods|Update|

		void LateUpdate()
		{

			if (!CheckResources || !CheckComponents) return; // Check resources and components.


			// Update sky sphere scale and position.
            UpdateScaleAndPosition();

			// Set resources to components.
			SetResources();


			// Set sky sphere matrices to shaders.
            Shader.SetGlobalMatrix("CSky_WorldToLocal", m_Transform.worldToLocalMatrix);
            Shader.SetGlobalMatrix("CSky_LocalToWorld", m_Transform.localToWorldMatrix);


			// Update Celestials.
			Vector3 sunPos  = CSky_MathV3.SphericalToCartesian(m_SunTheta, m_SunPI);
			Vector3 moonPos = CSky_MathV3.SphericalToCartesian(m_MoonTheta, m_MoonPI);
			UpdateCelestials(sunPos, moonPos);


			// Update Atmosphere.
			UpdateAtmosphere();


			// Update Color Correction.
            if (!m_HDR)
                Shader.DisableKeyword("CSky_HDR");
            else
                Shader.EnableKeyword("CSky_HDR");

            Shader.SetGlobalFloat("CSky_Exposure", m_Exposure);

			// Update Lighting.

            UpdateCelestialsLighting(sunPos, moonPos);
            UpdateEnvironmentLighting();
            UpdateUnityFog();

		}


 		protected void UpdateScaleAndPosition()
        {

            if (!m_Camera) return; // Check if camera is assigned.

            // The scale of the sky sphere adjusts to the farclip of the camera.
            m_Transform.localScale = Vector3.one * (m_Camera.farClipPlane - 100f);

            // Update position.
            m_Transform.position = m_Camera.transform.position + m_Camera.transform.rotation * Vector3.one;

        }


		#endregion

        #region |Properties|Color Correction|

        // Color correction.
        public bool HDR
        {
            get { return this.m_HDR; }
            set { this.m_HDR = value; }
        }

        public float Exposure
        {
            get { return this.m_Exposure; }
            set { this.m_Exposure = value; }
        }

        #endregion

        #region |Properties|Eclipses|

        public float SolarEclipseThreshold
        {
            get { return this.m_SolarEclipseThreshold; }
            set { this.m_SolarEclipseThreshold = value; }
        }

        #endregion

	}
}

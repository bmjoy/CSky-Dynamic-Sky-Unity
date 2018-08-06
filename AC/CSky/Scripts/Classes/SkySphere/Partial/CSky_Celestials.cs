/////////////////////////////////////////////////////
/// CSky
/// Name: Sky Sphere.
/// Description: Celestials.
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

 	
		#region |Fields|Background|

		// Mesh quality.
  		[SerializeField] private CSky_Quality3 m_BackgroundMeshQuality = CSky_Quality3.Low;

        // Color.
        [SerializeField] private Color m_BackgroundColor     = Color.white;
        [SerializeField] private float m_BackgroundIntensity = 0.05f;

		// Curve multiplier.
        [AC_CurveRange(0.0f, 0.0f, 1.0f, 1.0f)]
        [SerializeField] private AnimationCurve m_BackgroundIntensityMultiplier = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

		#endregion

		#region |Fields|Stars Field|

		// Mesh quality.
		[SerializeField] private CSky_Quality3 m_StarsFieldMeshQuality = CSky_Quality3.High; 

        // Color.
        [SerializeField] private Color m_StarsFieldColor     = Color.white;
        [SerializeField] private float m_StarsFieldIntensity = 0.3f;

		// Curve multiplier.
        [AC_CurveRange(0.0f, 0.0f, 1.0f, 1.0f)]
        [SerializeField] private AnimationCurve m_StarsFieldIntensityMultiplier = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        // Scintillation.
        [SerializeField, Range(0.0f, 1.0f)] private float m_StarsFieldScintillation = 0.7f;
        [SerializeField] private float m_StarsFieldScintillationSpeed = 0.5f;

		#endregion

		#region |Fields|Sun|

		// Position.
        [SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_SunPI    = 0.0f;
        [SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_SunTheta = 0.70f;

		// Size.
 		[SerializeField, Range(0.0f, 1.0f)] private float m_SunSize = 0.07f;

		// Color.
        [SerializeField] private Color m_SunColor = new Color(1.0f, 0.798f, 0.316f, 1.0f);
        [SerializeField] private float m_SunIntensity = 1.0f;

		#endregion

		#region |Fields|Moon|

		// Mesh quality.
        [SerializeField] private CSky_Quality3 m_MoonMeshQuality = CSky_Quality3.Low;

        // Position.
        [SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_MoonPI    = 0.0f;
        [SerializeField, Range(-Mathf.PI, Mathf.PI)] private float m_MoonTheta = -1.19f;

		// Size.
  		[SerializeField, Range(0.0f, 1.0f)] private float m_MoonSize = 0.02f;

        // Color.
        [SerializeField] private Color m_MoonColor = new Color(1.0f, 0.872f, 0.661f, 1.0f);
        [SerializeField] private float m_MoonIntensity = 1.0f;

		#endregion

		#region |Direction|

        /// <summary>
        /// Return sun direction.
        /// </summary>
        public Vector3 SunDirection { get { return -m_Sun.transform.forward; } }

        /// <summary>
        /// Return moon direction.
        /// </summary>
        public Vector3 MoonDirection { get { return -m_Moon.transform.forward; } }

        /// <summary>
        /// Return distance between the sun and the moon.
        /// </summary>
        public float DistanceBetweenSunAndMoon
        {
            get { return Vector3.Distance(SunDirection, MoonDirection); }
        }

		#endregion

		#region |Methods|Update|

		void UpdateCelestials(Vector3 sunPos, Vector3 moonPos)
		{

 			// Background.
            if(m_Background.meshRenderer.enabled)
            {

                m_Background.meshRenderer.sharedMaterial.SetColor("_Color", m_BackgroundColor);
                m_Background.meshRenderer.sharedMaterial.SetFloat("_Intensity", m_BackgroundIntensity * m_BackgroundIntensityMultiplier.Evaluate(EvaluateTimeBySun));
            }

            // Stars field.
            if(m_StarsField.meshRenderer.enabled)
            {
                m_StarsField.meshRenderer.sharedMaterial.SetColor("_Color", m_StarsFieldColor);
                m_StarsField.meshRenderer.sharedMaterial.SetFloat("_Intensity", m_StarsFieldIntensity * m_StarsFieldIntensityMultiplier.Evaluate(EvaluateTimeBySun));
                m_StarsField.meshRenderer.sharedMaterial.SetFloat("_Scintillation", m_StarsFieldScintillation);
                m_StarsField.meshRenderer.sharedMaterial.SetFloat("_ScintillationSpeed", m_StarsFieldScintillationSpeed);
            }

            // Sun.
            if(m_Sun.meshRenderer.enabled)
            {

                m_Sun.transform.localScale    = Vector3.one * m_SunSize;
                m_Sun.transform.localPosition =  sunPos; //CSky_MathV3.SphericalToCartesian(m_SunTheta, m_SunPI);
                m_Sun.transform.LookAt(m_Transform, Vector3.forward);

                Shader.SetGlobalVector("CSky_SunDirection", SunDirection);
                m_Sun.meshRenderer.sharedMaterial.SetColor("_Color", m_SunColor);
                m_Sun.meshRenderer.sharedMaterial.SetFloat("_Intensity", m_SunIntensity);
            }

            // Moon.
            if(m_Moon.meshRenderer.enabled)
            {
                m_Moon.transform.localScale    = Vector3.one * m_MoonSize;
                m_Moon.transform.localPosition = moonPos; //CSky_MathV3.SphericalToCartesian(m_MoonTheta, m_MoonPI);
                m_Moon.transform.LookAt(m_Transform);

                Shader.SetGlobalVector("CSky_MoonDirection", MoonDirection);
                m_Moon.meshRenderer.sharedMaterial.SetColor("_Color", m_MoonColor);
                m_Moon.meshRenderer.sharedMaterial.SetFloat("_Intensity", m_MoonIntensity);
            }

		}

		#endregion

        #region |Properties|Outer Space|

        public CSky_Quality3 BackgroundMeshQuality
        {
            get { return this.m_BackgroundMeshQuality; }
            set { this.m_BackgroundMeshQuality = value; }
        }

        public Color BackgroundColor
        {
            get { return this.m_BackgroundColor; }
            set { this.m_BackgroundColor = value; }
        }

        public float NebulaIntensity
        {
            get { return this.m_BackgroundIntensity; }
            set { this.m_BackgroundIntensity = value; }
        }


        public CSky_Quality3 StarsFieldMeshQuality
        {
            get { return this.m_StarsFieldMeshQuality; }
            set { this.m_StarsFieldMeshQuality = value; }
        }


        public Color StarsFieldColor
        {
            get { return this.m_StarsFieldColor; }
            set { this.m_StarsFieldColor = value; }
        }

        public float StarsFieldIntensity
        {
            get { return this.m_StarsFieldIntensity; }
            set { this.m_StarsFieldIntensity = value; }
        }

        public float StarsFieldScintillation
        {
            get { return this.m_StarsFieldScintillation; }
            set { this.m_StarsFieldScintillation = value; }
        }

        public float StarsFieldScintillationSpeed
        {
            get { return this.m_StarsFieldScintillationSpeed; }
            set { this.m_StarsFieldScintillationSpeed = value; }
        }


        public CSky_CelestialObject Background
        {
            get { return this.m_Background; }
            set
            {
                m_Background.transform.localRotation = value.transform.localRotation;
                m_Background.transform.rotation = value.transform.rotation;
            }
        }

        public CSky_CelestialObject StarsField
        {
            get { return this.m_StarsField; }
            set
            {
                m_StarsField.transform.localRotation = value.transform.localRotation;
                m_StarsField.transform.rotation = value.transform.rotation;
            }
        }


        #endregion

        #region |Properties|Sun|

        public float SunPI
        {
            get { return this.m_SunPI; }
            set { this.m_SunPI = value; }
        }

        public float SunTheta
        {

            get { return this.m_SunTheta; }
            set { this.m_SunTheta = value; }
        }

        public Color SunColor
        {
            get { return this.m_SunColor; }
            set { this.m_SunColor = value; }
        }

        public float SunSize
        {
            get { return this.m_SunSize; }
            set { this.m_SunSize = value; }
        }

        public float SunIntensity
        {
            get { return this.m_SunIntensity; }
            set { this.m_SunIntensity = value; }
        }

        #endregion

        #region |Properties|Moon|

        public CSky_Quality3 MoonMeshQuality
        {
            get { return this.m_MoonMeshQuality; }
            set { this.m_MoonMeshQuality = value; }
        }

        public float MoonPI
        {
            get { return this.m_MoonPI; }
            set { this.m_MoonPI = value; }
        }

        public float MoonTheta
        {

            get { return this.m_MoonTheta; }
            set { this.m_MoonTheta = value; }
        }

        public Color MoonColor
        {
            get { return this.m_MoonColor; }
            set { this.m_MoonColor = value; }
        }

        public float MoonSize
        {
            get { return this.m_MoonSize; }
            set { this.m_MoonSize = value; }
        }

        public float MoonIntensity
        {
            get { return this.m_MoonIntensity; }
            set { this.m_MoonIntensity = value; }
        }

        #endregion


	}
}

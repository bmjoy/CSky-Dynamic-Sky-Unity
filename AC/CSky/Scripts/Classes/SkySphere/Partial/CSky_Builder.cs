/////////////////////////////////////////////////////
/// CSky
/// Name: Sky Sphere.
/// Description: Sky sphere builder.
/// 
/////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AC.CSky
{


	public partial class CSky_SkySphere : MonoBehaviour
	{


 		#region |Resources and Components|

 		
		// Data resources.
        [SerializeField] private CSky_SkySphereResources m_Resources = null; 


		// Components.
        protected Transform m_Transform = null;
        [SerializeField] private Camera m_Camera = null;


		// Celestials Bodies.
        [SerializeField] private CSky_CelestialObject m_Background = new CSky_CelestialObject(); 
        [SerializeField] private CSky_CelestialObject m_StarsField = new CSky_CelestialObject();
        [SerializeField] private CSky_CelestialObject m_Sun        = new CSky_CelestialObject();  
        [SerializeField] private CSky_CelestialLight  m_SunLight   = new CSky_CelestialLight();   
        [SerializeField] private CSky_CelestialObject m_Moon       = new CSky_CelestialObject();  
        [SerializeField] private CSky_CelestialLight  m_MoonLight  = new CSky_CelestialLight(); 
        [SerializeField] private CSky_CelestialObject m_Atmosphere = new CSky_CelestialObject();


 		public bool CheckResources
        {
            get
            {
                if (m_Resources == null) return false;
          
                return true;
            }
        }

  		public bool CheckComponents
        {
            get
            {

                if (!m_Background.CheckComponents) return false;
                if (!m_StarsField.CheckComponents) return false;
                if (!m_Sun.CheckComponents)        return false;
                if (!m_SunLight.CheckComponents)   return false;
                if (!m_Moon.CheckComponents)       return false;
                if (!m_MoonLight.CheckComponents)  return false;
                if (!m_Atmosphere.CheckComponents) return false;
                return true;
            }
        }


		#endregion

		#region Build

 		public void BuildSkySphere()
        {

            if (!CheckResources) return;

           // m_Transform = this.transform; // Get transform.

            // Build background.
            m_Background.Build(this.name, "Background");
            m_Background.InitTransform(this.transform);
            m_Background.GetComponents();

            // Build stars field.
            m_StarsField.Build(this.name, "Stars Field");
            m_StarsField.InitTransform(this.transform);
            m_StarsField.GetComponents();

            // Build sun.
            m_Sun.Build(this.name, "Sun");
            m_Sun.InitTransform(this.transform);
            m_Sun.GetComponents();

            // Build sun light.
            m_SunLight.Build(this.name, this.name, "Sun Light");
            m_SunLight.InitTransform(this.transform);
            m_SunLight.GetComponents();

            // Build moon
            m_Moon.Build(this.name, "Moon");
            m_Moon.InitTransform(this.transform);
            m_Moon.GetComponents();

            // Build moon light.
            m_MoonLight.Build(this.name, this.name, "Moon Light");
            m_MoonLight.InitTransform(this.transform);
            m_MoonLight.GetComponents();

            // Build atmosphere.
            m_Atmosphere.Build(this.name, "Atmosphere");
            m_Atmosphere.InitTransform(this.transform);
            m_Atmosphere.GetComponents();

            SetResources();
          
        }


        void SetResources()
        {

            if (!CheckResources) return;


            // Background.
            //if (m_Background.gameObject.activeSelf)
            {
                switch (m_BackgroundMeshQuality)
                {
                    case CSky_Quality3.High:

                        m_Background.meshFilter.mesh = m_Resources.sphereLOD0;

                    break;

                    case CSky_Quality3.Medium:

                        m_Background.meshFilter.mesh = m_Resources.sphereLOD1;

                    break;

                    case CSky_Quality3.Low:

                        m_Background.meshFilter.mesh = m_Resources.sphereLOD2;

                    break;
                }
                m_Background.meshRenderer.sharedMaterial = m_Resources.backgroundMaterial;
                m_Background.meshRenderer.sharedMaterial.shader = m_Resources.backgroundShader;
            }


            // Stars.
            //if (m_StarsField.gameObject.activeSelf)
            {


                switch (m_StarsFieldMeshQuality)
                {
                    case CSky_Quality3.High:

                        m_StarsField.meshFilter.mesh = m_Resources.StarsLOD0;

                    break;

                    case CSky_Quality3.Medium:

                        m_StarsField.meshFilter.mesh = m_Resources.StarsLOD1;

                    break;

                    case CSky_Quality3.Low:

                        m_StarsField.meshFilter.mesh = m_Resources.StarsLOD2;

                    break;
                }

                m_StarsField.meshRenderer.sharedMaterial        = m_Resources.starsFieldMaterial;
                m_StarsField.meshRenderer.sharedMaterial.shader = m_Resources.starsFieldShader;
            }

            // Sun.
            //if (m_Sun.gameObject.activeSelf)
            {
                m_Sun.meshFilter.mesh                    = m_Resources.quadMesh;
                m_Sun.meshRenderer.sharedMaterial        = m_Resources.sunMaterial;
                m_Sun.meshRenderer.sharedMaterial.shader = m_Resources.sunShader;
            }

            // Moon.
            //if (m_Moon.gameObject.activeSelf)
            {
                switch (m_MoonMeshQuality)
                {
                    case CSky_Quality3.High:

                        m_Moon.meshFilter.mesh = m_Resources.sphereLOD0;

                    break;

                    case CSky_Quality3.Medium:

                        m_Moon.meshFilter.mesh = m_Resources.sphereLOD1;

                    break;

                    case CSky_Quality3.Low:

                        m_Moon.meshFilter.mesh = m_Resources.sphereLOD2;

                    break;
                }
                m_Moon.meshRenderer.sharedMaterial        = m_Resources.moonMaterial;
                m_Moon.meshRenderer.sharedMaterial.shader = m_Resources.moonShader;
            }

            // Atmosphere.
            //if (m_Atmosphere.gameObject.activeSelf)
            {
                switch (m_AtmosphereMeshQuality)
                {
                    case CSky_Quality3.High:

                        m_Atmosphere.meshFilter.mesh = m_Resources.sphereLOD0;

                    break;

                    case CSky_Quality3.Medium:

                        m_Atmosphere.meshFilter.mesh = m_Resources.sphereLOD1;

                    break;

                    case CSky_Quality3.Low:

                        m_Atmosphere.meshFilter.mesh = m_Resources.sphereLOD2;

                    break;
                }

                m_Atmosphere.meshRenderer.sharedMaterial = m_Resources.atmosphereMaterial;

                switch (m_AtmosphereShader)
                {
                    case CSky_AtmosphereModel.Defautl:

                        m_Atmosphere.meshRenderer.sharedMaterial.shader = m_Resources.defautlAtmosphereShader;

                    break;

                    case CSky_AtmosphereModel.Preetham:

                        m_Atmosphere.meshRenderer.sharedMaterial.shader = m_Resources.PreethamAtmosphereShader;

                    break;
                }
            }
        }


		#endregion


	}
}

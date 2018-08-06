using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC.Utility;

namespace AC.CSky
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(CSky_SkySphere))]
    public class CSky_SimpleClouds : MonoBehaviour
    {



        [SerializeField] private Material m_CloudsMaterial = null;

        [SerializeField] private Mesh m_DomeMesh;
        [SerializeField] private Shader m_Shader;
        [SerializeField] private CSky_SkySphere m_SkySphere = null;
        [SerializeField] private CSky_CelestialObject m_CloudsDome = new CSky_CelestialObject();


        
        [SerializeField] private Gradient m_Color;

        [AC_CurveRange(0,0,1,5)]
        [SerializeField] private AnimationCurve m_Intensity;



        [SerializeField] private Gradient m_MoonColor;

        [AC_CurveRange(0,0, 1, 5)]
        [SerializeField] private AnimationCurve m_MoonIntensity;





        private Transform m_Transform = null;





        bool CheckResources
        {

            get

            {

                if (m_CloudsMaterial == null) return false;
                if (m_DomeMesh == null) return false;
                if (m_Shader == null) return false;
                if (m_SkySphere == null) return false;


                return true;
            }
        }



        bool CheckComponents
        {

            get
            {

                if (m_Transform == null) return false;
                if (!m_CloudsDome.CheckComponents) return false;
                if (m_SkySphere == null) return false;

                return true;
            }
        }


        public void BuildCloudsDome()
        {

            if (!CheckResources) return;

            m_Transform = this.transform;

            m_CloudsDome.Build(this.name, "Simple Clouds ");
            m_CloudsDome.InitTransform(this.transform);
            m_CloudsDome.GetComponents();

            SetResources();

        }


        void SetResources()
        {

            if (!CheckResources) return;


            m_CloudsDome.meshFilter.mesh = m_DomeMesh;

            m_CloudsDome.meshRenderer.sharedMaterial = m_CloudsMaterial;
            m_CloudsDome.meshRenderer.sharedMaterial.shader = m_Shader;


        }


        public bool build = false;


     

        private void Start()
        {

            if (!CheckComponents)
            {
                BuildCloudsDome();

            }
            m_SkySphere = GetComponent<CSky_SkySphere>();
        }


        private void LateUpdate()
        {


            if(build)
            {

                BuildCloudsDome();
                build = false;
            }


            if (!CheckResources || !CheckComponents) return;


            SetResources(); // Set skysphere resources.


            Color col = m_Color.Evaluate(m_SkySphere.EvaluateTimeBySun);

            if(m_SkySphere.MoonRayleigh)
            {
                col += m_MoonColor.Evaluate(m_SkySphere.EvaluateTimeByMoon);
            }


            float intensity = m_Intensity.Evaluate(m_SkySphere.EvaluateTimeBySun);
            if (m_SkySphere.MoonRayleigh)
            {
                intensity += m_MoonIntensity.Evaluate(m_SkySphere.EvaluateTimeByMoon);
            }

            m_CloudsMaterial.SetColor("_Color", col);
            m_CloudsMaterial.SetFloat("_Intensity", intensity);
        }
    }
}

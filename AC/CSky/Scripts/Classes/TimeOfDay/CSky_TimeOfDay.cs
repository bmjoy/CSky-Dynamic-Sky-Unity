using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;


namespace AC.CSky
{


    [ExecuteInEditMode]

    [RequireComponent(typeof(CSky_SkySphere))]
    public class CSky_TimeOfDay : CSky_DateTime
    {

        [SerializeField]
        private CSky_SkySphere m_SkySphere = null;


        public CSky_CelestialsCalculations celestialsCalculations = new CSky_CelestialsCalculations();


        public override bool IsDay
        {
            get
            {
                return m_SkySphere.SunLightEnable;
            }
        }



        protected override void Awake()
        {
            base.Awake();


            m_SkySphere = GetComponent<CSky_SkySphere>();
        }


        protected override void Update()
        {
            base.Update();

            celestialsCalculations.dateTime = DateTime;

            Vector3 sunCoords = celestialsCalculations.GetSunCoords();

            m_SkySphere.SunPI = sunCoords.x - Mathf.PI;
            m_SkySphere.SunTheta = sunCoords.z;


            Vector3 moonCoords = celestialsCalculations.GetMoonCoords();

            m_SkySphere.MoonPI = moonCoords.x - Mathf.PI;
            m_SkySphere.MoonTheta = moonCoords.z;


            Quaternion OuterSpaceBackgroundRotation = Quaternion.Euler(270 + celestialsCalculations.m_Latitude, 0, 0) * Quaternion.Euler(0, celestialsCalculations.m_Longitude, 0) * Quaternion.Euler(0, celestialsCalculations.m_LST, 0);

            m_SkySphere.Background.transform.localRotation = OuterSpaceBackgroundRotation;
            m_SkySphere.StarsField.transform.localRotation = OuterSpaceBackgroundRotation;
        }

    }
}

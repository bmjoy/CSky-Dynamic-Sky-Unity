
//using System;
using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{

    [CustomEditor(typeof(CSky_SkySphere))]
    public partial class CSky_SkySphereEditor : CSky_CommonEditor
    {

        #region |Target|

        CSky_SkySphere tar;

        #endregion


        #region Color Correction.

        SerializedProperty m_HDR;
        SerializedProperty m_Exposure;

        #endregion

        #region Eclipses

        SerializedProperty m_SolarEclipseThreshold;

        #endregion

bool m_OtherSettingsFoldout;

       
        protected override string Title
        {
            get
            {
                return "Sky Sphere";
            }
        }

        string EvaluateByAboveSun
        {
            get { return "Evaluate time by above sun cycle"; }
        }

        string EvaluateByAboveMoon
        {
            get { return "Evaluate time by above sun cycle"; }
        }

        string EvaluateByFullSun
        {
            get { return "Evaluate time by full sun cycle"; }
        }

        string EvaluateByFullMoon
        {
            get { return "Evaluate time by full moon cycle"; }
        }

      
        protected override void OnEnable()
        {

            base.OnEnable();

            // Get target class.
            tar = (CSky_SkySphere)target;

            // Find resources and components properties.
            InitResoucesAndComponents();

            // Find celestials properties.
            InitCelestials();

            // Find atmosphere properties.
            InitAtmosphere();

            // Find lighting properties.
            InitLighting();

            // Find color correction and eclipses properties.
            InitOtherSettings();

        }


        void InitOtherSettings()
        {

            #region |Color Correction|
            
            m_HDR      = serObj.FindProperty("m_HDR");
            m_Exposure = serObj.FindProperty("m_Exposure");

            #endregion


            #region Eclipse

            m_SolarEclipseThreshold = serObj.FindProperty("m_SolarEclipseThreshold");

            #endregion
        }


        protected override void _OnInspectorGUI()
        {

           // Resources and components.
           OnInspectorResoucresAndComponents();

           // Celestials-
           OnInspectorCelestials();

           // Atmosphere-
           OnInspectorAtmosphere();

           // Lighting.
           OnInspectorLighting();

           // Other.
           OnInspectorOtherSettings();


        }


        void OnInspectorOtherSettings()
        {

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Other Settings", TextTitleStyle, ref m_OtherSettingsFoldout);
            if (m_OtherSettingsFoldout)
            {

                AC_EditorGUIUtility.ShurikenHeader("Color Correction", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    //GUI.backgroundColor = m_HDR.boolValue ? green : red;
                    EditorGUILayout.PropertyField(m_HDR, new GUIContent("HDR"));
                   // GUI.backgroundColor = Color.white;
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_Exposure, new GUIContent("Exposure"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();


                AC_EditorGUIUtility.ShurikenHeader("Eclipse", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(m_SolarEclipseThreshold, new GUIContent("Solar Eclipse Threshold"));
            }


        }

    }
}

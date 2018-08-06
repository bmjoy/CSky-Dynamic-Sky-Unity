
//using System;
using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{

    //[CustomEditor(typeof(CSky_SkySphere))]
    public partial class CSky_SkySphereEditor : CSky_CommonEditor
    {

       
        #region |Background|

        // Background.
        SerializedProperty m_BackgroundMeshQuality;
        SerializedProperty m_BackgroundColor;
        SerializedProperty m_BackgroundIntensity;
        SerializedProperty m_BackgroundIntensityMultiplier;

        #endregion

        #region |StarsField|

        // Stars field.
        SerializedProperty m_StarsFieldMeshQuality;

        SerializedProperty m_StarsFieldColor;
        SerializedProperty m_StarsFieldIntensity;
        SerializedProperty m_StarsFieldIntensityMultiplier;

        SerializedProperty m_StarsFieldScintillation;
        SerializedProperty m_StarsFieldScintillationSpeed;

        #endregion

        #region |Sun|

        // Sun.
        SerializedProperty m_SunPI;
        SerializedProperty m_SunTheta;

        SerializedProperty m_SunColor;
        SerializedProperty m_SunSize;
        SerializedProperty m_SunIntensity;

        #endregion

        #region |Moon|

        // Moon.
        SerializedProperty m_MoonMeshQuality;

        SerializedProperty m_MoonPI;
        SerializedProperty m_MoonTheta;

        SerializedProperty m_MoonColor;
        SerializedProperty m_MoonSize;
        SerializedProperty m_MoonIntensity;

        #endregion


        bool m_CelestialsFoldout;

        protected  void InitCelestials()
        {

            #region |Background|

            // Quality.
            m_BackgroundMeshQuality         = serObj.FindProperty("m_BackgroundMeshQuality");

            // COlor.
            m_BackgroundColor               = serObj.FindProperty("m_BackgroundColor");
            m_BackgroundIntensity           = serObj.FindProperty("m_BackgroundIntensity");

            // Curve Multiplier.
            m_BackgroundIntensityMultiplier = serObj.FindProperty("m_BackgroundIntensityMultiplier");

            #endregion

            #region |Stars Field|

            // Quality.
            m_StarsFieldMeshQuality         = serObj.FindProperty("m_StarsFieldMeshQuality");

            // Color.
            m_StarsFieldColor               = serObj.FindProperty("m_StarsFieldColor");
            m_StarsFieldIntensity           = serObj.FindProperty("m_StarsFieldIntensity");
            m_StarsFieldIntensityMultiplier = serObj.FindProperty("m_StarsFieldIntensityMultiplier");

            // Scintillation.
            m_StarsFieldScintillation       = serObj.FindProperty("m_StarsFieldScintillation");
            m_StarsFieldScintillationSpeed  = serObj.FindProperty("m_StarsFieldScintillationSpeed");

            #endregion

            #region |Sun|

            // Position.
            m_SunPI        = serObj.FindProperty("m_SunPI");
            m_SunTheta     = serObj.FindProperty("m_SunTheta");

            // Size.
            m_SunSize = serObj.FindProperty("m_SunSize");

            // Color
            m_SunColor     = serObj.FindProperty("m_SunColor");
            m_SunIntensity = serObj.FindProperty("m_SunIntensity");

            #endregion

            #region |Moon|

            // Quality.
            m_MoonMeshQuality = serObj.FindProperty("m_MoonMeshQuality");

            // Position.
            m_MoonPI          = serObj.FindProperty("m_MoonPI");
            m_MoonTheta       = serObj.FindProperty("m_MoonTheta");

            // Size
            m_MoonSize        = serObj.FindProperty("m_MoonSize");

            // Color
            m_MoonColor       = serObj.FindProperty("m_MoonColor");
            m_MoonIntensity   = serObj.FindProperty("m_MoonIntensity");

            #endregion

        }


        protected  void OnInspectorCelestials()
        {

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Celestials", TextTitleStyle, ref m_CelestialsFoldout);

            if(m_CelestialsFoldout)
            {

                // Background.

                AC_EditorGUIUtility.ShurikenHeader("Background", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_BackgroundMeshQuality, new GUIContent("Background Mesh Quality"));

                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_BackgroundColor, new GUIContent("Background Color"));
                    EditorGUILayout.PropertyField(m_BackgroundIntensity, new GUIContent("Background Intensity"));

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.PropertyField(m_BackgroundIntensityMultiplier, new GUIContent("Background Intensity Multiplier"));
                        EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);

                    EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();


                // Stars Field
                AC_EditorGUIUtility.ShurikenHeader("Stars Field", TextSectionStyle, 20);
                EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_StarsFieldMeshQuality, new GUIContent("Stars Field Mesh Quality"));
                    AC_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_StarsFieldColor, new GUIContent("Stars Field Color"));
                    EditorGUILayout.PropertyField(m_StarsFieldIntensity, new GUIContent("Stars Field Intensity"));

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.PropertyField(m_StarsFieldIntensityMultiplier, new GUIContent("Stars Field Intensity Multiplier"));
                        EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.PropertyField(m_StarsFieldScintillation, new GUIContent("Stars Field Scintillation"));
                    EditorGUILayout.PropertyField(m_StarsFieldScintillationSpeed, new GUIContent("Stars Field Scintillation Speed"));
                    AC_EditorGUIUtility.Separator(2);

                EditorGUILayout.Separator();


                
                // Sun.

                AC_EditorGUIUtility.ShurikenHeader("Sun", TextSectionStyle, 20);
                EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_SunPI, new GUIContent("Sun PI"));
                    EditorGUILayout.PropertyField(m_SunTheta, new GUIContent("Sun Theta"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_SunSize, new GUIContent("SunSize"));

                    EditorGUILayout.PropertyField(m_SunColor, new GUIContent("Sun Color"));
                    EditorGUILayout.PropertyField(m_SunIntensity, new GUIContent("Sun Intensity"));


                EditorGUILayout.Separator();



                // Moon.

                AC_EditorGUIUtility.ShurikenHeader("Moon", TextSectionStyle, 20);
                EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_MoonMeshQuality, new GUIContent("Moon Mesh Quality"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_MoonPI, new GUIContent("Moon PI"));
                    EditorGUILayout.PropertyField(m_MoonTheta, new GUIContent("Moon Theta"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_MoonSize, new GUIContent("MoonSize"));
                    EditorGUILayout.PropertyField(m_MoonColor, new GUIContent("Moon Color"));
                    EditorGUILayout.PropertyField(m_MoonIntensity, new GUIContent("Moon Intensity"));


                EditorGUILayout.Separator();




            }
           
        }

    }
}

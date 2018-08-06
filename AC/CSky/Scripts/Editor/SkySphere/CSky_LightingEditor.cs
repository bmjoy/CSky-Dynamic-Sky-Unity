
//using System;
using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{

    //[CustomEditor(typeof(CSky_SkySphere))]
    public partial class CSky_SkySphereEditor : CSky_CommonEditor
    {



        #region |Celestials|

        // Sun.
        SerializedProperty m_SunLightColor;
        SerializedProperty m_SunLightIntensity;
        SerializedProperty m_SunLightIntensityMultiplier;
        SerializedProperty m_SunLightThreshold;

        // Moon.
        SerializedProperty m_MoonLightColor;
        SerializedProperty m_MoonLightIntensity;
        SerializedProperty m_MoonLightIntensityMultiplier;

        SerializedProperty m_DisableMoonLightInDay;
        SerializedProperty m_MoonLightIntensityCorrector;

        #endregion


        #region |Environment|

        // Refresh.
        SerializedProperty m_EnviroUpdateInterval;

        // Ambient.
        SerializedProperty m_AmbientMode;

        // Ambient Sun.
        SerializedProperty m_AmbientSunSkyColor;
        SerializedProperty m_AmbientSunEquatorColor;
        SerializedProperty m_AmbientSunGroundColor;

        // Ambient Moon.
        SerializedProperty m_AmbientMoonSkyColor;
        SerializedProperty m_AmbientMoonEquatorColor;
        SerializedProperty m_AmbientMoonGroundColor;

        // Ambient Intensity.
        SerializedProperty m_AmbientIntensity;

        // Reflection
        SerializedProperty m_SendSkybox;

        #endregion


        #region |Unity Fog|

        SerializedProperty m_EnableUnityFog;
        SerializedProperty m_UnityFogMode;
        SerializedProperty m_UnitySunFogColor;
        SerializedProperty m_UnityMoonFogColor;
        SerializedProperty m_UnityFogDensity;
        SerializedProperty m_UnityFogStartDistance;
        SerializedProperty m_UnityFogEndDistance;

        #endregion




        bool m_LightingFoldout;

      
        protected  void InitLighting()
        {


            #region |Celestials|
            // Sun.
            m_SunLightColor               = serObj.FindProperty("m_SunLightColor");
            m_SunLightIntensity           = serObj.FindProperty("m_SunLightIntensity");
            m_SunLightIntensityMultiplier = serObj.FindProperty("m_SunLightIntensityMultiplier");

            // Moon.
            m_SunLightThreshold            = serObj.FindProperty("m_SunLightThreshold");
            m_MoonLightColor               = serObj.FindProperty("m_MoonLightColor");
            m_MoonLightIntensity           = serObj.FindProperty("m_MoonLightIntensity");
            m_MoonLightIntensityMultiplier = serObj.FindProperty("m_MoonLightIntensityMultiplier");

            m_DisableMoonLightInDay       = serObj.FindProperty("m_DisableMoonLightInDay");
            m_MoonLightIntensityCorrector = serObj.FindProperty("m_MoonLightIntensityCorrector");

            #endregion


            #region |Environment|

            // Refresh.
            m_EnviroUpdateInterval = serObj.FindProperty("m_EnviroUpdateInterval");

            // Mode
            m_AmbientMode       = serObj.FindProperty("m_AmbientMode");

            // Sun.
            m_AmbientSunSkyColor     = serObj.FindProperty("m_AmbientSunSkyColor");
            m_AmbientSunEquatorColor = serObj.FindProperty("m_AmbientSunEquatorColor");
            m_AmbientSunGroundColor  = serObj.FindProperty("m_AmbientSunGroundColor");

            // Moon.
            m_AmbientMoonSkyColor     = serObj.FindProperty("m_AmbientMoonSkyColor");
            m_AmbientMoonEquatorColor = serObj.FindProperty("m_AmbientMoonEquatorColor");
            m_AmbientMoonGroundColor  = serObj.FindProperty("m_AmbientMoonGroundColor");

            // Intensity.
            m_AmbientIntensity = serObj.FindProperty("m_AmbientIntensity");

            // Reflection.
            m_SendSkybox = serObj.FindProperty("m_SendSkybox");

            #endregion

            #region |UnityFog|

            m_EnableUnityFog = serObj.FindProperty("m_EnableUnityFog");
            m_UnityFogMode   = serObj.FindProperty("m_UnityFogMode");

            // Color.
            m_UnitySunFogColor = serObj.FindProperty("m_UnitySunFogColor");
            m_UnityMoonFogColor = serObj.FindProperty("m_UnityMoonFogColor");

            // Density
            m_UnityFogDensity = serObj.FindProperty("m_UnityFogDensity");

            // Fog distance.
            m_UnityFogStartDistance = serObj.FindProperty("m_UnityFogStartDistance");
            m_UnityFogEndDistance   = serObj.FindProperty("m_UnityFogEndDistance");

            #endregion

        }


        protected  void OnInspectorLighting()
        {

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Lighting", TextTitleStyle, ref m_LightingFoldout);

            if (m_LightingFoldout)
            {

                #region |Celestials|

                // Sun.

                AC_EditorGUIUtility.ShurikenHeader("Sun Light", TextSectionStyle, 20);
                EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_SunLightColor, new GUIContent("Sun Light Color"));
                    EditorGUILayout.PropertyField(m_SunLightIntensity, new GUIContent("Sun Light Intensity"));

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.PropertyField(m_SunLightIntensityMultiplier, new GUIContent("Sun Light Intensity Multiplier"));

                        EditorGUILayout.HelpBox(EvaluateByAboveSun, MessageType.Info);

                    EditorGUILayout.EndVertical();

                EditorGUILayout.PropertyField(m_SunLightThreshold, new GUIContent("Sun Light Threshold"));


                EditorGUILayout.Separator();


                // Moon.

                AC_EditorGUIUtility.ShurikenHeader("Moon Light", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_MoonLightColor, new GUIContent("Moon Light Color"));
                    EditorGUILayout.PropertyField(m_MoonLightIntensity, new GUIContent("Moon Light Intensity"));


                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.PropertyField(m_MoonLightIntensityMultiplier, new GUIContent("Moon Light Intensity Multiplier"));

                        EditorGUILayout.HelpBox(EvaluateByAboveMoon, MessageType.Info);

                    EditorGUILayout.EndVertical();

                    //GUI.backgroundColor = m_DisableMoonLightInDay.boolValue ? green : red;
                    EditorGUILayout.PropertyField(m_DisableMoonLightInDay, new GUIContent("Disable Moon Light In Day"));
                    //GUI.backgroundColor = Color.white;

                    if (m_DisableMoonLightInDay.boolValue)
                    {

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                            EditorGUILayout.PropertyField(m_MoonLightIntensityCorrector, new GUIContent("Moon Light Intensity Corrector"));
                            EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);
                        EditorGUILayout.EndVertical();
                    }


                EditorGUILayout.Separator();


                #endregion


                #region Environment

                AC_EditorGUIUtility.ShurikenHeader("Environment", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(m_EnviroUpdateInterval, new GUIContent("Update Interval"));
                EditorGUILayout.Separator();

                // GUI.backgroundColor = m_SendSkybox.boolValue ? green : red;
                EditorGUILayout.PropertyField(m_SendSkybox, new GUIContent("Send Skybox"));
                // GUI.backgroundColor = Color.white;
                AC_EditorGUIUtility.Separator(2);
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(m_AmbientMode, new GUIContent("Ambient Mode"));
                AC_EditorGUIUtility.Separator(2);
                EditorGUILayout.Separator();

                switch (m_AmbientMode.enumValueIndex)
                {

                    case 0:

                        EditorGUILayout.PropertyField(m_AmbientIntensity, new GUIContent("Ambient Intensity"));

                        if (m_SendSkybox.boolValue)
                        {

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                EditorGUILayout.PropertyField(m_AmbientSunGroundColor, new GUIContent("Ambient Ground Color"));

                                EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                if(m_MoonRayleigh.boolValue)
                                {
                                    EditorGUILayout.PropertyField(m_AmbientMoonGroundColor, new GUIContent("Ambient Moon Ground Color"));
                                    EditorGUILayout.HelpBox(EvaluateByFullMoon + " | " + "Moon influence mode = add", MessageType.Info);
                                }

                            EditorGUILayout.EndVertical();
                        }

                    break;

                    case 1:

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            EditorGUILayout.PropertyField(m_AmbientSunSkyColor, new GUIContent("Ambient Sun Sky Color"));
                            EditorGUILayout.PropertyField(m_AmbientSunEquatorColor, new GUIContent("Ambient Sun Equator Color"));
                            EditorGUILayout.PropertyField(m_AmbientSunGroundColor, new GUIContent("Ambient Sun Ground Color"));

                            EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);
                        EditorGUILayout.EndVertical();
                        AC_EditorGUIUtility.Separator(2);
                        EditorGUILayout.Separator();

                        if(m_MoonRayleigh.boolValue)
                        {

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                                EditorGUILayout.PropertyField(m_AmbientMoonSkyColor, new GUIContent("Ambient Moon Sky Color"));
                                EditorGUILayout.PropertyField(m_AmbientMoonEquatorColor, new GUIContent("Ambient Moon  Equator Color"));
                                EditorGUILayout.PropertyField(m_AmbientMoonGroundColor, new GUIContent("Ambient Moon  Ground Color"));
                            EditorGUILayout.HelpBox(EvaluateByFullMoon +  "|" + "Moon influence mode = add" , MessageType.Info);
                            EditorGUILayout.EndVertical();
                        }

                    break;


                    case 2:
                    {

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            EditorGUILayout.PropertyField(m_AmbientSunSkyColor, new GUIContent("Ambient Sun Sky Color"));
                            if(m_SendSkybox.boolValue) EditorGUILayout.PropertyField(m_AmbientSunGroundColor, new GUIContent("Ambient Sun Ground Color"));
                                EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);

                        EditorGUILayout.EndVertical();

                        AC_EditorGUIUtility.Separator(2);
                        EditorGUILayout.Separator();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            if(m_MoonRayleigh.boolValue)
                            {
                                EditorGUILayout.PropertyField(m_AmbientMoonSkyColor, new GUIContent("Ambient Moon Sky Color"));
                                    if (m_SendSkybox.boolValue) 
                                            EditorGUILayout.PropertyField(m_AmbientMoonGroundColor, new GUIContent("Ambient Moon Ground Color"));

                                EditorGUILayout.HelpBox(EvaluateByFullMoon + " | " + "Moon influence mode = add", MessageType.Info);
                            }

                        EditorGUILayout.EndVertical();
                    }

                    break;
                }

                #endregion


                #region |Unity Fog|

                AC_EditorGUIUtility.ShurikenHeader("Unity Fog", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                //GUI.backgroundColor = m_EnableUnityFog.boolValue ? green : red;
                EditorGUILayout.PropertyField(m_EnableUnityFog, new GUIContent("Enable Unity Fog"));
                //GUI.backgroundColor = Color.white;
                if (m_EnableUnityFog.boolValue)
                {

                    EditorGUILayout.PropertyField(m_UnityFogMode, new GUIContent("Unity Fog Mode"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();


                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(m_UnitySunFogColor, new GUIContent("Unity Sun Fog Color"));
                    EditorGUILayout.HelpBox(EvaluateByFullSun, MessageType.Info);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        if (m_MoonRayleigh.boolValue)
                        {
                            EditorGUILayout.PropertyField(m_UnityMoonFogColor, new GUIContent("Unity Moon Fog Color"));
                            EditorGUILayout.HelpBox(EvaluateByFullMoon, MessageType.Info);
                        }

                    EditorGUILayout.EndVertical();


                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    if (m_UnityFogMode.enumValueIndex != 0)
                    {
                        EditorGUILayout.PropertyField(m_UnityFogDensity, new GUIContent("Unity Fog Density"));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(m_UnityFogStartDistance, new GUIContent("Unity Fog Start Distance"));
                        EditorGUILayout.PropertyField(m_UnityFogEndDistance, new GUIContent("Unity Fog End Distance"));
                    }
                }


                EditorGUILayout.Separator();

                #endregion

            }
           
        }

    }
}

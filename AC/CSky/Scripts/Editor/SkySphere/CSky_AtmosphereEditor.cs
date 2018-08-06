
//using System;
using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{

    //[CustomEditor(typeof(CSky_SkySphere))]
    public partial class CSky_SkySphereEditor : CSky_CommonEditor
    {

       
        #region |Atmosphere|Settings|

        // Atmosphere Settings.
        SerializedProperty m_AtmosphereMeshQuality;
        SerializedProperty m_AtmosphereQuality;
        SerializedProperty m_AtmosphereShader;

        #endregion

      
        #region |Atmosphere|Rayleigh|


        SerializedProperty m_WavelengthR;
        SerializedProperty m_WavelengthG;
        SerializedProperty m_WavelengthB;


        SerializedProperty m_AtmosphereThickness;

        SerializedProperty m_SunBrightness;

        SerializedProperty m_SunAtmosphereTint;

        SerializedProperty m_SunIntensityFactor;

        SerializedProperty m_MoonRayleigh;

        SerializedProperty m_MoonBrightness;
        SerializedProperty m_MoonAtmosphereTint;
        SerializedProperty m_MoonIntensityFactor;


        #endregion

       
       #region |Mie|

        // General.
        SerializedProperty m_Mie;
        SerializedProperty m_Turbidity;

        // Sun Mie.
        SerializedProperty m_SunMieColor;
        SerializedProperty m_SunMieAnisotropy;
        SerializedProperty m_SunMieScattering;

        // Moon Mie.
        SerializedProperty m_MoonMieColor;
        SerializedProperty m_MoonMieAnisotropy;
        SerializedProperty m_MoonMieScattering;

        #endregion

        #region |Atmosphere|Other|

        // Exponent.
        SerializedProperty m_AtmosphereExponent;

        // Camera Height.
        SerializedProperty m_CameraHeight;

        // Horizon Offset.
        SerializedProperty m_HorizonOffset;

        // Zenith.
        SerializedProperty m_RayleighZenithLength;
        SerializedProperty m_MieZenithLength;

        #endregion

        bool m_AtmosphereFoldout;

        protected  void InitAtmosphere()
        {

            #region |Atmosphere|Settings|

            m_AtmosphereMeshQuality = serObj.FindProperty("m_AtmosphereMeshQuality");
            m_AtmosphereQuality     = serObj.FindProperty("m_AtmosphereQuality");
            m_AtmosphereShader      = serObj.FindProperty("m_AtmosphereShader");

            #endregion

            #region |Atmosphere|Rayleigh|

            m_WavelengthR = serObj.FindProperty("m_WavelengthR");
            m_WavelengthG = serObj.FindProperty("m_WavelengthG");
            m_WavelengthB = serObj.FindProperty("m_WavelengthB");

            m_AtmosphereThickness = serObj.FindProperty("m_AtmosphereThickness");


            // Sun.
            m_SunBrightness      = serObj.FindProperty("m_SunBrightness");
            m_SunAtmosphereTint  = serObj.FindProperty("m_SunAtmosphereTint");
            m_SunIntensityFactor = serObj.FindProperty("m_SunIntensityFactor");

            // Moon.
            m_MoonRayleigh         = serObj.FindProperty("m_MoonRayleigh");
            m_MoonBrightness       = serObj.FindProperty("m_MoonBrightness");
            m_MoonAtmosphereTint   = serObj.FindProperty("m_MoonAtmosphereTint");
            m_MoonIntensityFactor  = serObj.FindProperty("m_MoonIntensityFactor");

            #endregion

            #region |Atmosphere|Mie|

            // General.
            m_Mie       = serObj.FindProperty("m_Mie");
            m_Turbidity = serObj.FindProperty("m_Turbidity");

            // Sun.
            m_SunMieColor      = serObj.FindProperty("m_SunMieColor");
            m_SunMieAnisotropy = serObj.FindProperty("m_SunMieAnisotropy");
            m_SunMieScattering = serObj.FindProperty("m_SunMieScattering");

            // Moon.
            m_MoonMieColor      = serObj.FindProperty("m_MoonMieColor");
            m_MoonMieAnisotropy = serObj.FindProperty("m_MoonMieAnisotropy");
            m_MoonMieScattering = serObj.FindProperty("m_MoonMieScattering");


            #endregion

            #region |Atmosphere|Other|

            m_AtmosphereExponent   = serObj.FindProperty("m_AtmosphereExponent");
            m_CameraHeight         = serObj.FindProperty("m_CameraHeight");
            m_HorizonOffset        = serObj.FindProperty("m_HorizonOffset");
            m_RayleighZenithLength = serObj.FindProperty("m_RayleighZenithLength");
            m_MieZenithLength      = serObj.FindProperty("m_MieZenithLength");

            #endregion
        }


        protected  void OnInspectorAtmosphere()
        {

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Atmosphere", TextTitleStyle, ref m_AtmosphereFoldout);

            if (m_AtmosphereFoldout)
            {

                // Settings.
                AC_EditorGUIUtility.ShurikenHeader("Atmosphere Settings", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_AtmosphereMeshQuality, new GUIContent("Atmosphere Mesh Quality"));

                    GUI.backgroundColor = m_AtmosphereQuality.enumValueIndex == 1 ? Color.yellow : green;
                    EditorGUILayout.PropertyField(m_AtmosphereQuality, new GUIContent("Atmosphere Quality"));
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.PropertyField(m_AtmosphereShader, new GUIContent("Atmosphere Shader"));

                EditorGUILayout.Separator();


                
                // Wavelengths.

                AC_EditorGUIUtility.ShurikenHeader("Rayleigh", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    GUI.backgroundColor = Color.red;
                    EditorGUILayout.PropertyField(m_WavelengthR, new GUIContent("Wavelength R"));

                    GUI.backgroundColor = Color.green;
                    EditorGUILayout.PropertyField(m_WavelengthG, new GUIContent("Wavelength G"));

                    GUI.backgroundColor = Color.blue;
                    EditorGUILayout.PropertyField(m_WavelengthB, new GUIContent("Wavelength B"));

                    GUI.backgroundColor = Color.white;


                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_AtmosphereThickness, new GUIContent("Atmosphere Thickness"));

                    EditorGUILayout.Separator();



                    EditorGUILayout.PropertyField(m_SunBrightness, new GUIContent("Sun Brightness"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_SunAtmosphereTint, new GUIContent(m_MoonRayleigh.boolValue ? "Sun Atmosphere Tint" : "Day Atmosphere Tint"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    if (m_AtmosphereShader.enumValueIndex == 1)
                    {
                        EditorGUILayout.PropertyField(m_SunIntensityFactor, new GUIContent(m_MoonRayleigh.boolValue ? "Sun Intensity Factor" : "Day Intensity Factor"));
                        AC_EditorGUIUtility.Separator(2);
                        EditorGUILayout.Separator();
                    }

                    //GUI.backgroundColor = m_EnableMoonInfluence.boolValue ? green : red;
                    EditorGUILayout.PropertyField(m_MoonRayleigh, new GUIContent("Moon Rayleigh"));
                    EditorGUILayout.Separator();
                

                    EditorGUILayout.PropertyField(m_MoonBrightness, new GUIContent("Moon Brightness"));

                    EditorGUILayout.PropertyField(m_MoonAtmosphereTint, new GUIContent(m_MoonRayleigh.boolValue ? "Moon Atmosphere Tint" : "Night Atmosphere Tint"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    if (m_AtmosphereShader.enumValueIndex == 1)
                    {
                        EditorGUILayout.PropertyField(m_MoonIntensityFactor, new GUIContent("Night Intensity Factor"));
                        AC_EditorGUIUtility.Separator(2);
                        EditorGUILayout.Separator();
                    }



                EditorGUILayout.Separator();


     
                 // Mie.
                AC_EditorGUIUtility.ShurikenHeader("Mie", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_Mie, new GUIContent("Mie"));

                    if (m_AtmosphereShader.intValue == 1)
                    {
                        EditorGUILayout.PropertyField(m_Turbidity, new GUIContent("Turbidity"));
                        AC_EditorGUIUtility.Separator(2);
                        EditorGUILayout.Separator();

                    }

                    EditorGUILayout.PropertyField(m_SunMieColor, new GUIContent("Sun Mie Color"));
                    EditorGUILayout.PropertyField(m_SunMieAnisotropy, new GUIContent("Sun Mie Anisotropy"));
                    EditorGUILayout.PropertyField(m_SunMieScattering, new GUIContent("Sun Mie Scattering"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();

                    EditorGUILayout.PropertyField(m_MoonMieColor, new GUIContent("Moon Mie Color"));
                    EditorGUILayout.PropertyField(m_MoonMieAnisotropy, new GUIContent("Moon Mie Anisotropy"));
                    EditorGUILayout.PropertyField(m_MoonMieScattering, new GUIContent("Moon Mie Scattering"));


                EditorGUILayout.Separator();


               

                // Atmosphere Other.

                AC_EditorGUIUtility.ShurikenHeader("Other", TextSectionStyle, 20);
                EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_AtmosphereExponent, new GUIContent("Atmosphere Exponent"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();


                    EditorGUILayout.PropertyField(m_HorizonOffset, new GUIContent("Horizon Offset"));
                    AC_EditorGUIUtility.Separator(2);
                    EditorGUILayout.Separator();


                    if (m_AtmosphereShader.intValue == 1)
                    {
                        EditorGUILayout.PropertyField(m_RayleighZenithLength, new GUIContent("Rayleigh Zenith Length"));
                        EditorGUILayout.PropertyField(m_MieZenithLength, new GUIContent("Mie Zenith Length"));

                    }
                    else
                    {

                        EditorGUILayout.PropertyField(m_CameraHeight, new GUIContent("Camera Height"));
                    }

                    // AC_EditorGUIUtility.Separator(2);
                    //EditorGUILayout.Separator();

          
                EditorGUILayout.Separator();


            }
       
        }

    }
}

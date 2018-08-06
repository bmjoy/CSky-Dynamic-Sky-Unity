
//using System;
using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{

    //[CustomEditor(typeof(CSky_SkySphere))]
    public partial class CSky_SkySphereEditor : CSky_CommonEditor
    {

       

        SerializedProperty  m_Resources;
        SerializedProperty  m_Camera;

        bool m_ResourcesAndComponentsFoldout;

        protected  void InitResoucesAndComponents()
        {

            // Resources.
            m_Resources = serObj.FindProperty("m_Resources");

            // Components.
            m_Camera = serObj.FindProperty("m_Camera");
        }


        protected  void OnInspectorResoucresAndComponents()
        {

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Resources And Components", TextTitleStyle, ref m_ResourcesAndComponentsFoldout);

            if(m_ResourcesAndComponentsFoldout)
            {

                // Resources.
                AC_EditorGUIUtility.ShurikenHeader("Resources", TextSectionStyle, 20);


                    GUI.backgroundColor = (m_Resources.objectReferenceValue != null) ? green : red;
                    EditorGUILayout.PropertyField(m_Resources, new GUIContent("Resources"));
                    GUI.backgroundColor = Color.white;

                    if (m_Resources.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("Resources Data is not assigned", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Be sure to allocate all resources", MessageType.Info);
                    }

                EditorGUILayout.Separator();


                // Build.
                AC_EditorGUIUtility.ShurikenHeader("Build", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    GUI.backgroundColor = (m_Resources.objectReferenceValue != null) ? green : red;
                    if (GUILayout.Button("Build Sky Sphere", GUILayout.Height(30)))
                        tar.BuildSkySphere();
                    GUI.backgroundColor = Color.white;

                EditorGUILayout.Separator();


                AC_EditorGUIUtility.ShurikenHeader("Camera", TextSectionStyle, 20);

                    GUI.backgroundColor = (m_Camera.objectReferenceValue != null) ? green : red;
                    EditorGUILayout.PropertyField(m_Camera, new GUIContent("Camera"));
                    GUI.backgroundColor = Color.white;

                    if (m_Camera.objectReferenceValue == null)
                        EditorGUILayout.HelpBox("Camera is not assigned", MessageType.Warning);

                EditorGUILayout.Separator();

            }

           
        }

    }
}

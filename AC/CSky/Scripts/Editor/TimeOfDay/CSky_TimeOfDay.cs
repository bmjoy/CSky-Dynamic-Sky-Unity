//////////////////////////////////////////////////////
/// CSky: DateTime.
/// DateTime Editor Class.
/// Description: Custom inspector for DateTime class.
/// 
//////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{


    [CustomEditor(typeof(CSky_TimeOfDay))]
    public class CSky_TimeOfDayEditor : CSky_DateTimeEditor
    {



        #region Time.

        SerializedProperty m_Latitude;
        SerializedProperty m_Longitude;
       

        SerializedProperty m_UTC;
       

        #endregion



        #region Foldouts.

        bool m_LocationFoldout;
     

        #endregion

        protected override string Title
        {
            get
            {
                return "Time Of Day";
            }
        }

        protected override bool OverrideDayRange
        {
            get { return true; }
        }


        protected override void OnEnable()
        {


            base.OnEnable();


            #region Location.

            
            m_Latitude  = serObj.FindProperty("celestialsCalculations.m_Latitude");
            m_Longitude = serObj.FindProperty("celestialsCalculations.m_Longitude");
            m_UTC = serObj.FindProperty("celestialsCalculations.m_UTC");
           
            #endregion



        }

        /*
        protected override void UpdateInspector()
        {

            base.UpdateInspector();

        }*/


        protected override void LastUpdateInspector()
        {


            #region Location.

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Location", TextTitleStyle, ref m_LocationFoldout);

            if (m_LocationFoldout)
            {

                AC_EditorGUIUtility.ShurikenHeader("Location", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(m_Latitude, new GUIContent("Latitude"));
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(m_Longitude, new GUIContent("Longitude"));
                EditorGUILayout.Separator();


                EditorGUILayout.PropertyField(m_UTC, new GUIContent("UTC"));
                EditorGUILayout.Separator();

            }

            #endregion



            base.LastUpdateInspector();
        }




    }
}

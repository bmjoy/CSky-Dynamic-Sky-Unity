//////////////////////////////////////////////////////
/// CSky.
/// Name: Date Time Editor.
/// Description: Custom inspector for Date Time class.
/// 
//////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{


    [CustomEditor(typeof(CSky_DateTime))]
    public class CSky_DateTimeEditor : CSky_CommonEditor
    {

        #region target.

        CSky_DateTime tar;

        #endregion

        #region Time.

        SerializedProperty m_ProgressTime;
        SerializedProperty m_Timeline;

        SerializedProperty m_UseDayNightLength;
        SerializedProperty m_DayRange;
        SerializedProperty m_DayLength;
        SerializedProperty m_NightLength;

        SerializedProperty m_Hour;
        SerializedProperty m_Minute;
        SerializedProperty m_Second;
    
        #endregion

        #region Date.

        SerializedProperty m_Day;
        SerializedProperty m_Month;
        SerializedProperty m_Year;

        #endregion

        #region Options.

        SerializedProperty m_SyncWithSystem;

        #endregion

        #region Unity Events.

        SerializedProperty OnInvalidValue;
        SerializedProperty OnHour;
        SerializedProperty OnMinute;
        SerializedProperty OnDay;
        SerializedProperty OnMonth;
        SerializedProperty OnYear;

        #endregion

        #region Foldouts.

        bool
        m_TimeFoldout,
        m_DateFoldout,
        m_OptionsFoldout,
        m_ValueEventsFoldout,
        m_TimeEventsFoldout,
        m_DateEventsFoldout;

        #endregion

        protected override string Title
        {
            get
            {
                return "Date Time";
            }
        }

        protected virtual bool OverrideDayRange
        {
            get { return false; }
        }


        protected override void OnEnable()
        {


            base.OnEnable();

            #region Target.

            tar = (CSky_DateTime)target;

            #endregion

            #region Time.

            // Timeline.
            m_ProgressTime = serObj.FindProperty("m_ProgressTime");
            m_Timeline = serObj.FindProperty("m_Timeline");

            // Length
            m_UseDayNightLength = serObj.FindProperty("m_UseDayNightLength");
            m_DayRange          = serObj.FindProperty("m_DayRange");
            m_DayLength         = serObj.FindProperty("m_DayLengthInMinutes");
            m_NightLength       = serObj.FindProperty("m_NightLengthInMinutes");

            // Time.
            m_Hour   = serObj.FindProperty("m_Hour");
            m_Minute = serObj.FindProperty("m_Minute");
            m_Second = serObj.FindProperty("m_Second");

            #endregion

            #region Date

            m_Day   = serObj.FindProperty("m_Day");
            m_Month = serObj.FindProperty("m_Month");
            m_Year  = serObj.FindProperty("m_Year");

            #endregion

            #region Options.

            m_SyncWithSystem = serObj.FindProperty("m_SyncWithSystem");

            #endregion

            #region Unity Events.

            // Values.
            OnInvalidValue = serObj.FindProperty("OnInvalidValue");

            // DateTime.
            OnHour   = serObj.FindProperty("OnHour");
            OnMinute = serObj.FindProperty("OnMinute");
            OnDay    = serObj.FindProperty("OnDay");
            OnMonth  = serObj.FindProperty("OnMonth");
            OnYear   = serObj.FindProperty("OnYear");

            #endregion

        }

        protected override void _OnInspectorGUI()
        {

            #region Time.

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Time", TextTitleStyle, ref m_TimeFoldout);

            if (m_TimeFoldout)
            {

                AC_EditorGUIUtility.ShurikenHeader("Timeline", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    GUI.backgroundColor = m_ProgressTime.boolValue ? green : red;

                    EditorGUILayout.PropertyField(m_ProgressTime, new GUIContent("ProgressTime"));

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(m_Timeline, new GUIContent("Timeline"));
                    EditorGUILayout.EndVertical();

                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.Separator();


                AC_EditorGUIUtility.ShurikenHeader("Time Length", TextSectionStyle, 20);
                EditorGUILayout.Separator();

                    if (!m_SyncWithSystem.boolValue)
                    {


                        GUI.backgroundColor = m_UseDayNightLength.boolValue ? green : red;

                        EditorGUILayout.PropertyField(m_UseDayNightLength, new GUIContent("Use Day Night Length"));

                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        if (m_UseDayNightLength.boolValue)
                        {

                            
                            if (!OverrideDayRange)
                            {

                                // Day Range.
                                float min = m_DayRange.vector2Value.x;
                                float max = m_DayRange.vector2Value.y;

                                AC_EditorGUIUtility.ShurikenHeader("Day Range", TextSectionStyle, 20);
                                
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.MinMaxSlider(ref min, ref max, 0, 24);

                                    m_DayRange.vector2Value = new Vector2(min, max);

                                    EditorGUILayout.PropertyField(m_DayRange, new GUIContent(""));
                                }
                                EditorGUILayout.EndHorizontal();

                                string startInfo = "Day Start: " + CSky_DateTimeHelper.TimeToString
                                (
                                    CSky_DateTimeHelper.GetTimelineHour(min),
                                    CSky_DateTimeHelper.GetTimelineMinute(min)
                                );

                                string endInfo = "Day End: " + CSky_DateTimeHelper.TimeToString
                                (
                                    CSky_DateTimeHelper.GetTimelineHour(max),
                                    CSky_DateTimeHelper.GetTimelineMinute(max)
                                );
                                EditorGUILayout.HelpBox(startInfo + " | " + endInfo, MessageType.Info);

                                EditorGUILayout.Separator();
                            }
                            AC_EditorGUIUtility.Separator(2);
                            EditorGUILayout.PropertyField(m_DayLength, new GUIContent("Day In Minutes"));
                            EditorGUILayout.PropertyField(m_NightLength, new GUIContent("Night In Minutes"));
                            
                        }
                        else
                        {

                            EditorGUILayout.PropertyField(m_DayLength, new GUIContent("Day In Minutes"));

                        }
                        EditorGUILayout.Separator();
                        EditorGUILayout.EndVertical();
                    }
                EditorGUILayout.Separator();


                AC_EditorGUIUtility.ShurikenHeader("Set Time", TextSectionStyle, 20);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.PropertyField(m_Hour, new GUIContent("Hour"));
                        EditorGUILayout.PropertyField(m_Minute, new GUIContent("Minute"));
                        EditorGUILayout.PropertyField(m_Second, new GUIContent("Second"));

                        GUI.backgroundColor = green;
                        if (GUILayout.Button("Set Time", GUILayout.MinHeight(30)))
                        {

                            tar.Hour = m_Hour.intValue;
                            tar.Minute = m_Minute.intValue;
                            tar.Second = m_Second.intValue;

                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();

            }

            #endregion


            #region Date

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Date", TextTitleStyle, ref m_DateFoldout);
            if(m_DateFoldout)
            {


                EditorGUILayout.PropertyField(m_Day, new GUIContent("Day"));
                EditorGUILayout.PropertyField(m_Month, new GUIContent("Month"));
                EditorGUILayout.PropertyField(m_Year, new GUIContent("Year"));

            }

            #endregion

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Options", TextTitleStyle, ref m_OptionsFoldout);
            if (m_OptionsFoldout)
            {

                GUI.backgroundColor = m_SyncWithSystem.boolValue ? Color.green : Color.red;
                EditorGUILayout.Separator();
                    EditorGUILayout.PropertyField(m_SyncWithSystem, new GUIContent("Synchronize With System DateTime"));
                EditorGUILayout.Separator();
                GUI.backgroundColor = Color.white;

            }



            LastUpdateInspector();


        }

        protected virtual void LastUpdateInspector()
        {


            #region Unity Events

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Value Events", TextTitleStyle, ref m_ValueEventsFoldout);
            if (m_ValueEventsFoldout)
            {
                EditorGUILayout.PropertyField(OnInvalidValue, new GUIContent("On Invalid Value"));
            }

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Time Events", TextTitleStyle, ref m_TimeEventsFoldout);

            if (m_TimeEventsFoldout)
            {
                EditorGUILayout.PropertyField(OnHour, new GUIContent("On Hour"));
                EditorGUILayout.PropertyField(OnMinute, new GUIContent("On Minute"));
            }

            AC_EditorGUIUtility.ShurikenFoldoutHeader("Date Events", TextTitleStyle, ref m_DateEventsFoldout);
            if (m_DateEventsFoldout)
            {
                EditorGUILayout.PropertyField(OnDay, new GUIContent("On Day"));
                EditorGUILayout.PropertyField(OnMonth, new GUIContent("On Month"));
                EditorGUILayout.PropertyField(OnYear, new GUIContent("On Year"));
            }

            #endregion


        }

    }
}

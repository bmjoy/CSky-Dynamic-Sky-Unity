/////////////////////////////////////////////////////
/// CSky
/// Name: Common Editor
/// Description: Common for CSky custom inspectors. 
/// 
/////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using AC.Utility;

namespace AC.CSky
{


    
    public abstract class CSky_CommonEditor : Editor
    {


       protected SerializedObject serObj;


        protected virtual GUIStyle TextTitleStyle
        {

            get
            {

                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 20;

                return style;
            }
        }


        protected virtual GUIStyle TextSectionStyle
        {

            get
            {

                GUIStyle style  = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                style.fontSize  = 10;

                return style;
            }
        }

        protected virtual string Title
        {
            get
            {
                return "New Class";
            }
        }

       protected Color green { get { return EditorGUIUtility.isProSkin ? Color.green : Color.green * 0.7f; } }

       protected Color red { get { return EditorGUIUtility.isProSkin ? Color.red : Color.red * 0.7f; } }


        protected virtual void OnEnable()
        {

            #region Target.

            serObj = new SerializedObject(target);
       
            #endregion

        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            AC_EditorGUIUtility.ShurikenHeader(Title, TextTitleStyle, 30);

            _OnInspectorGUI();

            serObj.ApplyModifiedProperties();
        }

        protected abstract void _OnInspectorGUI();
       

    }
}

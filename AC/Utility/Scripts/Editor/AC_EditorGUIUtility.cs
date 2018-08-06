//////////////////////////////////////////////////////
/// AC Utility.
/// Name: GUI Utility
/// Description: Small utility for custom inspectors.
///
//////////////////////////////////////////////////////

#if UNITY_EDITOR
using UnityEngine; 
using UnityEditor;

namespace AC.Utility
{

	public static class AC_EditorGUIUtility
	{
        #region Separator

		public static void Separator(int height)
		{
			GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(height) });
		}

       
		public static void Separator(int height, Color color)
		{
			GUI.color = color;
			GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(height) });
			GUI.color = Color.white;
		}

        #endregion

        #region Fields

		public static void PropertyField(SerializedProperty property, string name, int width)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(name, GUILayout.MinWidth(20));
			EditorGUILayout.PropertyField(property, new GUIContent(""), GUILayout.MaxWidth(width), GUILayout.MinWidth(width * 0.5f));
			EditorGUILayout.EndHorizontal();
		}
			
      
		public static void CurveField(string name, SerializedProperty curve, Color color, Rect rect, int width)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(name,GUILayout.MinWidth(20));
			EditorGUILayout.CurveField(curve.animationCurveValue, color, rect, GUILayout.MaxWidth(width), GUILayout.MinWidth(width*0.5f));
			EditorGUILayout.EndHorizontal();
		}

        #endregion

        #region Label

		public static void Label(string text, GUIStyle textStyle, bool center)
		{

			if(center)
			{
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(text, textStyle);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			} 
			else
			{ 
				GUILayout.Label(text, textStyle);
			}
		}
			
		public static void Label(string text, GUIStyle textStyle, bool center, int width)
		{

			if(center)
			{
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(text, textStyle, GUILayout.Width(width));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			} 
			else
			{
				GUILayout.Label(text, textStyle, GUILayout.Width(width));
			}
		}

        #endregion

        #region Header

		public static void ShurikenHeader(string text, GUIStyle texStyle)
		{


			GUIStyle h = new GUIStyle("ShurikenModuleTitle")
			{
				font          = new GUIStyle("Label").font,
				border        = new RectOffset(15, 7, 4, 4),
				fixedHeight   = 30,
				contentOffset = new Vector2(20f, -2f)
			}; 

            

			EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
			{
				Label(text, texStyle, true);
			}
			EditorGUILayout.EndHorizontal();
		}
			
       
		public static void ShurikenHeader(string text, GUIStyle texStyle, int height)
		{

			GUIStyle h = new GUIStyle("ShurikenModuleTitle")
			{
				font          = new GUIStyle("Label").font,
				border        = new RectOffset(15, 7, 4, 4),
				fixedHeight   = height,
				contentOffset = new Vector2(20f, -2f)
			}; 

			EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25)); 
			{
				Label(text, texStyle, true);
			}
			EditorGUILayout.EndHorizontal();
		}


        public static void ShurikenHeader(string text, GUIStyle texStyle, Color color)
        {


            GUIStyle h = new GUIStyle("ShurikenModuleTitle")
            {
                font = new GUIStyle("Label").font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 30,
                contentOffset = new Vector2(20f, -2f)
            };

            GUI.backgroundColor = color;
            EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
            GUI.backgroundColor = Color.white;
            {
                Label(text, texStyle, true);
            }
            EditorGUILayout.EndHorizontal();
        }


        public static void ShurikenHeader(string text, GUIStyle texStyle, Color color, int height)
        {

            GUIStyle h = new GUIStyle("ShurikenModuleTitle")
            {
                font = new GUIStyle("Label").font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = height,
                contentOffset = new Vector2(20f, -2f)
            };

            GUI.backgroundColor = color;
            EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
            GUI.backgroundColor = Color.white;
            {
                Label(text, texStyle, true);
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Foldout Header

        public static void ShurikenFoldoutHeader(string text, GUIStyle texStyle, SerializedProperty foldout)
		{

			GUIStyle h = new GUIStyle("ShurikenModuleTitle")
			{
				font          = new GUIStyle("Label").font,
				border        = new RectOffset(15, 7, 4, 4),
				fixedHeight   = 22,
				contentOffset = new Vector2(20f, -2f)
			}; 

			EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
			{

				foldout.boolValue = GUILayout.Toggle(foldout.boolValue, new GUIContent(text), EditorStyles.foldout, GUILayout.Width(25));
				//Label (tex, texStyle, true);
			}
			EditorGUILayout.EndHorizontal();
		}
			

		public static void ShurikenFoldoutHeader(string text, GUIStyle texStyle, Color color, SerializedProperty foldout)
		{

			GUIStyle h = new GUIStyle("ShurikenModuleTitle")
			{
				font          = new GUIStyle("Label").font,
				border        = new RectOffset(15, 7, 4, 4),
				fixedHeight   = 22,
				contentOffset = new Vector2(20f, -2f)
			}; 

			GUI.backgroundColor = color;
			EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
			GUI.backgroundColor = Color.white;
			{
				foldout.boolValue = GUILayout.Toggle(foldout.boolValue, new GUIContent(text), EditorStyles.foldout, GUILayout.Width(25));
				//Label (tex, texStyle, true);
			}
			EditorGUILayout.EndHorizontal();
		}


		public static void ShurikenFoldoutHeader(string text, GUIStyle texStyle, ref bool foldout)
		{

			GUIStyle h = new GUIStyle("ShurikenModuleTitle")
			{
				font          = new GUIStyle("Label").font,
				border        = new RectOffset(15, 7, 4, 4),
				fixedHeight   = 22,
				contentOffset = new Vector2(20f, -2f)
			}; 

			EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
			{
				foldout = GUILayout.Toggle(foldout, new GUIContent(text), EditorStyles.foldout, GUILayout.Width(25));
				//Label (tex, texStyle, true);
			}
			EditorGUILayout.EndHorizontal();
		}


		public static void ShurikenFoldoutHeader(string text, GUIStyle texStyle, Color color, ref bool foldout)
		{

			GUIStyle h = new GUIStyle("ShurikenModuleTitle")
			{
				font          = new GUIStyle("Label").font,
				border        = new RectOffset(15, 7, 4, 4),
				fixedHeight   = 22,
				contentOffset = new Vector2(20f, -2f)
			}; 

			GUI.backgroundColor = color;
			EditorGUILayout.BeginHorizontal(h, GUILayout.Height(25));
			GUI.backgroundColor = Color.white;
			{
				foldout = GUILayout.Toggle(foldout, new GUIContent(text), EditorStyles.foldout, GUILayout.Width(25));
				//Label (tex, texStyle, true);
			}
			EditorGUILayout.EndHorizontal();
		}

        #endregion

        #region Progress Bar

		public static void ProgressBar(float value, string name, bool showPorcentage)
		{

		     EditorGUI.ProgressBar(GUILayoutUtility.GetRect(0,20), value/100, showPorcentage ? name + value + "%" : name);
		}
			
		public static void ProgressBar(Rect rect, float value, string name, bool showPorcentage)
		{
			
			        EditorGUI.ProgressBar(rect, value/100f, showPorcentage ? name + value  + "%" : name);
		}
        #endregion

	}
}
#endif
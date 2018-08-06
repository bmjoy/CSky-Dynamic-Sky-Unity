////////////////////////////////////////////
/// AC Utility.
/// Name: Curve Range Drawer.
/// Description: Range for curves.
/// 
////////////////////////////////////////////

using UnityEngine; 
using UnityEditor;

namespace AC.Utility
{
	[CustomPropertyDrawer(typeof(AC_CurveRange))] 
	public class AC_CurveRangeDrawer : PropertyDrawer
	{

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			
			AC_CurveRange attr = attribute as AC_CurveRange;

			if(property.propertyType == SerializedPropertyType.AnimationCurve)
				EditorGUI.CurveField(rect, property, Color.white, new Rect(attr.timeStart, attr.valueStart, attr.timeEnd, attr.valueEnd));
			else
				EditorGUI.HelpBox(rect, "Only use with Animation Curves", MessageType.Warning);
			
		}
	}
}
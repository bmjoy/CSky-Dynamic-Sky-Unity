/* 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC.CSky
{

	[ExecuteInEditMode]
	[RequireComponent(typeof(CSky_SkySphere))]
	public class CSky_SkySphereCurvesAndGradients : MonoBehaviour 
	{


		#region |Components|

		[SerializeField] private CSky_SkySphere m_SkySphere;

		#endregion


		#region |Celestials|Background|

		[SerializeField] private bool m_EnableBackgroundGradient;
		[SerializeField] private CSky_EvaluateTimeMode m_BackgroundGradientEvaluateTime = CSky_EvaluateTimeMode.BySun;
		[SerializeField] private Gradient m_BackgroundGradient;

		#endregion


		void Awake()
		{

			m_SkySphere = GetComponent<CSky_SkySphere>();

		}


		void Update()
		{

			if(m_EnableBackgroundGradient)
				m_SkySphere.BackgroundColor = GetColor(m_BackgroundGradient, m_BackgroundGradientEvaluateTime);


		}


		Color GetColor(Gradient gradient, CSky_EvaluateTimeMode mode)
		{

			Color res = new Color();

			switch (mode)
			{
					
				case CSky_EvaluateTimeMode.BySun:                res = gradient.Evaluate(m_SkySphere.EvaluateTimeBySun); break;
				case CSky_EvaluateTimeMode.BySunAboveHorizon:    res = gradient.Evaluate(m_SkySphere.EvaluateTimeBySunAboveHorizon); break;
				case CSky_EvaluateTimeMode.BySunBellowHorizon:   res = gradient.Evaluate(m_SkySphere.EvaluateTimeBySunBellowHorizon); break;
				case CSky_EvaluateTimeMode.ByMoon:               res = gradient.Evaluate(m_SkySphere.EvaluateTimeByMoon); break;
				case CSky_EvaluateTimeMode.ByMoonAboveHorizon:   res = gradient.Evaluate(m_SkySphere.EvaluateTimeByMoonAboveHorizon); break;
				case CSky_EvaluateTimeMode.ByMoonBellowHorizon:  res = gradient.Evaluate(m_SkySphere.EvaluateTimeByMoonBellowHorizon); break;
			}


			return res;
		}



		float GetValue(AnimationCurve curve, CSky_EvaluateTimeMode mode)
		{

			float res = new float();

			switch (mode)
			{
					
				case CSky_EvaluateTimeMode.BySun:                res = curve.Evaluate(m_SkySphere.EvaluateTimeBySun); break;
				case CSky_EvaluateTimeMode.BySunAboveHorizon:    res = curve.Evaluate(m_SkySphere.EvaluateTimeBySunAboveHorizon); break;
				case CSky_EvaluateTimeMode.BySunBellowHorizon:   res = curve.Evaluate(m_SkySphere.EvaluateTimeBySunBellowHorizon); break;
				case CSky_EvaluateTimeMode.ByMoon:               res = curve.Evaluate(m_SkySphere.EvaluateTimeByMoon); break;
				case CSky_EvaluateTimeMode.ByMoonAboveHorizon:   res = curve.Evaluate(m_SkySphere.EvaluateTimeByMoonAboveHorizon); break;
				case CSky_EvaluateTimeMode.ByMoonBellowHorizon:  res = curve.Evaluate(m_SkySphere.EvaluateTimeByMoonBellowHorizon); break;
			}


			return res;
		}


	
	}
}

*/
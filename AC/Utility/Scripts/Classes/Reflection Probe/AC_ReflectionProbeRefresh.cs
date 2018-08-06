///////////////////////////////////////////////////////
/// AC Utility 
/// Name: Reflection Probe Refresh
/// Description: Simple utility for reflection probes.
/// 
///////////////////////////////////////////////////////

using System; 
using UnityEngine; 
using UnityEngine.Rendering;

namespace AC.Utility
{
	[AddComponentMenu("AC/Utility/Reflection Probe Refresh")]
	[RequireComponent(typeof(ReflectionProbe))]
	public class AC_ReflectionProbeRefresh : MonoBehaviour
	{

		//[SerializeField] 
		private ReflectionProbe m_Probe = null;

		[SerializeField] 
		private float m_UpdateInterval  = 1; 

		public float updateInterval 
		{
			get{ return m_UpdateInterval;  }
			set{ m_UpdateInterval = value; }
		}

		void Start()
		{
			m_Probe              = GetComponent<ReflectionProbe>();
			m_Probe.mode         = ReflectionProbeMode.Realtime;
			m_Probe.refreshMode  = ReflectionProbeRefreshMode.ViaScripting;
		}
			
		void Update()
		{
			if(m_Probe == null) return;
			UpdateProbe();
		}
			
		float m_Timer; 
		void UpdateProbe(RenderTexture rt = null)
		{

			float updateRate = 1.0f / m_UpdateInterval;
			m_Timer += Time.deltaTime;

			if(m_Timer >= updateRate) 
			{
				m_Probe.RenderProbe(rt);
				m_Timer = 0;
			}
		}
	}
}
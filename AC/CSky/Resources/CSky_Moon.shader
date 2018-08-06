////////////////////////////////////
/// CSky.
/// Name: Moon.
/// Description: Simple moon shader.
////////////////////////////////////

Shader "AC/CSky/Moon"
{
	Properties
	{
		
		_MainTex("Texture", 2D) = "white" {}
		//=================================================================

		_Saturation("Saturation", Float) = 1.0        // Moon exponent.
		//=================================================================

		CSky_HorizonFade("HorizonFade", Float) = 0.0  // Moon exctintion.
		//=================================================================
		
	}

	CGINCLUDE


		#include "CSky_Common.cginc" 
		//===============================

		sampler2D      _MainTex;
		float4         _MainTex_ST;
		//===============================

		#ifndef SHADER_API_MOBILE
		uniform float  _Saturation;
		#endif
		//===============================

		uniform float  _Intensity;
		uniform float4 _Color;
		//===============================
		
		
		struct v2f
		{
			float2 texcoord : TEXCOORD0;
			float3 normal   : TEXCOORD1;
			float3 worldPos : TEXCOORD2;
			float4 vertex   : SV_POSITION;

			UNITY_VERTEX_OUTPUT_STEREO 
		};


		v2f vert(appdata_base v)
		{

			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//=======================================================================

			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//=======================================================================

			o.vertex   = UnityObjectToClipPos(v.vertex);
			o.normal   = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
			//=======================================================================

			o.worldPos = SKY_SPHERE_WORLD_POS(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);  
			//=======================================================================

			return o;
		}

		half4 frag(v2f i) : SV_Target
		{

			float3 worldPos = normalize(i.worldPos);
			//=======================================================================

			// Simple lighting.
			half diff = saturate(max(0.0, dot(CSky_SunDirection, i.normal)) * 2.0); 
			//=======================================================================

			half4 color = tex2D(_MainTex, i.texcoord);
			//=======================================================================

			#ifndef SHADER_API_MOBILE
			if(_Saturation > 1.0)
			{
				color.rgb = pow(color.rgb, _Saturation);
			}
			#endif
			//=======================================================================

			color.rgb  *= _Color.rgb * _Intensity;
			//=======================================================================

			return (color * diff) * HORIZON_FADE(worldPos.y);
			//=======================================================================
		}
	ENDCG

	SubShader
	{

		Tags{"Queue"="Background+1555" "RenderType"="Background" "IgnoreProjector"="True"}
		//==============================================================================================

		Pass
		{

			ZWrite Off 
			ZTest LEqual 
			//Blend One One 
			//Blend SrcAlpha OneMinusSrcAlpha 
			Fog{ Mode Off }
			//==========================================================================================
		
			CGPROGRAM

				#pragma target   2.0
				#pragma vertex   vert
				#pragma fragment frag

			ENDCG

			//==========================================================================================
		}
	}
}

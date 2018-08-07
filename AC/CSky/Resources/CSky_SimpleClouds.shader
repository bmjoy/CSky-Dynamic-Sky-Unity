//////////////////////////////////
/// CSky.
/// Moon.
/// Description: Moon Shader.
//////////////////////////////////

Shader "AC/CSky/Simple Clouds"
{
	Properties
	{
		
		_NoiseTex("Noise Texture", 2D) = "white" {}
		//=================================================================

		_NoiseTex2("Noise Texture 2", 2D) = "white" {}
		//=================================================================

		_Color("Color", Color) = (1,1,1,1)
		//=================================================================

		_Intensity("Intensity", Float) = 1.0
		//=================================================================

		_Coverage("Coverage", Float) = 1.0      
		//=================================================================

		_Scale("Scale", Float) = 0.05
		//=================================================================

		_Speed("Speed", Vector) = (0.5,0.1,0.3,0.05)
		//=================================================================

		CSky_HorizonFade("HorizonFade", Float) = 0.0 
		//=================================================================
		
	}

	CGINCLUDE


		#include "CSky_Common.cginc" 
		#include "CSky_AtmosphereCommon.cginc"
		//===============================

		sampler2D      _NoiseTex;
		float4         _NoiseTex_ST;
		//===============================

		sampler2D      _NoiseTex2;
		float4         _NoiseTex2_ST;
		//===============================

		uniform float  _Coverage;

		//===============================

		uniform float  _Intensity;
		uniform float4 _Color;
		//===============================


		uniform float  _Scale;

		//===============================

		uniform float4  _Speed;

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

			o.worldPos = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
			o.worldPos.y += 0.3;
			o.texcoord = o.worldPos.xz / o.worldPos.y; //(o.vertex.xy / o.vertex.w + 1) * 0.5;
			o.texcoord *= _Scale;
			//=======================================================================

			return o;
		}

		half4 frag(v2f i) : SV_Target
		{

			float3 worldPos = normalize(i.worldPos);
			//=======================================================================

			
			half4 nt = tex2D(_NoiseTex, i.texcoord + (_Time.xx * _Speed.xy));
			half4 nt2 = tex2D(_NoiseTex2, i.texcoord + (_Time.xx * _Speed.zw));
		

		
			half4 col = (nt + nt2)*0.5;


			col = saturate(col);

				
			half a = pow(col.a, _Coverage) * HORIZON_FADE(worldPos.y);

			col.rgb *=  _Intensity;

			//ColorCorrection(col.rgb);
			//=======================================================================

			return half4(col.rgb*_Color,a);
			//=======================================================================
		}
	ENDCG

	SubShader
	{

		Tags{"Queue"="Background+1600" "RenderType"="Background" "IgnoreProjector"="True"}
		//==============================================================================================

		Pass
		{

			Cull Front 
			ZWrite Off 
			ZTest LEqual  
			Blend SrcAlpha OneMinusSrcAlpha 
			Fog{ Mode Off }
			//==========================================================================================
		
			CGPROGRAM

				#pragma target   2.0
				#pragma vertex   vert
				#pragma fragment frag
				#pragma multi_compile __ CSky_GAMMA_COLOR_SPACE

			ENDCG

			//==========================================================================================
		}
	}
}

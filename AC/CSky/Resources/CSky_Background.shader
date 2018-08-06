////////////////////////////////////
/// CSky.
/// Name: Background.
/// Description: Background shader.
////////////////////////////////////

Shader "AC/CSky/Background"
{
	Properties
	{

		[NoScaleOffset] _Cubemap("Cubemap", Cube) = "grey" {}
		//=========================================================

		_Saturation("Saturation", Float) = 1
		//=========================================================

		CSky_HorizonFade("Horizon Fade", Float) = 0
		//=========================================================

	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "CSky_Common.cginc"
		//===========================

		uniform samplerCUBE _Cubemap;
		half4 _Cubemap_HDR;

		//=========================================================

		uniform float3 _Color;
		uniform float  _Intensity;

		#ifndef SHADER_API_MOBILE
		uniform float  _Saturation;
		#endif
		//=========================================================


		struct appdata_t
		{
            float4 vertex : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 vertex   : SV_POSITION;
            float3 texcoord : TEXCOORD0;
            float3 worldPos : TEXCOORD1;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert (appdata_t v)
        {
            v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//=======================================================================================

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//=======================================================================================

            o.vertex = UnityObjectToClipPos(v.vertex);
			//=======================================================================================
           
            o.texcoord = v.vertex.xyz;
            o.worldPos = normalize(mul((float3x3)unity_ObjectToWorld, normalize(v.vertex.xyz)));
			//=======================================================================================

            return o;
			//=======================================================================================
        }

        half4 frag (v2f i) : SV_Target
        {

        	half4 cube   = texCUBE(_Cubemap, i.texcoord);
            half3 color  = DecodeHDR(cube, _Cubemap_HDR);
			//=======================================================================================

			#ifndef SHADER_API_MOBILE
        	if(_Saturation > 1.0)
			{ 
				color = pow(color, _Saturation);
			}
			#endif
			//=======================================================================================

            color.rgb *= _Color * _Intensity;
            color *= unity_ColorSpaceDouble.rgb;
			//=======================================================================================

			//ColorCorrection(color.rgb);
			//=======================================================================================

            return half4(color.rgb, 1.0) * HORIZON_FADE(normalize(i.worldPos).y);
			//=======================================================================================
        }	
	ENDCG

	SubShader
	{

		Tags{ "Queue"="Background+1525" "RenderType"="Background" "IgnoreProjector"="True" }
		//====================================================================================

		Pass
		{

			Cull Front 
			ZWrite Off 
			ZTest LEqual 
			Fog{ Mode Off }
			//================================================================================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0

				//#pragma multi_compile __ CSky_GAMMA_COLOR_SPACE

			ENDCG

			//================================================================================
		}
	}
}

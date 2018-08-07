/////////////////////////////////////////
/// CSky.
/// Name: Preetham Skybox.
/// Description: 
/// Skybox with only rayleigh scattering.
/////////////////////////////////////////

Shader "AC/CSky/Preetham Skybox"
{
	Properties
	{
		CSky_GroundFade("Ground Fade", Range(0,100)) = 30
		CSky_GroundAltitude("Ground Altitude", Range(0, 0.1)) = 0.01	
	}

	CGINCLUDE


		#include "UnityCG.cginc"
		#include "CSky_PreethamAtmosphericScattering.cginc"
		//=================================================

		struct appdata
		{
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID 
		};

		struct v2f
		{
			half3 color   : TEXCOORD0;
			float4 vertex : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO 
		};


		v2f vert(appdata v)
		{
			v2f o;
	 		UNITY_INITIALIZE_OUTPUT(v2f, o);
			//================================================================================================================

	 		UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//================================================================================================================

			o.vertex = UnityObjectToClipPos(v.vertex);
			//================================================================================================================

			float3 ray = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
			//================================================================================================================

			half3 sR; half3 sM;

			OpticalDepth(abs(ray.y+CSky_HorizonOffset), sR, sM);
			//================================================================================================================

			float2 cosTheta  = float2(dot(ray, CSky_SunDirection.xyz), dot(ray, CSky_MoonDirection.xyz));
			//================================================================================================================

			o.color.rgb  = RayleighScattering(ray, sR, sM, cosTheta);
			//================================================================================================================

			o.color.rgb = ATMOSPHERE_EXPONENT(o.color.rgb);
			//================================================================================================================

			ColorCorrection(o.color, CSky_GroundColor);
			//================================================================================================================

			o.color.rgb = lerp(o.color.rgb, CSky_GroundColor, saturate((-ray.y - CSky_GroundAltitude) * CSky_GroundFade));
			//================================================================================================================

			return o;
			//================================================================================================================
		}

		half4 frag(v2f i) : SV_Target
		{
			return half4(i.color.rgb, 1);
			//============================
		}

	ENDCG

	SubShader
	{

		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
		//==============================================================================

		Pass
		{

			Cull Off 
			ZWrite Off 
			//=========================================================
		
			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0
		 
				#pragma multi_compile __ CSky_HDR
				#pragma multi_compile __ CSky_GAMMA_COLOR_SPACE

			ENDCG

			//=========================================================
		}
	}
}

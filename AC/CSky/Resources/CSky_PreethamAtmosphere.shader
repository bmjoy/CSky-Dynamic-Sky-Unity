////////////////////////////////////////////////////////////
/// CSky.
/// Name: Preetham Atmosphere.
/// Description: Atmosphere with perspective from the earth.
////////////////////////////////////////////////////////////

Shader "AC/CSky/Preetam Atmosphere"
{
	Properties
	{
		CSky_DitheringTex("BlueNoise", 2D) = "white" {}
		CSky_DitheringTexSize("BlueNoiseSize", Float) = 1.0 
		CSky_DitheringTexSpeed("BlueNoiseSpeed", Float) = 1.0 
	}

	CGINCLUDE


		#include "UnityCG.cginc"
		#include "CSky_PreethamAtmosphericScattering.cginc"
		//================================================

		struct appdata
		{
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID 
		};

		struct v2f
		{

			float3 worldPos : TEXCOORD0;

			#ifdef CSky_PER_PIXEL_ATMOSPHERE

			half3  sR       : TEXCOORD1;
			half3  sM       : TEXCOORD2;

			#else

			half3 color   : TEXCOORD1;

			#endif

			float2 texcoord: TEXCOORD3;

			float4 vertex : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO 
		};


		v2f vert(appdata_base v)
		{
			v2f o;
	 		UNITY_INITIALIZE_OUTPUT(v2f, o);
			//=============================================================================================================================

	 		UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//=============================================================================================================================

			o.vertex = UnityObjectToClipPos(v.vertex);
			//=============================================================================================================================

				o.texcoord = v.texcoord.xy;

			#if defined(CSky_PER_PIXEL_ATMOSPHERE)

				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
				//=========================================================================================================================

				//float3 ray = normalize(o.worldPos.xyz);
				//OpticalDepth(ray.y, o.sR, o.sM);

			#else

				float3 ray = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
				//=========================================================================================================================

				half3 sR; half3 sM;
				//=========================================================================================================================

				OpticalDepth(abs(ray.y+CSky_HorizonOffset), sR, sM);
				//=========================================================================================================================

				float2 cosTheta = float2(dot(ray, CSky_SunDirection.xyz),dot(ray, CSky_MoonDirection.xyz));
				//=========================================================================================================================

				o.color.rgb  = AtmosphericScattering(ray, sR, sM, cosTheta);
				//=========================================================================================================================

				//o.color.rgb += MiePhaseSimplified(moonCosTheta, CSky_MoonBetaMiePhase, CSky_MoonMieScattering, CSky_MoonMieColor);
				//=========================================================================================================================

				o.color.rgb = ATMOSPHERE_EXPONENT(o.color.rgb);
				//=========================================================================================================================

				
				ColorCorrection(o.color.rgb);
				//=========================================================================================================================


			#endif

		

			return o;
			//============================================================================================================================
		}


		half4 frag(v2f i) : SV_Target
		{

			half4 color = half4(0.0, 0.0, 0.0, 1.0);
			//===================================================================================================

			float3 ray = normalize(i.worldPos.xyz);

			
			//===================================================================================================

			#if defined(CSky_PER_PIXEL_ATMOSPHERE)

				
				OpticalDepth(abs(ray.y+CSky_HorizonOffset), i.sR, i.sM);
				//===============================================================================================

				float2 cosTheta = float2(dot(ray, CSky_SunDirection.xyz), dot(ray, CSky_MoonDirection.xyz));
				//===============================================================================================

				color.rgb  = AtmosphericScattering(ray, i.sR, i.sM, cosTheta);
				//===============================================================================================

				color.rgb = ATMOSPHERE_EXPONENT(color);
				//===============================================================================================

				ColorCorrection(color.rgb);
				//===============================================================================================

			#else

				color.rgb += i.color.rgb;
				//===============================================================================================

			#endif
			
		
			return color;
			//===================================================================================================
		}


	ENDCG

	SubShader
	{

		Tags {"Queue"="Background+1600" "RenderType"="Background" "IgnoreProjector"="True"}
		//===================================================================================================

		Pass
		{

			Cull Front 
			ZWrite Off 
			ZTest 
			LEqual 
			Blend One One 
			Fog{ Mode Off }
			//===============================================================================================
		
			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0
		 
				#pragma multi_compile __ CSky_HDR
				#pragma multi_compile __ CSky_GAMMA_COLOR_SPACE
				#pragma multi_compile __ CSky_PER_PIXEL_ATMOSPHERE

			ENDCG

			//===============================================================================================
		}
	}
}

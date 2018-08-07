////////////////////////////////////////////////////////////
/// CSky.
/// Name: Defautl Atmosphere.
/// Description: Atmosphere with perspective from the earth.
////////////////////////////////////////////////////////////

Shader "AC/CSky/Defautl Atmosphere"
{
	Properties
	{

		
	}

	CGINCLUDE


		#include "UnityCG.cginc"
		#include "CSky_DefautlAtmosphericScattering.cginc"
		//==============================================

		struct appdata
		{
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID 
		};

		struct v2f
		{

			#if defined(CSky_PER_PIXEL_ATMOSPHERE)

			float3 worldPos   : TEXCOORD0;
			half3  inscatter  : TEXCOORD1;
			half4  outscatter : TEXCOORD2;

			#else

			half3 color : TEXCOORD0;

			#endif

			float4 vertex : SV_POSITION;

			UNITY_VERTEX_OUTPUT_STEREO 
		};


		v2f vert(appdata v)
		{
			v2f o;
	 		UNITY_INITIALIZE_OUTPUT(v2f, o);
			//====================================================================================================================================

	 		UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//====================================================================================================================================

			o.vertex = UnityObjectToClipPos(v.vertex);
			//====================================================================================================================================

			#if defined(CSky_PER_PIXEL_ATMOSPHERE)

				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
				float3 ray = normalize(o.worldPos);
				//================================================================================================================================
				
				AtmosphericScattering(float3(ray.x, abs(ray.y+CSky_HorizonOffset), ray.z), o.inscatter, o.outscatter, 1);
				//================================================================================================================================

			#else

				float3 ray = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
				//================================================================================================================================

				half3 inscatter; half4 outscatter;

				AtmosphericScattering(float3(ray.x, abs(ray.y+CSky_HorizonOffset), ray.z), inscatter, outscatter, 1);
				//================================================================================================================================

				float  sunCosTheta  = dot(ray, CSky_SunDirection.xyz);
				float  moonCosTheta = dot(ray, CSky_MoonDirection.xyz); 
				//================================================================================================================================

				o.color.rgb  = inscatter;
				o.color.rgb += MiePhase(sunCosTheta, CSky_SunBetaMiePhase, CSky_SunMieScattering, CSky_SunMieColor) * outscatter.rgb;
				o.color.rgb += MiePhaseSimplified(moonCosTheta, CSky_MoonBetaMiePhase, CSky_MoonMieScattering, CSky_MoonMieColor) * outscatter.a;
				//================================================================================================================================

				o.color.rgb = ATMOSPHERE_EXPONENT(o.color.rgb);
				//================================================================================================================================

				ColorCorrection(o.color.rgb);
				//================================================================================================================================

			#endif

			return o;
		}


		half4 frag(v2f i) : SV_Target
		{

			half4 color = half4(0.0, 0.0, 0.0, 1.0);
			//====================================================================================================================================

			#if defined(CSky_PER_PIXEL_ATMOSPHERE)

				float3 ray = normalize(i.worldPos);
				//================================================================================================================================

				float  sunCosTheta  = dot(ray, CSky_SunDirection.xyz);
				float  moonCosTheta = dot(ray, CSky_MoonDirection.xyz); 
				//================================================================================================================================

				color.rgb  = i.inscatter;
				color.rgb += MiePhase(sunCosTheta, CSky_SunBetaMiePhase, CSky_SunMieScattering, CSky_SunMieColor) * i.outscatter.rgb;
				color.rgb += MiePhaseSimplified(moonCosTheta, CSky_MoonBetaMiePhase, CSky_MoonMieScattering, CSky_MoonMieColor) * i.outscatter.a;
				//================================================================================================================================

				color.rgb = ATMOSPHERE_EXPONENT(color.rgb);
				//================================================================================================================================
				
				ColorCorrection(color.rgb);
				//================================================================================================================================

			#else

				color.rgb = i.color.rgb;
				//================================================================================================================================

			#endif

			return color;
			//====================================================================================================================================
		}


	ENDCG

	SubShader
	{

		Tags {"Queue"="Background+1600" "RenderType"="Background" "IgnoreProjector"="True"}
		//=====================================================================================

		Pass
		{

			Cull Front 
			ZWrite Off 
			ZTest LEqual
			Blend One One 
			Fog{ Mode Off }
			//================================================================================================================================

			CGPROGRAM

				#pragma vertex   vert
				#pragma fragment frag
				#pragma target 2.0
		 
				#pragma multi_compile __ CSky_HDR
				#pragma multi_compile __ CSky_GAMMA_COLOR_SPACE
				#pragma multi_compile __ CSky_PER_PIXEL_ATMOSPHERE


			ENDCG

			//================================================================================================================================
		}
	}
}

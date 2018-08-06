//////////////////////////////////
/// CSky.
/// Sun.
/// Description: Sun Shader.
//////////////////////////////////


Shader "AC/CSky/Sun"
{
	Properties
	{


		_MainTex("Texture", 2D) = "white" {}
		//==============================================

		[Toggle(CSky_PROCEDURAL_PARTICLE_SPOT)]
		_ProceduralSpot("Procedural Spot", Float) = 0
		//==============================================

		_DiscSize("DiscSize", Float) = 0.15
		_GlowSize("GlowSize", Float) = 0.5
		//==============================================

		
		CSky_HorizonFade("HorizonFade", Float) = 0.0
		//==============================================

	}

		
	SubShader
	{

		Tags{"Queue"="Background+1545" "RenderType"="Background" "IgnoreProjector"="True"}
		//=====================================================================================

		CGINCLUDE

			#include "CSky_StarsCommon.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float4 vertex   : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};


			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				//============================================

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				//============================================

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = SKY_SPHERE_WORLD_POS(v.vertex);
				//============================================

				#if defined(CSky_PROCEDURAL_PARTICLE_SPOT)
					o.texcoord = v.texcoord - 0.5;
				#else
					o.texcoord = v.texcoord;
				#endif
				//============================================

				return o;
				//============================================
			}
		

			half4 frag(v2f i) : SV_Target
			{

				half3 color = half3(0.0, 0.0, 0.0);
				//===================================================================

				#if defined(CSky_PROCEDURAL_PARTICLE_SPOT)

					color = DiscGlow(i.texcoord, _DiscSize, _GlowSize);

				#else

					color = tex2D(_MainTex, i.texcoord );

				#endif
				//===================================================================

				color *= _Color * _Intensity;
				//===================================================================

				ColorCorrection(color);
				//===================================================================

				return half4(color, 1.0) * HORIZON_FADE(normalize(i.worldPos).y);
				//===================================================================
			}
		
		ENDCG
		

		Pass
		{
			Cull Off 
			ZWrite Off 
			ZTest LEqual 
			Blend One One 
			Fog{ Mode Off } 
			// ColorMask RGB
			//=================================================================================

			CGPROGRAM

				#pragma target   2.0
				#pragma vertex   vert
				#pragma fragment frag
				#pragma multi_compile  __ CSky_PROCEDURAL_PARTICLE_SPOT
				#pragma multi_compile  __ CSky_GAMMA_COLOR_SPACE

			ENDCG

			//=================================================================================
		}
	}
}

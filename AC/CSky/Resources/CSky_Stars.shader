//////////////////////////////////
/// CSky.
/// Stars.
/// Description: Stars Shader.
//////////////////////////////////

Shader "AC/CSky/Stars"
{
	Properties
	{
		

		_MainTex("Texture", 2D) = "white" {}
		//==============================================

	    _NoiseTex("Texture", 2D)        = "white" {}
		_NoiseSize("Noise Size", Float) = 0.5
		//==============================================

		[Toggle(CSky_PROCEDURAL_PARTICLE_SPOT)]
		_ProceduralSpot("Procedural Spot", Float) = 0

		//_DiscSize("Disc Size", Float) = 0.15
		_GlowSize("Glow Size", Float) = 0.5
		//==============================================

		CSky_HorizonFade("Horizon Fade", Float) = 0.0
		//==============================================
	}

	CGINCLUDE

		#include "CSky_StarsCommon.cginc"
		//================================


		// Noise.
		uniform sampler2D _NoiseTex;
		float4 _NoiseTex_ST;

		uniform half _NoiseSize;
		//================================


		// Scintillation.
		uniform half _ScintillationSpeed;
		uniform half _Scintillation;
		//================================


		struct appdata_t
		{
			float4 vertex   : POSITION;
			half4  color    : COLOR;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		//==================================================================================


		struct v2f
		{

			half4  color    : COLOR;
			float2 texcoord : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
			float2 noiseCoords : TEXCOORD2;
			float4 vertex   : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		//==================================================================================


		v2f vert(appdata_t v)
		{

			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//============================================

			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//============================================

			o.vertex   = UnityObjectToClipPos(v.vertex);
			o.worldPos = SKY_SPHERE_WORLD_POS(v.vertex);
			//============================================

			#if defined(CSky_PROCEDURAL_PARTICLE_SPOT)
				o.texcoord = v.texcoord - 0.5;
			#else
				o.texcoord = v.texcoord;
			#endif
			//============================================

			o.noiseCoords = (o.worldPos.xz / (o.worldPos.y+0.03) * _NoiseSize) + _Time.xx * _ScintillationSpeed;

			o.color = v.color;
			//============================================

			return o;
			//============================================
		}

		half4 frag(v2f i) : SV_Target
		{

			half3 color = half3(0.0, 0.0, 0.0);
			//===================================================================

			#if defined(CSky_PROCEDURAL_PARTICLE_SPOT)

				half g = clamp(_GlowSize * i.color.a, 0.35, 1);

				color = Glow(i.texcoord, g);

			#else

				color =  tex2D(_MainTex,  i.texcoord.xy ) ;

			#endif
			//===================================================================

			color *= i.color.rgb * i.color.a;

			//===================================================================

			color *= _Color * _Intensity;
			//===================================================================

			color = lerp(color, color * tex2D(_NoiseTex, i.noiseCoords), _Scintillation);
			//===================================================================

			ColorCorrection(color);
			//===================================================================

			return half4(color, 1.0) * HORIZON_FADE(normalize(i.worldPos).y);
			//===================================================================
		}

/*
         v2f passThrough(float4 color : COLOR)
         {
             v2f o;

             o.color = color;

             return o;
         }*/


	ENDCG

	SubShader
	{

		Tags{"Queue"="Background+1550" "RenderType"="Background" "IgnoreProjector"="True"}
		//==================================================================================

		Pass
		{

			Cull Back
			ZWrite Off 
			ZTest LEqual 
			Blend  One One 
			Fog{ Mode Off } 
			//ColorMask RGB
			//BindChannels 
			//{
			//	Bind "Vertex", vertex
			//	Bind "texcoord", texcoord
			//	Bind "Color", color
			//}

			//===============================================================================

			CGPROGRAM



				#pragma target 2.0
				#pragma vertex   vert
				#pragma fragment frag

				#pragma multi_compile __ CSky_PROCEDURAL_PARTICLE_SPOT
				#pragma multi_compile __ CSky_GAMMA_COLOR_SPACE

			ENDCG

			//================================================================================
		}
	}
}

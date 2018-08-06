//////////////////////////////////
/// CSky.
/// Name: Common.
/// Description: Common for CSky.
//////////////////////////////////

#ifndef CSKY_COMMON_INCLUDED
#define CSKY_COMMON_INCLUDED

#include "UnityCG.cginc"
//==============================================


// Matrices.
uniform float4x4 CSky_WorldToLocal;
uniform float4x4 CSky_LocalToWorld;
//==============================================

// Light direction.
uniform float3 CSky_SunDirection;
uniform float3 CSky_MoonDirection;
//==============================================

// Horizon.
uniform half CSky_HorizonFade;
//==============================================

// HDR.
uniform half CSky_Exposure;
//==============================================

// Ground.
uniform half3 CSky_GroundColor;
uniform half  CSky_GroundFade;
uniform half  CSky_GroundAltitude;
//==============================================

// Dithering.
uniform sampler2D CSky_DitheringTex;
uniform float     CSky_DitheringTexSize;
uniform half      CSky_DitheringTexSpeed;
//==============================================

// Matrix.
#define SKY_SPHERE_WORLD_POS(vertex) mul(CSky_WorldToLocal, mul(unity_ObjectToWorld, vertex)).xyz; 
//===================================================================================================

// Horizon.
#define HORIZON_FADE(dir) saturate((dir - CSky_HorizonFade) * 10);
//===================================================================================================

// Color Correction.
#define DESATURATE(color) (color.r + color.g + color.b) * 0.33333333h  
//===================================================================================================

#define FAST_TONEMAPING(color) 1.0 - exp(CSky_Exposure * -color)
//===================================================================================================

#ifdef SHADER_API_MOBILE
	#define GAMMA_TO_LINEAR(color) sqrt(color)          
#else
	#define GAMMA_TO_LINEAR(color) pow(color, 0.45454545h)
#endif
//===================================================================================================


inline void ColorCorrection(inout half3 color)
{

	color *= CSky_Exposure;
	//=====================================

	#if !defined(CSky_HDR)
		color = FAST_TONEMAPING(color);
	#endif
	//=====================================

	#if defined(UNITY_COLORSPACE_GAMMA)
		color = GAMMA_TO_LINEAR(color);
	#endif
	//=====================================

}
//===================================================================================================

inline void ColorCorrection(inout half3 color, inout half3 groundColor)
{

	color *= CSky_Exposure;
	//=====================================

	#if !defined(CSky_HDR)
		color = FAST_TONEMAPING(color);
	#endif
	//=====================================

	#if defined(UNITY_COLORSPACE_GAMMA)
		color = GAMMA_TO_LINEAR(color);
	#else
		groundColor *= groundColor;
	#endif
	//=====================================
}
//===================================================================================================


// Dithering.

// See: http://momentsingraphics.de/?p=127
inline float3 BlueNoiseDithering(float2 uv)
{

	float3 blueNoise = tex2D(CSky_DitheringTex, (uv.xy * CSky_DitheringTexSize) + _Time.xx * CSky_DitheringTexSpeed).rgb* 2.0 - 1.0;

	//blueNoise = mad(blueNoise, 2.0, 1.0);

	blueNoise = sign(blueNoise) * (1.0 - sqrt(1.0 - abs(blueNoise)));

	return blueNoise / 255;
}
//===================================================================================================

// coords = pos.xy/pos.w 
inline float3 ScreenSpaceDithering(float2 coords)
{

	coords *= CSky_DitheringTexSize;
	coords += _Time.xx * CSky_DitheringTexSpeed;

	float4 d = dot(float2(171, 231), coords);
	d.rgb = frac(d / float3(103, 71, 97));
	d.rgb -= 0.5;

	return d / 255;
}
//===================================================================================================


#endif
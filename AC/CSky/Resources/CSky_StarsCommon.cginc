//////////////////////////////////
/// CSky.
/// Stars common.
/// Description: Common for stars.
//////////////////////////////////

#ifndef CSKY_STARS_COMMON_INCLUDED
#define CSKY_STARS_COMMON_INCLUDED

#include "UnityCG.cginc"
#include "CSky_Common.cginc"
//=============================

// Star texture.
uniform sampler2D _MainTex;
float4 _MainTex_ST;
//================================

// General params.
uniform float  _Intensity;
uniform float4 _Color;
//uniform float  _Saturation;
//================================

// Procedural spot.
uniform float _DiscSize;
uniform float _GlowSize;
//================================


// Procedural Spot.
//=================


// Simple disc.
inline half Disc(float2 coords, half size)
{
	half dist = length(coords);
	return 1.0 - step(size, dist);
}

inline half3 Disc(float2 coords, half size, half3 color)
{
	half dist = length(coords);
	return (1.0 - step(size, dist)) * color;
}
//=====================================================================================================================================


// Glow.
inline half Glow(float2 coords, half size)
{
	half dist = length(coords);
	return (1.0 - smoothstep(0.0, size, dist));
}

inline half3 Glow(float2 coords, half size, half3 color)
{
	half dist = length(coords);
	return (1.0 - smoothstep(0.0, size, dist)) * color;
}
//=====================================================================================================================================


// Disc + glow.
inline half3 DiscGlow(float2 coords, half spotSize, half glowSize)
{
	half dist = length(coords);
	return (1.0 - step(spotSize, dist)) + (1.0 - smoothstep(0.0, glowSize, dist));
}

inline half3 DiscGlow(float2 coords, half spotSize, half glowSize, half3 color)
{
	half dist = length(coords);
	return pow(((1.0 - smoothstep(0.0, spotSize, dist))), 3.5) + pow(((1.0 - smoothstep(0.0, glowSize, dist)) * color*0.5), 2.5);
}
//=====================================================================================================================================


#endif
////////////////////////////////////////
/// CSky.
/// Name: Atmosphere common.
/// Description: Common for atmosphere.
////////////////////////////////////////


#ifndef CSKY_ATMOSPHERE_COMMON_INCLUDED
#define CSKY_ATMOSPHERE_COMMON_INCLUDED


#include "UnityCG.cginc"
//#include "CSky_Common.cginc"
//==============================================


// Sun.
uniform float3 CSky_SunBetaMiePhase;
uniform half   CSky_SunMieScattering;
uniform half3  CSky_SunMieColor;
//==============================================

// Moon.
uniform int    CSky_MoonRayleigh;
uniform float3 CSky_MoonBetaMiePhase;
uniform half   CSky_MoonMieScattering;
uniform half3  CSky_MoonMieColor;
//==============================================

// Horizon.
uniform float CSky_HorizonOffset;
//==============================================

// Tint.
uniform half3 CSky_SunAtmosphereTint;
uniform half4 CSky_MoonAtmosphereTint;
//==============================================

// FadeParams.
uniform float3 CSky_FadeParams;
//==============================================

// Exponent.
uniform half CSky_AtmosphereExponent;
//==============================================

// PI.
#define CSky_PI14  0.079577f  // 1 / (4*pi).
#define CSky_PI316 0.059683f  // 3 / (16 * pi).
//==============================================

// Exponent.
#define ATMOSPHERE_EXPONENT(color) CSky_AtmosphereExponent > 1.0 ? pow(color, CSky_AtmosphereExponent) : color
//======================================================================


// Henyey Greenstein.
//===================

// Simplified Henyey Greenstein phase function for moon.
inline half3 MiePhaseSimplified(float cosTheta, float3 betaMiePhase, half scattering, half3 color)
{
	return (CSky_PI14 * (betaMiePhase.x / (betaMiePhase.y - (betaMiePhase.z * cosTheta)))) * scattering *  color;
}
//======================================================================================================================


// Cornette Sharks Henyey Greenstein phase function with small changes.
inline half3 MiePhase(float cosTheta, float3 betaMiePhase, half scattering, half3 color)
{
	return (1.5 * betaMiePhase.x * ((1.0 + cosTheta * cosTheta) *
		pow(betaMiePhase.y - (betaMiePhase.z * cosTheta), -1.5))) * scattering * color;
}
//======================================================================================================================


#endif
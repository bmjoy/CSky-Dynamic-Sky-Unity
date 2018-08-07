/////////////////////////////////////////////////////////////////
/// CSky.
/// Name: Preetham Atmospheric Scattering.
/// Description: 
///  Atmospheric scattering based on Preetham And Holfman papers, 
///  
/////////////////////////////////////////////////////////////////

#ifndef CSKY_PREETHAM_ATMOSPHERIC_SCATTERING_INCLUDED
#define CSKY_PREETHAM_ATMOSPHERIC_SCATTERING_INCLUDED

#include "UnityCG.cginc"
#include "CSky_Common.cginc"
#include "CSky_AtmosphereCommon.cginc"
//========================================


uniform half3 CSky_BetaRay;
uniform half3 CSky_BetaMie;
uniform float CSky_SunE;
//========================================

uniform float CSky_RayleighZenithLength;
uniform float CSky_MieZenithLength;
//========================================

//#define RAYLEIGH_ZENITH_LENGTH 8.4e3
//#define MIE_ZENITH_LENGTH 1.25e3 


inline float RayleighPhase(float cosTheta)
{
	return CSky_PI316 * (1.0 + (cosTheta * cosTheta));
	//return  (3.0 / 4.0) * (1.0 + (cosTheta * cosTheta));
}
//=================================================================================================


// Defautl optical depth.
inline void OpticalDepth(half dir, inout half3 sr, inout half3 sm)
{

	float h = saturate((dir));
	float3 zenith = acos(h);
	zenith        = (cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180.0) / UNITY_PI), -1.253));

	sr = CSky_RayleighZenithLength / zenith;
	sm = CSky_MieZenithLength      / zenith;
}
//=================================================================================================

/*
// Based in Nielsen paper, See Documentation: References.
inline void OpticalDepth(float ray, inout float3 sr, inout float3 sm)
{

	float h = saturate((ray));
	float f = pow(h, 0.25); // h 1 / 5.0
	float t = (1.05 - f) * 190000;

	sr = t + f * (CSky_RayleighZenithLength - t);
	sm = t + f * (CSky_MieZenithLength - t);
}*/
//=================================================================================================

inline half3 AtmosphericScattering(float3 ray, half3 sr, half3 sm, float2 cosTheta)
{

	half3 fex = exp(-(CSky_BetaRay * sr + CSky_BetaMie * sm)); // Combined extinction factor.
	half3 finalFex = saturate(lerp(1.0 - fex, (1.0 - fex) * (fex), (CSky_FadeParams.z)));
	//==================================================================================================================

	half3 sunBRT = RayleighPhase(cosTheta.x) * CSky_BetaRay;
	half3 moonBRT = RayleighPhase(cosTheta.y) * CSky_BetaRay;
	//==================================================================================================================

	half3 sunMiePhase = MiePhase(cosTheta.x, CSky_SunBetaMiePhase, CSky_SunMieScattering, CSky_SunMieColor);
	half3 sunBMT = (sunMiePhase * CSky_BetaMie) * CSky_SunMieColor * finalFex.r;
	//==================================================================================================================

	half3 moonMiePhase = MiePhaseSimplified(cosTheta.y, CSky_MoonBetaMiePhase, CSky_MoonMieScattering, CSky_MoonMieColor);
	half3 moonBMT = (moonMiePhase * CSky_BetaMie);
	//==================================================================================================================

	half3 SUN_BRMT = (sunBRT + sunBMT) / (CSky_BetaRay + CSky_BetaMie);
	half3 MOON_BRMT = (moonBRT + moonBMT) / (CSky_BetaRay + CSky_BetaMie);
	//==================================================================================================================

	half3 inscatter = (CSky_SunE * CSky_FadeParams.x) * (SUN_BRMT * finalFex)* CSky_SunAtmosphereTint;
	//==================================================================================================================

	half  nightIntensity = CSky_SunE * CSky_FadeParams.y;
	half3 nightInscatter = nightIntensity * (MOON_BRMT * (1.0 - fex)) * CSky_MoonAtmosphereTint.rgb;
	//==================================================================================================================

	return (inscatter + nightInscatter) * 0.5;
	//==================================================================================================================
}
//=======================================================================================================================


inline half3 RayleighScattering(float3 ray, half3 sr, half3 sm, float2 cosTheta)
{

	half3 fex = exp(-(CSky_BetaRay * sr + CSky_BetaMie * sm)); // Combined extinction factor.
	half3 finalFex = saturate(lerp(1.0 - fex, (1.0 - fex) * (fex), CSky_FadeParams.z));
	//==================================================================================================================

	// Rayleigh.
	half3 sunBRT = RayleighPhase(cosTheta.x) * CSky_BetaRay;
	half3 moonBRT = RayleighPhase(cosTheta.y) * CSky_BetaRay;
	//==================================================================================================================

	half3 SUN_BRMT = sunBRT / CSky_BetaRay;
	half3 MOON_BRMT = moonBRT / CSky_BetaRay;
	//==================================================================================================================

	half3 inscatter = (CSky_SunE * CSky_FadeParams.x) * (SUN_BRMT * finalFex)* CSky_SunAtmosphereTint;
	//==================================================================================================================

	half  nightIntensity = CSky_SunE * CSky_FadeParams.y;
	half3 nightInscatter = nightIntensity * (MOON_BRMT * (1.0 - fex)) * CSky_MoonAtmosphereTint.rgb;
	//==================================================================================================================

	return (inscatter + nightInscatter) * 0.5;
	//==================================================================================================================
}

#endif
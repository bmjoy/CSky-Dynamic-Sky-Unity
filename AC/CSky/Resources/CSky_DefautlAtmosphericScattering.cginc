/////////////////////////////////////////////////////////////
/// CSky.
/// Name: Defautl Atmospheric Scattering.
/// Description: 
///  Atmospheric scattering based on Sean Oneil work, 
///  GPU Gems2 and Nisita papers.
/////////////////////////////////////////////////////////////

#ifndef CSKY_DEFAUTL_ATMOSPHERIC_SCATTERING_INCLUDED
#define CSKY_DEFAUTL_ATMOSPHERIC_SCATTERING_INCLUDED

#include "UnityCG.cginc"
#include "CSky_Common.cginc"
#include "CSky_AtmosphereCommon.cginc"
//=====================================



uniform float  CSky_kCameraHeight;
uniform float  CSky_kInnerRadius;
uniform float  CSky_kInnerRadius2;
uniform float  CSky_kOuterRadius;
uniform float  CSky_kOuterRadius2;
uniform float  CSky_kScale;
uniform float  CSky_kScaleOverScaleDepth;
uniform float  CSky_kKmESun;
uniform float  CSky_kKm4PI;
uniform float  CSky_kKrESun;
uniform float  CSky_kKr4PI;
uniform float3 CSky_InvWavelength;
//=====================================


inline float RayleighPhase(float cosTheta)
{
	return 0.75 + 0.75 * (1.0 + cosTheta * cosTheta);
}
//===========================================================================================================

float Scale(float Cos)
{
	float x = 1.0 - Cos;
	return 0.25 * exp(-0.00287 + x * (0.459 + x * (3.83 + x * (-6.80 + x * 5.25))));
}
//===========================================================================================================


inline void AtmosphericScattering(float3 ray, out float3 inscatter, out float4 outscatter, int clampScatter)
{


	//ray.y += CSky_HorizonOffset;

	// Fix downside.

	/*
	#ifndef SHADER_API_MOBILE
		ray.y = abs(ray.y);
	#endif*/

	ray.y = saturate(ray.y);
	ray = normalize(ray);
	//======================================================================================================================================

	float3 cameraPos = float3(0.0, CSky_kInnerRadius + CSky_kCameraHeight, 0.0);
	float  far       = (sqrt(CSky_kOuterRadius2 + CSky_kInnerRadius2 * ray.y * ray.y - CSky_kInnerRadius2) - CSky_kInnerRadius * ray.y);
	float3 pos       = cameraPos + far * ray;
	//======================================================================================================================================

	float startDepth  = exp(CSky_kScaleOverScaleDepth * -CSky_kCameraHeight);
	float startHeight = CSky_kInnerRadius + CSky_kCameraHeight;
	float startAngle  = dot(ray, cameraPos) / startHeight;
	float startOffset = startDepth * Scale(startAngle);
	//======================================================================================================================================

	const float samples = 2;

	float  sampleLength = far / samples;
	float  scaledLength = sampleLength * CSky_kScale;
	float3 sampleRay    = ray * sampleLength;
	float3 samplePoint  = cameraPos + sampleRay * 0.5;
	//======================================================================================================================================

	float3 frontColor = 0.0; float4 outColor = 0.0;

	for (int i = 0; i < int(samples); i++)
	{

		float height    = length(samplePoint);
		float invHeight = 1.0 / height; // reciprocal.
		//==================================================================================================================================

		float  depth       = exp(CSky_kScaleOverScaleDepth * (CSky_kInnerRadius - height));
		float  lightAngle  = dot(CSky_SunDirection.xyz, samplePoint) * invHeight;
		float  cameraAngle = dot(ray, samplePoint) * invHeight;
		float3 betaAtten   = (CSky_InvWavelength * CSky_kKr4PI) + CSky_kKm4PI;
		//==================================================================================================================================

		float  scatter   = startOffset + depth * (Scale(lightAngle) - Scale(cameraAngle));
		float3 attenuate = clampScatter ? exp(-clamp(scatter, 0.0, 50) * betaAtten) : exp(-scatter * betaAtten);
		float3 dayColor  = attenuate * (depth * scaledLength) * CSky_SunAtmosphereTint;
		//==================================================================================================================================

		float3 nightColor = 0; float nightLightAngle = 0.0;

		if(CSky_MoonRayleigh == 1)
		{
			nightLightAngle = dot(CSky_MoonDirection.xyz, samplePoint) * invHeight;
		}
		else
		{
			nightLightAngle = -lightAngle;
		}

		float  nightScatter   = startOffset + depth * (Scale(nightLightAngle) - Scale(cameraAngle));
		float3 nightAttenuate = clampScatter ? exp(-clamp(nightScatter, 0.0, 50) * betaAtten) : exp(-nightScatter * betaAtten);

		nightColor = nightAttenuate * (depth * scaledLength) * CSky_MoonAtmosphereTint.rgb;
		outColor.a += nightAttenuate * (depth * scaledLength);
		//==================================================================================================================================

		frontColor   += dayColor + nightColor;
		outColor.rgb += dayColor;
		samplePoint  += sampleRay;
	}
	//==================================================================================================================================
	
	float cosTheta = dot(ray, CSky_SunDirection.xyz);
	inscatter      = ((frontColor * (CSky_InvWavelength * CSky_kKrESun))) * RayleighPhase(cosTheta);
	outscatter     = outColor * CSky_kKmESun;
	//outscatter.a = DESATURATE(outColor.aaa);
	//==================================================================================================================================
}
//==========================================================================================================================================


inline void RayleighScattering(float3 ray, out float3 inscatter, int clampScatter)
{


	//ray.y += CSky_HorizonOffset;

	// Fix downside.

	/*
	#ifndef SHADER_API_MOBILE
		ray.y = abs(ray.y);
	#endif*/

	ray.y = saturate(ray.y);

	ray = normalize(ray);
	//==================================================================================================================================

	float3 cameraPos = float3(0.0, CSky_kInnerRadius + CSky_kCameraHeight, 0.0);
	float  far       = (sqrt(CSky_kOuterRadius2 + CSky_kInnerRadius2 * ray.y * ray.y - CSky_kInnerRadius2) - CSky_kInnerRadius * ray.y);
	float3 pos       = cameraPos + far * ray;
	//==================================================================================================================================

	float startDepth  = exp(CSky_kScaleOverScaleDepth * -CSky_kCameraHeight);
	float startHeight = CSky_kInnerRadius + CSky_kCameraHeight;
	float startAngle  = dot(ray, cameraPos) / startHeight;
	float startOffset = startDepth * Scale(startAngle);
	//==================================================================================================================================

	const float samples = 2;

	float  sampleLength = far / samples;
	float  scaledLength = sampleLength * CSky_kScale;
	float3 sampleRay    = ray * sampleLength;
	float3 samplePoint  = cameraPos + sampleRay * 0.5;
	//==================================================================================================================================

	float3 frontColor = 0.0;

	for (int i = 0; i < int(samples); i++)
	{

		float height    = length(samplePoint);
		float invHeight = 1.0 / height; // reciprocal.
		//==================================================================================================================================

		float  depth       = exp(CSky_kScaleOverScaleDepth * (CSky_kInnerRadius - height));
		float  lightAngle  = dot(CSky_SunDirection.xyz, samplePoint) * invHeight;
		float  cameraAngle = dot(ray, samplePoint) * invHeight;
		float3 betaAtten   = (CSky_InvWavelength * CSky_kKr4PI) + CSky_kKm4PI;
		//==================================================================================================================================

		float  scatter   = startOffset + depth * (Scale(lightAngle) - Scale(cameraAngle));
		float3 attenuate = clampScatter ? exp(-clamp(scatter, 0.0, 50) * betaAtten) : exp(-scatter * betaAtten);
		float3 dayColor  = attenuate * (depth * scaledLength) * CSky_SunAtmosphereTint;
		//==================================================================================================================================

		float3 nightColor = 0; float nightLightAngle = 0.0;

		if(CSky_MoonRayleigh)
		{
			nightLightAngle = dot(CSky_MoonDirection.xyz, samplePoint) * invHeight;
		}
		else
		{
			nightLightAngle = -lightAngle;
		}

		float  nightScatter   = startOffset + depth * (Scale(nightLightAngle) - Scale(cameraAngle));
		float3 nightAttenuate = clampScatter ? exp(-clamp(nightScatter, 0.0, 50) * betaAtten) : exp(-nightScatter * betaAtten);
		nightColor            = nightAttenuate * (depth * scaledLength) * CSky_MoonAtmosphereTint.rgb;
		//==================================================================================================================================

		frontColor  += dayColor + nightColor;
		samplePoint += sampleRay;
	}
	//======================================================================================================================================

	float cosTheta = dot(ray, CSky_SunDirection.xyz);
	inscatter      = ((frontColor * (CSky_InvWavelength * CSky_kKrESun))) * RayleighPhase(cosTheta);
	//======================================================================================================================================
}
//==========================================================================================================================================


#endif
// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/SnowStandard"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		
		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}

		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8, 0.8, 0.8, 0.6)
		_RimMin ("Rim Min", Range(0, 1)) = 0.5
		_RimMax ("Rim Max", Range(0, 1)) = 1.0

		//SNOW
		_SnowColor ("Snow Color", Color) = (.94, .96, 1, 1)
		_SnowShadowColor ("Snow Shadow Color", Color) = (.2, .2, .3, 1)
		_SnowRimColor ("Snow Rim", Color) = (1, 1, 1, 0.7)
		_SnowAngle ("Snow Angle", Vector) = (0, 1, 0, 0)
		_SnowThr ("Snow Threshold", Range(0, 1)) = 0.5
		_SnowThickness ("Snow Thickness", Range(0, 0.2)) = 0.02

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows addshadow vertex:vert exclude_path:deferred exclude_path:prepass
		#pragma target 3.0

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		fixed4 _SnowColor;
		fixed4 _SnowShadowColor;
		fixed4 _SnowRimColor;
		half4 _SnowAngle;
		fixed _SnowThr;
		fixed _SnowThickness;
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
		};

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			float4 tangent : TANGENT;
		#endif
	#if UNITY_VERSION >= 550
			UNITY_VERTEX_INPUT_INSTANCE_ID
	#endif
		};
		
		//================================================================
		// VERTEX FUNCTION

		void vert(inout appdata_tcp2 v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float3 worldN = UnityObjectToWorldNormal(v.normal);

			//Snow accumulation
			#define SNOW_NORMAL worldN
			float3 snowAngle = normalize(_SnowAngle);
			float snowFactor = (dot(SNOW_NORMAL, snowAngle) + 1) / 2.0;
			half sn = (1-_SnowThr*(1 + 0.1));
			snowFactor = smoothstep(sn, sn + 0.1, snowFactor);
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			worldPos.xyz += SNOW_NORMAL.xyz * snowFactor * _SnowThickness;
			v.vertex = mul(unity_WorldToObject, worldPos);
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;

			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (_RimColor.rgb * rim) * _RimColor.a;

			//Snow accumulation
			float3 snowAngle = normalize(_SnowAngle);
			float snowFactor = (dot(o.Normal, snowAngle) + 1) / 2.0;
			half sn = (1-_SnowThr*(1 + 0.1));
			snowFactor = smoothstep(sn, sn + 0.1, snowFactor);
			o.Albedo = lerp(o.Albedo, _SnowColor.rgb, snowFactor * _SnowColor.a);
			o.Emission += rim * _SnowRimColor.rgb * _SnowRimColor.a * snowFactor;
		}

		ENDCG
	}

	Fallback "Diffuse"
}
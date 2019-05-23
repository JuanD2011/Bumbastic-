Shader "Custom/OscillatingLightModel" 
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Osc
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}

		float4 LightingOsc(SurfaceOutput s, float3 lightDirection, float attenuation)
		{
			float light = (dot(s.Normal, lightDirection) * 2);
			float oscLight = light/2 * sin(_Time.y * 8) + (3 * (light/2));

			float4 c;
			c.rgb = oscLight * attenuation * s.Albedo * _LightColor0.rgb;
			c.a = s.Alpha;
			return c;
		}

		ENDCG
	}
		FallBack "Diffuse"
}

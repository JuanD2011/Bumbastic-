Shader "Custom/Transparent/TranparencyWithEdges"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
		}
        LOD 200

        CGPROGRAM

        #pragma surface surf HalfLambert alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _Mask;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 mask = tex2D(_Mask, IN.uv_MainTex);
			
            o.Albedo = c.rgb;
			o.Alpha = c.a - (1 - mask);
        }

		float4 LightingHalfLambert(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float pp = dot(s.Normal, lightDir);
			float hLambert = pp * 0.5 + 0.5;

			float4 col;
			col.rgb = s.Albedo * hLambert;
			col.a = s.Alpha;
			return col;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
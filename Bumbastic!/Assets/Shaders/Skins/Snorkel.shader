Shader "Custom/Snorkel"
{
    Properties
    {
		_Cube("Cubemap", CUBE) = "" {}

        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags 
		{ 
			"Queue"="Transparent"
			"IgnoreProyector"="True"
			"RenderType"="TransparentCutout"
		}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _Mask;
		samplerCUBE _Cube;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldRefl;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float m = tex2D (_Mask, IN.uv_MainTex).r;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Emission = texCUBE(_Cube, IN.worldRefl).rgb * (1 - m);
			
			if( m < 1)
			{
				o.Alpha = 0.2;
			}
			else
			{
				o.Alpha = 1;
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}

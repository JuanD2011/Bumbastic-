﻿Shader "Custom/Transparent/Tranparency"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque"
			"IgnoreProjector" = "True"
			"RenderType" = "TranparentCutout"
		}
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

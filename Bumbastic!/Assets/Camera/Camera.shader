Shader "Custom/Camera" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Contrast("Contrast", Range(0, 1)) = 0.5
		_Brightness("Brightness", Range(-255, 255)) = 0
	}
	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;

			float _Contrast;
			float _Brightness;

			float4 frag(v2f_img i) : COLOR
			{
				float4 c1 = tex2D(_MainTex, i.uv);
				//float4 c2 = c1 + 100.0 / 255;
				//float4 c2 = pow(c1 , 0.9);
				float4 c2 = (c1 + _Brightness / 255) * _Contrast;
				//float4 c2 = (c1.r + c1.g + c1.b) / 3;
				//float4 c2 = c1.r * 0.2126 + c1.g * 0.7152 + c1.b * 0.072;
				c2.a = 1;
				return c2;
			}
			ENDCG
		}
	}
	FallBack off
}
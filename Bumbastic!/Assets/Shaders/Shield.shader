Shader "Custom/Shield"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Alpha("Alpha", Range(0, 1)) = 1
		_AlphaMask("Mask alpha", Range(0,1)) = 1
		_Mask("Mask", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
		{ 
			"Queue" = "Transparent"
			"IgnoreProyector" = "True"
			"RenderType" = "TransparentCutout"
		}

        LOD 200
		Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0
        sampler2D _MainTex;
        sampler2D _Mask;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Mask;
        };

        fixed4 _Color;
		float _Alpha;
		float _AlphaMask;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed mask = tex2D(_Mask, IN.uv_Mask);

			if (mask < 0.2)
			{
				mask = _AlphaMask;
			}

            o.Albedo = c.rgb;
            o.Alpha = _Alpha * mask;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

Shader "Custom/Shield"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Alpha("Alpha", Range(0, 1)) = 1
		_AlphaMask("Mask alpha", Range(0,1)) = 1
		_Mask("Mask", 2D) = "white" {}

		_VelX("X speed", float) = 1
		_VelY("Y speed", float) = 1
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
            half2 uv_MainTex;
			half2 uv_Mask;
        };

		half4 _Color;
		half4 _Color2;
		half _Alpha;
		half _AlphaMask;
		
		float _VelX;
		float _VelY;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 moveTexture = IN.uv_Mask;
			moveTexture += float2(_Time.y * _VelX, _Time.y * _VelY);

			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			half mask = tex2D(_Mask, moveTexture);

			if (mask < 0.2)
			{
				mask = _AlphaMask;
			}
			else
			{
				c.rgb *= _Color2;
			}

            o.Albedo = c.rgb;
            o.Alpha = _Alpha * mask;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

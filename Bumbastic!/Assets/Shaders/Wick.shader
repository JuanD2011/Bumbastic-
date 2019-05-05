Shader "Custom/Wick"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		//Alpha
		_AlphaMask("Alpha mask", 2D) = "white" {}
		_Factor("Factor", Range(0, 1)) = 0.5
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
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _AlphaMask;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
		float _Factor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 mask = tex2D(_AlphaMask, IN.uv_MainTex);

			o.Albedo = c.rgb;

			if (mask.r <= _Factor)
			{
				o.Alpha = 0;
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
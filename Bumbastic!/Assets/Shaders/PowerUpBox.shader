Shader "Custom/PowerUpBox"
{
    Properties
    {
        [HDR]_EmisiveColor ("EmisiveColor", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmisiveMask ("EmisiveMask", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_EmisiveIntensity("Emisive", Range(0,10)) = 1.0
		_Alpha("Alpha", Range(0, 1)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent" 
			"RenderType" = "TransparentCutout"
			"IgnoreProyector" = "True"
		}

		Pass 
		{
			ColorMask 0
		}	

		LOD 200
		
		CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _EmisiveMask;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
		half _EmisiveIntensity;
		half _Alpha;
        fixed4 _EmisiveColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float m = tex2D (_EmisiveMask, IN.uv_MainTex).r;
			
			if(m > 0.5)
			{
				o.Albedo = _EmisiveColor* _EmisiveIntensity;;			
				o.Emission = _EmisiveColor * _EmisiveIntensity;
			}
			else o.Albedo = c.rgb;
            
			o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Alpha = _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

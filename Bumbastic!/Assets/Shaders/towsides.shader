Shader "Custom/towsides"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Emisive ("Emisive", Range(0,1)) = 0.5
		_Umbral("Umbral", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags 
		{	
			"Queue"="Transparent" 
			"IgnoreProjector" = "True"
			"RenderType" = "TranparentCutout"
		}

		Cull Off

        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			fixed3 viewDir;
			fixed3 worldNormal;
        };

        half _Emisive;
		float _Umbral;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;

			float pp = abs(dot(IN.viewDir, IN.worldNormal));



            o.Albedo = c.g * _Color;
			o.Emission = c.g * _Emisive * _Color;
            o.Alpha = c.a + 1-pp;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

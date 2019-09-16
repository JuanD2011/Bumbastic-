Shader "Custom/CartoonWater"
{
    Properties
    {
		[Space(20)][Header(MainTex and Base)][Space(20)]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Alpha("Alpha", Range(0, 1)) = 1
    
		[Space(20)][Header(Voronoi and its velocity movement)][Space(20)]
		_Voronoi ("Voronoi", 2D) = "white" {}
		[HDR]_VoronoiColor ("Voronoi Color", Color) = (1, 1, 1, 1)
		_VoronoiVelX("Voronoi X velocity", float) = 0.1
		_VoronoiVelY("Voronoi Y velocity", float) = 0.1
    }
    SubShader
    {
        Tags 
		{
			"IgnoreProyector" = "True"
			"RenderType" = "TransparentCutout"
		}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
		float _Alpha;

        sampler2D _Voronoi;
		float _VoronoiVelX;
		float _VoronoiVelY;
		float4 _VoronoiColor;
        
		struct Input
        {
            float2 uv_MainTex;
            float2 uv_Voronoi;
			float2 uv_Noise;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 voronoiMovement = IN.uv_Voronoi;
			float2 voronoiMovement1 = IN.uv_Voronoi;
			voronoiMovement += float2(_Time.y * _VoronoiVelX, _Time.y * _VoronoiVelY);
			voronoiMovement1 += float2(_Time.y * -_VoronoiVelX, _Time.y * -_VoronoiVelY);
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 voronoi = tex2D(_Voronoi, voronoiMovement) * _VoronoiColor;
			fixed4 voronoi1 = tex2D(_Voronoi, voronoiMovement1);
			
			o.Albedo = c.rgb + (voronoi.rgb * voronoi1.rgb);
            o.Alpha = _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

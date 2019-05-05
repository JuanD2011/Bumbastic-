Shader "Custom/BombHole"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _HoleColor ("Hole Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Target("Target pos", Vector) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
        };

        fixed4 _Color;
        fixed4 _HoleColor;
		float3 _Target;
        
		void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			float dist = distance(_Target, IN.worldPos);

			if (dist >= 0 && dist <= 1.25)
			{
				c.rgb = _HoleColor;
			}
            o.Albedo = c.rgb;
        }

		float4 LightingLambert(SurfaceOutput s, fixed3 lightDir, fixed atten) 
		{
			float4 c;
			c.rgb = _LightColor0.rgb * atten * s.Albedo;
			c.a = s.Alpha;
			return c;
		}

        ENDCG
    }
    FallBack "Diffuse"
}

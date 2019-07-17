Shader "Custom/towsides"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Saturation("_Saturation", Range(0, 100)) = 0.5
		_AngularVelocity("_AngularVelocity", float) = 0.5
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

		float _Saturation;
		float _H;
        fixed4 _Color;
        fixed _AngularVelocity;

		inline float4 RGBToHSV(float4 _rgb)
		{
			float Cmax = max(max(_rgb.r, _rgb.g), _rgb.b);
			float Cmin = min(min(_rgb.r, _rgb.g), _rgb.b);

			float delta = Cmax - Cmin;

			float h;
			float s;
			float v;

			if (Cmax == _rgb.r && _rgb.g >= _rgb.b)
			{
				h = 60.0 / 360 * (_rgb.g - _rgb.b) / delta;
			}
			else if (Cmax == _rgb.r && _rgb.g < _rgb.b)
			{
				h = 60.0 / 360 * ((_rgb.g - _rgb.b) / delta) + 360.0 / 360;
			}
			else if (Cmax == _rgb.g)
			{
				h = 60.0 / 360 * ((_rgb.b - _rgb.r) / delta) + 120.0 / 360;
			}
			else if (Cmax == _rgb.b)
			{
				h = 60.0 / 360 * ((_rgb.r - _rgb.g) / delta) + 240.0 / 360;
			}

			if (Cmax == 0)
			{
				s = 0;
			}
			else
			{
				s = 1 - (Cmin / Cmax);
			}

			v = Cmax;

			float4 hsv = float4(h, s, v, 1);
			return hsv;
		}

		inline float3 hsv_to_rgb(float3 HSV)
		{
			float3 RGB = HSV.z;

			float var_h = HSV.x * 6;
			float var_i = floor(var_h);
			float var_1 = HSV.z * (1.0 - HSV.y);
			float var_2 = HSV.z * (1.0 - HSV.y * (var_h - var_i));
			float var_3 = HSV.z * (1.0 - HSV.y * (1 - (var_h - var_i)));
			if (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
			else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
			else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
			else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
			else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
			else { RGB = float3(HSV.z, var_1, var_2); }

			return (RGB);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D (_MainTex, IN.uv_MainTex) ;

			float pp = abs(dot(IN.viewDir, IN.worldNormal));

			float4 converted = RGBToHSV(c);
			converted.r = 0.5 * sin(_Time.y * _AngularVelocity) + 0.5;
			converted.g = _Saturation;
			float3 converted1 = hsv_to_rgb(converted);

			o.Albedo = converted1 * _Color;
			//o.Emission = converted1 * _Color;
            o.Alpha = c.a + 1-pp;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

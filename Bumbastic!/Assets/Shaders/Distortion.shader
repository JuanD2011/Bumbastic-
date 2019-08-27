Shader "Custom/Distort"
{
	Properties
	{
		_Strength("Distort Strength", float) = 1.0
		_SpeedX("Speed X", float) = 1.0
		_SpeedY("Speed Y", float) = 1.0
		_Noise("Noise Texture", 2D) = "white" {}
		_StrengthFilter("Strength Filter", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"DisableBatching" = "True"
		}

		GrabPass
		{
			"_BackgroundTexture"
		}

		Pass
		{
			ZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _BackgroundTexture;
			sampler2D _Noise;
			sampler2D _StrengthFilter;

			float _Strength;
			float _SpeedX;
			float _SpeedY;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 texCoord : TEXCOORD0;
			};

			struct vertexOutput 
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = UnityObjectToClipPos(input.vertex);
				output.uv = ComputeGrabScreenPos(output.pos);

				float noise = tex2Dlod(_Noise, float4(input.texCoord, 0)).rgb;
				float filter = tex2Dlod(_StrengthFilter, float4(input.texCoord, 0)).rgb;
				
				output.uv.x += cos(noise * _Time.y * _SpeedX) * filter * _Strength;
				output.uv.y += sin(noise * _Time.y * _SpeedY) * filter * _Strength;

				return output;
			}

			float4 frag(vertexOutput input) : SV_Target
			{
				return tex2Dproj(_BackgroundTexture, input.uv);
			}	
			ENDCG
		}
	}
}
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Vignette"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_VignetteMask("Vignette mask", 2D) = "white" {}
		_ColorTreshold("Color Treshold", float) = 0.7
		
		_TexScale("Scale of Tex", float) = 1.0
		_TexRatio("Ratio of Tex", float) = 1.0
		_Theta("Rotation of Tex", float) = 0.0
		_PlaneScale("Scale of Plane Mesh", Vector) = (1, 1, 0, 0)
	}

	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _VignetteMask;

			float4 _Color;
			float _ColorTreshold;
			float2 _PlaneScale;
			float _TexScale;
			float _TexRatio;
			float _Theta;

			struct v2f {
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD;
			};

			float3x3 getXYTranslationMatrix(float2 translation) {
				return float3x3(1, 0, translation.x, 0, 1, translation.y, 0, 0, 1);
			}

			float3x3 getXYRotationMatrix(float theta) {
				float s = -sin(theta);
				float c = cos(theta);
				return float3x3(c, -s, 0, s, c, 0, 0, 0, 1);
			}

			float3x3 getXYScaleMatrix(float2 scale) {
				return float3x3(scale.x, 0, 0, 0, scale.y, 0, 0, 0, 1);
			}

			float2 applyMatrix(float3x3 m, float2 uv) {
				return mul(m, float3(uv.x, uv.y, 1)).xy;
			}

			v2f vert(appdata_full v) {
				v2f o;
				float2 offset = float2((1 - _PlaneScale.x) * 0.5, (1 - _PlaneScale.y) * 0.5);
				o.texcoord = applyMatrix(
					getXYTranslationMatrix(float2(0.5, 0.5)),
					applyMatrix( // scale
						getXYScaleMatrix(float2(1 / _TexRatio, 1) * _TexScale),
						applyMatrix( // rotate
							getXYRotationMatrix(_Theta),
							applyMatrix(
								getXYTranslationMatrix(float2(-0.5, -0.5) + offset),
								(v.texcoord.xy * _PlaneScale)
							)
						)
					)
				);

				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(v2f IN) : COLOR
			{
				float4 c1 = tex2D(_MainTex, IN.texcoord);
				float4 vignetteMask = tex2D(_VignetteMask, IN.texcoord);

				if (vignetteMask.r <= _ColorTreshold)
				{
					c1 *= _Color;
				}

				return c1 * vignetteMask;
			}
			ENDCG
		}
	}
	FallBack off
}
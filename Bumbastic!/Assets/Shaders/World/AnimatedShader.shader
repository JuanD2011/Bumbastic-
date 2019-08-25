Shader "Custom/AnimatedShader"
{
    Properties {
		_C1 ("Base Color", Color) = (1,1,1,1)
		_C2 ("Highlight", Color) = (1,1,1,1)
		_C3 ("Shadow", Color) = (0,0,0,1)
		_I1 ("Intensity", float) = 0
		_T1 ("Main Texture", 2D) = "white" {}
		_AV1 ("Angular Velocity", float) = 0
		_AP1 ("Amplitude", float) = 0
		_M1 ("Multi Mask", 2D) = "white" {}
		_V1 ("Velocity X", Range(-1,1)) = 0
		_V2	("Velocity Y", Range(-1,1)) = 0
		_Mult ("Mask Speed", float) = 0
		_S1 ("Mask Strength Primary", Range(0,1)) = 0
		_S2 ("Mask Strength Secondary", Range(0,1)) = 0
		_Com ("Mask Complement Speed", float) = 0

	}

	// Primer subshader
	SubShader { 
		LOD 200
		
		CGPROGRAM

		// Método para el cálculo de la luz
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		// Información adicional provista por el juego
		struct Input {
			float2 uv_T1;
			float2 uv_M1;
		};

		// Declaración de variables

		float4 _C1;
		float4 _C2;
		float4 _C3;
		float _I1;
		sampler2D _T1;
		sampler2D _M1;
		float _AV1;
		float _AP1;
		float _V1;
		float _V2;
		float _Mult;
		float _S1;
		float _S2;
		float _Com;
		// Nucleo del programa
		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 mainMaskAnimatedUV = IN.uv_M1;
			float2 main2MaskAnimatedUV = IN.uv_M1;
			float2 secondaryMaskAnimatedUV = IN.uv_M1;
			float2 textureAnimatedUV = IN.uv_T1;

			mainMaskAnimatedUV += float2(_Mult * _V1 * _Time.y, _Mult * _V2 * _Time.y);
			main2MaskAnimatedUV += float2(-_Mult * _V2 * _Time.y, -_Mult * _V1 * _Time.y);
			secondaryMaskAnimatedUV += float2(_Mult * _Com * _Time.y, 0);
			textureAnimatedUV += float2(_AP1 * sin(_Time.y * _AV1),_AP1 * cos(_Time.y * _AV1));

			float4 mainMask_ = tex2D(_M1, mainMaskAnimatedUV);
			float4 main2Mask_ = tex2D(_M1, main2MaskAnimatedUV);
			float4 secondaryMask_ = tex2D(_M1, secondaryMaskAnimatedUV);
			float4 texture_ = tex2D(_T1, textureAnimatedUV);
		
			float maskRed = mainMask_.r * _S1;
			float maskGreen = main2Mask_.g * _S2;
			float maskBlue = secondaryMask_.b;

			float textureGrey = (texture_.r + texture_.g + texture_.b) / 3;
			float3 modifiedTexture = (textureGrey,textureGrey,textureGrey);			

			float4 maskProduct = (1,1,1,1);						
			if((maskRed > 0 && maskGreen > 0)){maskProduct = abs(maskGreen + maskRed) / (2);}
			else{maskProduct = (abs(maskRed - maskGreen) * abs(_S1 - _S2)) * abs(maskGreen + maskRed) / (_S1 + _S2);}
			
			float3 secondaryMaskProuct = maskBlue * (maskProduct);

			float3 Color1 = modifiedTexture * _C1;
			float3 Color2 = modifiedTexture * maskProduct * _C2 ;
			float3 Color3 = modifiedTexture * (1 - maskProduct) * _C3;
			float3 Color4 = modifiedTexture * secondaryMaskProuct * _C2 * _I1;

			float3 outPut = Color1 * _I1 * (Color2 + Color3 + Color4);
				
			
			o.Albedo = outPut;	
		}
		ENDCG

	}// Final del primer subshader

	// Segundo subshader si existe alguno
	// Tercer subshader...

	// Si no es posible ejecutar ningún subshader ejecute Diffuse
	FallBack "Diffuse"
}

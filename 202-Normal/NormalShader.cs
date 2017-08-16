Shader "MyShader/Normal"
{
	Properties
	{
		//_MainTex("cww",2D) = "white" {}
		_EmissiveColor("Emissive Color", Color) = (1,1,1,1)
		_AmbientColor("Ambient Color", Color) = (1,1,1,1)
		_MySliderValue("This is a Slider", Range(0,10)) = 2.5
		_Texture("Texture", 2D) = ""{}

		//normal
		_NormalTex ("Normal Map",2D) = "bump"{}
		_NormalIntensity ("Normal Intensity",Range(0,2)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf BasicDiffuse  
		#pragma Target Level 5.0
		//tell the shader to find a lighting model by the name BasicDiffuss(defined in LightigBasicDiffuse below��)

		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _MySliderValue;
		sampler2D _Texture;
		//normal
		sampler2D _NormalTex;
		float _NormalIntensity;

		inline float4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten)
		//s��������surf�������������
		//lightDir�������߷���
		//atten������˥��ϵ��
		{
			float difLight = max(0, dot(s.Normal, lightDir));

			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten);  //_LightColor0����Unity��������
			col.a = s.Alpha;
			return col;
		}

		struct Input
		{
			float2 uv_MainTex;		
			float2 uv_Texture;		//������ͼ
			float2 uv_NormalTex;	//������ͼ
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			//������ɫ
			float4 TData = tex2D(_Texture, IN.uv_Texture);
			float4 finalColor;
			float4 c;
			c = pow((_EmissiveColor + _AmbientColor), _MySliderValue);
			finalColor = TData;
			c = lerp(_EmissiveColor, _AmbientColor, TData);
			finalColor *= c;
			finalColor = saturate(finalColor);
			//������ɫ
			float3 normalMap = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
			normalMap = float3(normalMap.x * _NormalIntensity, normalMap.y * _NormalIntensity, normalMap.z);

			o.Normal = normalMap.rgb;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}

		FallBack "Diffuse"
}

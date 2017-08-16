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
		//tell the shader to find a lighting model by the name BasicDiffuss(defined in LightigBasicDiffuse below！)

		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _MySliderValue;
		sampler2D _Texture;
		//normal
		sampler2D _NormalTex;
		float _NormalIntensity;

		inline float4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten)
		//s——经过surf函数处理后的输出
		//lightDir——光线方向
		//atten——光衰减系数
		{
			float difLight = max(0, dot(s.Normal, lightDir));

			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten);  //_LightColor0——Unity场景光线
			col.a = s.Alpha;
			return col;
		}

		struct Input
		{
			float2 uv_MainTex;		
			float2 uv_Texture;		//纹理贴图
			float2 uv_NormalTex;	//法线贴图
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			//纹理颜色
			float4 TData = tex2D(_Texture, IN.uv_Texture);
			float4 finalColor;
			float4 c;
			c = pow((_EmissiveColor + _AmbientColor), _MySliderValue);
			finalColor = TData;
			c = lerp(_EmissiveColor, _AmbientColor, TData);
			finalColor *= c;
			finalColor = saturate(finalColor);
			//法线颜色
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

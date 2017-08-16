Shader "MyShader/CustomBlinnPhong"
{
	Properties 
	{
		_MainTint ("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpecularColor ("Specular Tint", Color) = (1,1,1,1)
		_SpecPower ("Specular Power", Range(0.1, 120)) = 3  
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf CustomBlinnPhong

		sampler2D _MainTex;
		sampler2D _SpecularMask;
		float4 _MainTint;
		float4 _SpecularColor;
		float _SpecPower;
		
				
		inline fixed4 LightingCustomBlinnPhong(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			float3 halfVector = normalize (lightDir + viewDir);
			
			float diff = max (0, dot (s.Normal, lightDir));
			
			//相比单纯的Phong模型，这里所取得的后面幂运算底数H和N之间角度是Phong模型中R和V之间角度的一半，也就是底数相比Phong变大了，反射效果会更明显
			float NdotH = max (0, dot (s.Normal, halfVector));
			float spec = pow (NdotH, _SpecPower);
			
			float4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff) + (_LightColor0.rgb * _SpecularColor.rgb * spec) * (atten * 2);
			c.a = s.Alpha;
			return c;
		}

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_SpecularMask;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float4 c = tex2D (_MainTex, IN.uv_MainTex) * _MainTint;
			float4 specMask = tex2D(_SpecularMask, IN.uv_SpecularMask) * _SpecularColor;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

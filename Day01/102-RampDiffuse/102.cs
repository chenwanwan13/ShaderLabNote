Shader "myShader/RampDiffuse" 
{
	Properties 
	{
		_EmissiveColor ("Emissive Color", Color) = (1,1,1,1)
		_AmbientColor  ("Ambient Color", Color) = (1,1,1,1)
		_MySliderValue ("This is a Slider", Range(0,10)) = 2.5
		_RampTex ("Ramp Texture", 2D) = "white"{}
		_floatA("floatA", Range(0,1)) = 0.5
		_floatB("floatB", Range(0,1)) = 0.5
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BasicDiffuse

		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _MySliderValue;
		sampler2D _RampTex;
		float _floatA;
		float _floatB;
		

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float4 c;
			c =  pow((_EmissiveColor + _AmbientColor), _MySliderValue);
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		inline float4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = dot(s.Normal, lightDir);
			half hLambert = difLight * _floatA + _floatB;
			//float hLambert = difLight * 0.8 + 0.1;
			float3 ramp = tex2D(_RampTex, float2(hLambert,0)).rgb;

			float4 col;
			col.rgb = s.Albedo  * (ramp);
			col.a = s.Alpha;
			return col;
		}
		
		ENDCG
	} 
	
	FallBack "Diffuse"
}

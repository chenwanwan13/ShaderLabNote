Shader "MyShader/day01" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_floatA("FloatA",Float) = 1
		_floatB("FloatB",Float) = 1
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf WrapLambert
		float _floatA;
		float _floatB;

		half4 LightingWrapLambert(SurfaceOutput s, half3 lightDir, half atten) {
		half NdotL = dot(s.Normal, lightDir);
		half diff = NdotL * _floatA + _floatB;
		//half diff = NdotL * 1 + 1;
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
		c.a = s.Alpha;
		return c;
	}

	struct Input {
		float2 uv_MainTex;
	};

	sampler2D _MainTex;
	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
	}
	ENDCG
	}
		Fallback "Diffuse"
}
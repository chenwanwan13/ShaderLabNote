Shader "Unlit/Phong"
{
	Properties
	{
		_MainTint("Diffuse Tint",Color) = (1,1,1,1)				//漫反射颜色
		_MainTex("Base",2D)="white"{}							//贴图
		_SpecularColor("Specular Color",Color) = (1,1,1,1)		//反射颜色
		_SpecPower("Specular Power",Range(0.1,30)) = 1			//反射强度
		_SpecHardness("Specular Hardness",Range(0.1,10)) = 2	//反射硬度
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200		//LOD: Diffuse
		/*
			Level of Detail in Unity:
			VertexLit kind of shaders = 100
			Decal, Reflective VertexLit = 150
			Diffuse = 200
			Diffuse Detail, Reflective Bumped Unlit, Reflective Bumped VertexLit = 250
			Bumped, Specular = 300
			Bumped Specular = 400
			Parallax = 500
			Parallax Specular = 600
		*/


		CGPROGRAM
		#pragma surface surf Phong	 //使用Phong光照模型，在LightingPhong函数中定义

		float4 _SpecularColor;
		sampler2D _MainTex;
		float4 _MainTint;
		float _SpecPower;
		float _SpecHardness;

		inline fixed4 LightingPhong(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten) {
			float diff = dot(s.Normal, lightDir);									//求漫反射光方向，即计算入射光和法线夹角
			float3 reflectionVector = normalize((2.0 * s.Normal * diff) - lightDir);	//光线反射方向计算，即求入射光关于法线对称的向量

			float spec = pow(max(0, dot(reflectionVector, viewDir)), _SpecPower);	//计算反射强度
			float3 finalSpec = _SpecularColor.rgb * spec;								//加入反射颜色得出最终反射光数据

			fixed4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb * diff) + (_LightColor0.rgb * finalSpec);//_LightColor0是shader内部变量，即Unity场景中光照颜色
			c.a = 1.0;
			return c;
		}

		struct Input
		{
			float2 uv_MainTex;		//外部输入贴图数据
		};

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainTint;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

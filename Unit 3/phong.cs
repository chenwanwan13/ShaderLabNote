Shader "Unlit/Phong"
{
	Properties
	{
		_MainTint("Diffuse Tint",Color) = (1,1,1,1)				//��������ɫ
		_MainTex("Base",2D)="white"{}							//��ͼ
		_SpecularColor("Specular Color",Color) = (1,1,1,1)		//������ɫ
		_SpecPower("Specular Power",Range(0.1,30)) = 1			//����ǿ��
		_SpecHardness("Specular Hardness",Range(0.1,10)) = 2	//����Ӳ��
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
		#pragma surface surf Phong	 //ʹ��Phong����ģ�ͣ���LightingPhong�����ж���

		float4 _SpecularColor;
		sampler2D _MainTex;
		float4 _MainTint;
		float _SpecPower;
		float _SpecHardness;

		inline fixed4 LightingPhong(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten) {
			float diff = dot(s.Normal, lightDir);									//��������ⷽ�򣬼����������ͷ��߼н�
			float3 reflectionVector = normalize((2.0 * s.Normal * diff) - lightDir);	//���߷��䷽����㣬�����������ڷ��߶ԳƵ�����

			float spec = pow(max(0, dot(reflectionVector, viewDir)), _SpecPower);	//���㷴��ǿ��
			float3 finalSpec = _SpecularColor.rgb * spec;								//���뷴����ɫ�ó����շ��������

			fixed4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb * diff) + (_LightColor0.rgb * finalSpec);//_LightColor0��shader�ڲ���������Unity�����й�����ɫ
			c.a = 1.0;
			return c;
		}

		struct Input
		{
			float2 uv_MainTex;		//�ⲿ������ͼ����
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

Shader "Custom/DoorShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Emission("Emission",Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		float3 rand(float3 co)
		{
     		return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
 		}

 		float3 semirand(float3 co) 
 		{
 			return co * sin(0.2);
 		}

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Emission;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			//half3 newWorld = half3(rand(IN.worldPos));
			//half3 newAlb = half3(newWorld.x * c.r,newWorld.y * c.g, newWorld.z * c.b);
			half3 alb = semirand(IN.worldPos);
			o.Albedo = alb * _Color;
			//o.Albedo = newAlb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			//o.Emission = _Emission;

		}
		ENDCG
	}
	FallBack "Diffuse"
}

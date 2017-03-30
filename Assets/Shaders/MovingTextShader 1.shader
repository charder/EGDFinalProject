Shader "Custom/MovingTextShader 1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ShadSpeed ("Movement Speed", Range(0,2)) = 1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _ShadSpeed;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv + _Time * _ShadSpeed + v.uv.x;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//just invert the colors
				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}

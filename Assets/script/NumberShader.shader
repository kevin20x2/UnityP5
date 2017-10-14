Shader "Custom/NumberShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		Pass{
			//Tags {"LightMode" = "ForwardBase"}
		Tags { "Queue"="Transparent" }
	Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Tags {"LightMode" = "ForwardBase"}
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"


		sampler2D _MainTex;
		float4  _MainTex_ST;
		half _LineWidth;
		half _Percent;
		fixed4 _Color;

		struct a2v {
			float4 vertex :	POSITION;
			float4 texcoord : TEXCOORD0;
		};
		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv  : TEXCOORD0;

		};

		v2f vert(a2v v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			return o;


		}
		fixed4	frag(v2f i):SV_Target
		{
			fixed4 ans = tex2D(_MainTex,i.uv);
			return ans;
		}



		ENDCG
		}
	}
	FallBack "Diffuse"
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FlowShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
			Tags { "RenderType" = "Transparent" }
			Pass{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha


			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			float4 _MainTex_ST;
		sampler2D _MainTex;
		struct a2v
		{
			float4 vertex : POSITION;
			float4 texcoord : TEXCOORD0;
		};
		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};
		v2f vert(a2v v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord*_MainTex_ST.xy + _MainTex_ST.zw;
			return o;
		}
		fixed4 frag(v2f i):SV_Target
		{
			/*float PI = 3.141592653f;
		float len = sqrt(i.uv.x *i.uv.x + i.uv.y*i.uv.y);
			float dy = -cos((PI - _Time.y*1.0f) / 2)*len;
			float dx = sin((PI - _Time.y*1.0f) / 2)*len;*/
			float dx = _Time.y *0.01f;
			float dy = -_Time.y*0.1f;
			fixed4 ans = tex2D(_MainTex,i.uv+ float2(dx,dy));
		float is_white = step(0.9,ans.x);
			return is_white *fixed4(0.0f,0.0f,0.0f,1.0f);

		}

	
		ENDCG
		}
	}
	FallBack "Diffuse"
}

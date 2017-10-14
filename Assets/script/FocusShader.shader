// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FocusShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Height("Height",2D) = "white" {}
		_Inrad("inrad ",Float) = 1.0
		_Outrad("outrad",Float) = 1.0
		_Outheight("outheight",Float) = 1.0
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			Pass{
				Tags { "LightMode" = "ForwardBase"}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				float4 _Color;
		float4 _MainTex_ST;
		sampler2D _MainTex;
		float4 _Height_ST;
		sampler2D _Height;
		float _Inrad;
		float _Outrad;
		float _Outheight;

		struct a2v
		{
			float4 vertex:POSITION;
			float4 texcoord: TEXCOORD0;
		};
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv :TEXCOORD0;
		//	float3 normal : TEXCOORD1;
		//	float3 worldPos : TEXCOORD2;
		};
		v2f vert(a2v v)
		{
			v2f o;
			o.uv.xy = v.texcoord.xy*_MainTex_ST.xy + _MainTex_ST.zw;
			float4 raw = tex2Dlod(_Height,float4(v.texcoord.y+0.05f*sin(0.8f*_Time.y),0,0,0));
			float4 raw_out = tex2Dlod(_Height,float4(v.texcoord.y+0.05f*sin(0.8f*_Time.y+10),0,0,0));
			float edge = step( v.texcoord.x,0.0f);
			float out_edge = step(1.0f, v.texcoord.x);
			float edge2 = step(0.6f,raw.x);
			float height = 1.0f*raw.x*(sin(0.5f*_Time.y * 20+100*v.texcoord.y) + 1);
			float height_out = _Outheight*raw_out.x*(sin(0.5f*_Time.y * 20+100*v.texcoord.y) + 1);// 震动的相位以及高度
			float tt = edge*0.3f* edge2*height;
			float tt_out = out_edge*edge2*0.8*height_out;
			float angle =2* 3.141592653f*v.texcoord.y;
			float dx = edge*_Inrad*sin(angle) + out_edge*_Outrad*sin(angle);
			float dz = edge*_Inrad*cos(angle) + out_edge*_Outrad*cos(angle);
			

			
			o.pos = UnityObjectToClipPos(float4(v.vertex.x-tt*sin(angle)+tt_out*sin(angle)+dx,v.vertex.y,v.vertex.z-tt*cos(angle)+tt_out*cos(angle)+dz,v.vertex.w));
			return o;


			

		}
		fixed4 frag(v2f i) :SV_Target{

			//return fixed4(i.uv,0.0f,1.0f);
			return _Color;

		}
				
			ENDCG
			
		}
		
		
	}
	FallBack "Diffuse"
}

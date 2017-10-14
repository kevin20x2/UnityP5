Shader "Custom/BloodShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_LineWidth ("Smoothness", Range(0,0.1)) = 0.01
		_Percent ("Metallic", Range(0,1)) = 0.5
	}
	SubShader {

		Pass{
		Tags { "RenderType"="Transparent" }
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
			fixed4 ans2 = tex2D(_MainTex, i.uv + float2(_LineWidth,_LineWidth));
			fixed4 ans3 = tex2D(_MainTex, i.uv - float2(_LineWidth,_LineWidth));
			fixed4 ans4 = tex2D(_MainTex, i.uv + float2(_LineWidth,-_LineWidth));
			float step1 = step(i.uv.x, _Percent);  // 下面部分为1
			float step2 = step(ans2.r, 0.5f);    // 黑色部分为1
			float step3 = step(ans3.r, 0.5f);    // 黑色部分为1
			float step5 = step(ans4.r, 0.5f);    // 黑色部分为1
			
			float step4 = step2*step3*step5;
			fixed4 res = (1 - step1)*ans + step1*step4*_Color + step1*(1-step4)*ans; // 下面的黑色部分变色 ,上面的部分不变 
			return res;
		}



		ENDCG
			}
	}
	FallBack "Diffuse"
}

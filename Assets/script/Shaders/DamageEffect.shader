Shader "Custom/DamageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Number("number",Float) = 5
		_Uv("uv" ,Float) =  0.0
		_Speed ("Speed",Range(0,100)) = 7 
	}
	SubShader
	{
		// No culling or depth
		Tags {"Queue" = "Transparent" "IgnoreProjector"= "True" "RenderType"="Transparent"}

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
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
			sampler2D _MainTex;
			float _Number;
			float _Speed;
			float _Uv;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float time = floor(_Uv *_Number);
				//float index = time - floor(time / _Number)*_Number;
				half2 uv = i.uv + half2(time,0);
				uv.x /= _Number;
				fixed4 c = tex2D(_MainTex, uv);
				return c ;
			}
			ENDCG
		}
	}
}

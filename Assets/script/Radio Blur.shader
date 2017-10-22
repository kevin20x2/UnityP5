// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Radio Blur" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Level ("level", Range(0,100)) = 10
	}
		SubShader{
			Pass{

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
	int _Level;
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			fixed4 frag(v2f i):SV_Target
			{
				fixed4 c;
                fixed2 center=fixed2(.5,.5);  
                fixed2 uv=i.uv-center;  
                fixed3 c1=fixed3(0,0,0);  
                for(fixed j=0;j<_Level;j++){  
                    c1+=tex2D(_MainTex,uv*(1-0.01*j)+center).rgb;  
                }  
                c.rgb=c1/_Level;  
                c.a=1;  
                return c;

			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}

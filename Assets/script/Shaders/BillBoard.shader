// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/BillBoard"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1,1,1,1)
		_VerticalBillboarding ("Vertical Restrains",Range(0,1)) = 1
		_Uv ("uv",Range(0,1)) = 0.0
		_Number ("number",Float) = 5
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "DisableBatching"="True"}
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
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
			float4 _Color;
			float _VerticalBillboarding;
			float _Uv;
			float _Number;


			v2f vert (appdata v)
			{
				float3 center = float3(0, 0, 0);
				float3 viewer = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos,1));
				float3 normalDir = viewer - center;
				normalDir.y = normalDir.y*_VerticalBillboarding;
				normalDir = normalize(normalDir);
				float3 upDir = abs(normalDir.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
				float3 rightDir = normalize(cross(upDir, normalDir));
				upDir = normalize(cross(normalDir,rightDir));
				float3 centerOffs = v.vertex.xyz - center;
				float3 localPos = center + rightDir *centerOffs.x + upDir*centerOffs.y + normalDir*centerOffs.z;
				v2f o;
				o.vertex = UnityObjectToClipPos(float4 (localPos,1));
				o.uv = v.uv;
				return o;
			}
			

			fixed4 frag (v2f i) : SV_Target
			{
				float time = floor(_Uv * _Number);
				float2 uv = i.uv + float2(time, 0);
				uv.x /= _Number;
				fixed4 col = tex2D(_MainTex, uv);
				// just invert the colors
			//	col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}

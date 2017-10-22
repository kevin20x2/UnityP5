Shader "Custom/DepthShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurSize ("Blur size ",Float ) = 1.0
	}
	SubShader
		{
			// No culling or depth
			//Cull Off ZWrite Off ZTest Always
				Pass {
								ZTest Always Cull Off ZWrite Off
									CGPROGRAM
									#pragma vertex vert
									#pragma fragment frag
									//ENDCG


//				CGPROGRAM
			//			#pragma vertex vert
			//			#pragma fragment frag

						#include "UnityCG.cginc"

						/*struct appdata
						{
							float4 vertex : POSITION;
							float2 uv : TEXCOORD0;
						};*/

						struct v2f
						{
							float4 pos : SV_POSITION;
							float2 uv : TEXCOORD0;
							half2 uv_depth : TEXCOORD1;
							float4 interpolatedRay :TEXCOORD2;
						};

						float4x4 _FrustumCornersRay;
						float4x4 _PreviousViewProjectionMatrix;
						sampler2D _MainTex;
						half4 _MainTex_TexelSize;
						sampler2D _CameraDepthTexture;
						half _BlurSize;

						v2f vert(appdata_img v)
						{
							v2f o;
							o.pos = UnityObjectToClipPos(v.vertex);
							o.uv = v.texcoord;
							o.uv_depth = v.texcoord;

							#if UNITY_UV_STARTS_AT_TOP
							if (_MainTex_TexelSize.y < 0)
								o.uv_depth.y = 1 - o.uv_depth.y;
							#endif

							int index = 0;
							if (v.texcoord.x < 0.5 && v.texcoord.y < 0.5)
							{
								index = 0;
							}
							else if (v.texcoord.x > 0.5 && v.texcoord.y < 0.5)
							{
								index = 1;
							}
							else if (v.texcoord.x > 0.5 && v.texcoord.y > 0.5)
							{
								index = 2;
							}
							else
							{
								index = 3;
							}
							#if UNITY_UV_STARTS_AT_TOP
							if (_MainTex_TexelSize.y < 0)
								index = 3 - index;
							#endif
							o.interpolatedRay = _FrustumCornersRay[index];

							return o;
						}


						fixed4 frag(v2f i) : SV_Target
						{
							float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth);
							float linearDepth = LinearEyeDepth(d);
						float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.interpolatedRay.xyz;
						float4 currentPos = float4(i.uv.x * 2 - 1, i.uv.y * 2 - 1, d * 2 - 1, 1);
						float4 previousPos = mul(_PreviousViewProjectionMatrix,worldPos);
						
						previousPos /= previousPos.w;
						float2 velocity = (currentPos.xy - previousPos.xy) / 2.0f;
						float2 uv = i.uv;
						float4 c = tex2D(_MainTex, uv);
						uv += velocity * _BlurSize;
						for (int it = 1; it < 3; ++it, uv += velocity *_BlurSize)
						{
							float4 currentColor = tex2D(_MainTex, uv);
							c += currentColor;
						}
						c /= 3;

						return fixed4(c.rgb,1.0);
						}
							ENDCG
							}

					
		}
		FallBack Off
}

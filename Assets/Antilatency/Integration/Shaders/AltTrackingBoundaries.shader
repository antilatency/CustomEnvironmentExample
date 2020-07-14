Shader "Antilatency/Playzone" {
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.03
		_DrawDistance ("Draw distance", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching" = "True" }
		LOD 200
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 worldSpacePosition : TEXCOORD2;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float3 worldSpacePosition : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			float4 _Color;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float3 _Cutoff;
			float3 _UserPosition;

			float _DrawDistance;
			
			v2f vert (appdata v) {
				v2f o;

				float3 userLS = mul(unity_WorldToObject, float4(_UserPosition, 1)).xyz;
				float3 toUser = userLS - v.vertex.xyz;
				float3 vX = normalize(cross(v.normal.xyz, float3(0, 1, 0))) * v.uv2.x;
				float3 vY = float3(0,1,0) * v.uv2.y;

				float vXLength = length(vX);
				float vYLength = length(vY);

				float projectionX = dot(toUser, vX) / vXLength;
				float projectionY = dot(toUser, vY) / vYLength;

				float dist = abs(dot(v.normal.xyz, toUser));

				if (projectionX < -_DrawDistance || projectionY < -_DrawDistance || projectionX > vXLength + _DrawDistance || projectionY > vYLength + _DrawDistance || dist > _DrawDistance) {
					o.vertex = UnityObjectToClipPos(float4(0, 0, 0, 0));
				} else {
					o.vertex = UnityObjectToClipPos(v.vertex);
				}

				o.normal = v.normal.xyz;
				
				float3 n = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz).xyz);
				float3 vDirection = normalize(float3(0, 1, 0));
				float3 uDirection = normalize(cross(n, vDirection));
				float3 worldSpace = mul(unity_ObjectToWorld, v.vertex);
				
				o.worldSpacePosition = worldSpace;

				o.uv.xy = float2(dot(worldSpace.xyz, uDirection), dot(worldSpace.xyz, vDirection));
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target	{
				//return 1;
				fixed4 c = tex2D (_MainTex, i.uv) * _Color;
			
				float dist = clamp ((_DrawDistance - 0.0001f) - length(i.worldSpacePosition.xz - _UserPosition.xz), 0, 1);

				c.a = dist * c.a;
				clip( c.a - _Cutoff);

				return c;
			}
			ENDCG
		}
	}
}

Shader "Antilatency/Sky"
{
	Properties
	{
		_HorizonColor ("Horizon", Color) = (1, 1, 1, 0)
        _ZenithColor ("Zenith", Color) = (1, 1, 1, 0)

		_Distribution ("Distribution", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Background" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 direction : TEXCOORD0;
			};

			struct v2f
			{
				float3 direction : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			float4 _HorizonColor;
			float4 _ZenithColor;
			float  _Distribution;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.direction = v.direction;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target{

				float a = asin(normalize(i.direction).y) / (0.5*3.1415926535897932384626433832795);
				a = clamp(a,0,1);



				
				return lerp(_HorizonColor,_ZenithColor,pow(a,_Distribution));
			}
			ENDCG
		}
	}
}

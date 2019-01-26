Shader "Sprites/SpriteOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Width("Width", float) = 0
		_Height("Height", float) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Width;
			float _Height;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_MainTex, i.uv);

				fixed4 pixelTop = tex2D(_MainTex, i.uv + fixed2(0, _Height));
				fixed4 pixelBottom = tex2D(_MainTex, i.uv + fixed2(0, -_Height));
				fixed4 pixelLeft = tex2D(_MainTex, i.uv + fixed2(_Width, 0));
				fixed4 pixelRight = tex2D(_MainTex, i.uv + fixed2(-_Width, 0));

				fixed4 pixelTopLeft = tex2D(_MainTex, i.uv + fixed2(_Width, _Height));
				fixed4 pixelTopRight = tex2D(_MainTex, i.uv + fixed2(-_Width, _Height));
				fixed4 pixelBottomLeft = tex2D(_MainTex, i.uv + fixed2(_Width, -_Height));								
				fixed4 pixelBottomRight = tex2D(_MainTex, i.uv + fixed2(-_Width, -_Height));


				col = pixelBottom + pixelTop + pixelLeft + pixelRight + pixelTopRight + pixelTopLeft + pixelTopRight + pixelBottomLeft + pixelBottomRight;
				if(col.a >0)
				{						
					col = _Color;
					if(col2.a>0)
					{
						col.a = 0;
					}
				}
				return col;

			}
			ENDCG
		}
	}
}

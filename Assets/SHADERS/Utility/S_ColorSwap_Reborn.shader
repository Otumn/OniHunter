Shader "Color/ColorSwap_Reborn"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ArrayLength ("Array Length", float) = 0
		 //_OutputColors( "Output", float[]) = new float[]

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
			float4 _OutputColors[10];
			float4 _TargetColors[10];
			float _Tolerance[10];
			float _ArrayLength;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if(col.a == 0)
				{
					return col;
				}
				for (int i = 0; i < _ArrayLength+1; i++)
				{
					if((col.r <= _TargetColors[i].r + _Tolerance[i] && col.r >= _TargetColors[i].r - _Tolerance[i]) && (col.g <= _TargetColors[i].g + _Tolerance[i] && col.g >= _TargetColors[i].g - _Tolerance[i]) && (col.b <= _TargetColors[i].b + _Tolerance[i] && col.b >= _TargetColors[i].b - _Tolerance[i]))
					{		
						return _OutputColors[i];
						//return col;
					}
				}
				
				return col;
			}
			ENDCG
		}
	}
}

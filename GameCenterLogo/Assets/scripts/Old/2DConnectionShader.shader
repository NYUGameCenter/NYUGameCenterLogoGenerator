// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/2DConnectionShader"
{
	Properties
	{
		_Radius ("Radius", Float) = 1.117
		_MainTex ("Texture", 2D) = "white" {}
    	_Point1 ("Point1", Vector) = (0,0,0,0)
    	_Point2 ("Point2", Vector) = (0,0,0,0)
	}
	SubShader
	{ 
	    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	    LOD 200
	    Blend One One
	    ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
//				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 position_in_world_space: TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Point1;
			float4 _Point2;
			float4 _Point3;
			float4 _Point4;
			float4 _Point5;
			float4 _Point6;
			float _Radius;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
//				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//				UNITY_TRANSFER_FOG(o,o.vertex);
				o.position_in_world_space = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = float4(1, 1, 1, 1);

				if( (distance(_Point1, i.position_in_world_space) <= _Radius) || 
					(distance(_Point2, i.position_in_world_space) <= _Radius)){
					col = float4(0, 0, 0, 0);
				}

				return col;
			}
			ENDCG
		}
	}
}

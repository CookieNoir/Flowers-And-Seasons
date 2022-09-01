Shader "Custom/Island Background"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SeasonsColorRamp("Seasons Color Ramp", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ZTest On
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 seasonsUv : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SeasonsColorRamp;
			uniform sampler2D _Seasons;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4 objectOrigin = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));
				o.seasonsUv = float2(objectOrigin.x / 16., objectOrigin.z / 16.);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed season = tex2D(_Seasons, i.seasonsUv).r;
				fixed4 col = tex2D(_MainTex, i.uv) * tex2D(_SeasonsColorRamp, float2(season, 0.5));
				return col;
			}
			ENDCG
		}
	}
}

Shader "Custom/Island Foreground"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Spring ("Spring Texture", 2D) = "white" {}
        _Summer ("Summer Texture", 2D) = "white" {}
        _Autumn ("Autumn Texture", 2D) = "white" {}
        _Winter ("Winter Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off
        ZTest On
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 worldUv : TEXCOORD1;
                float2 seasonsUv : TEXCOORD2;
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                LIGHTING_COORDS(3, 4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Spring;
            sampler2D _Summer;
            sampler2D _Autumn;
            sampler2D _Winter;
            float4 _Spring_ST;
            uniform sampler2D _Seasons;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal, 1));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
                o.worldUv = float2(worldPosition.x / 2., worldPosition.z / 2.);
                float4 objectOrigin = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));
                o.seasonsUv = float2(objectOrigin.x / 16., objectOrigin.z / 16.);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed season = tex2D(_Seasons, i.seasonsUv).r * 4;
                half texSpring = max(1. - abs(season), 0) + max(1. - abs(season - 4.), 0);
                half texSummer = max(1. - abs(season - 1.), 0);
                half texAutumn = max(1. - abs(season - 2.), 0);
                half texWinter = max(1. - abs(season - 3.), 0);
                fixed4 col = texSpring * tex2D(_Spring, i.worldUv) +
                    texSummer * tex2D(_Summer, i.worldUv) +
                    texAutumn * tex2D(_Autumn, i.worldUv) +
                    texWinter * tex2D(_Winter, i.worldUv);
                fixed3 lighting = i.diff * LIGHT_ATTENUATION(i) + i.ambient;
                col.rgb *= lighting;
                col.a = tex2D(_MainTex, i.uv).a;
                return col;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}
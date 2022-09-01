Shader "Custom/Flower"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal: NORMAL;
                float4 color : COLOR0;
                fixed3 diff : COLOR1;
                fixed3 ambient : COLOR2;
                LIGHTING_COORDS(0, 1)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal, 1));
                o.color = v.color;
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = i.color;
                fixed3 lighting = i.diff * LIGHT_ATTENUATION(i) + i.ambient;
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }

        Pass
            {
                Tags { "LightMode" = "ShadowCaster" }

                Cull Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0


                struct appdata
                {
                    float4 vertex : POSITION;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    return 0;
                }
                ENDCG
            }
    }
}

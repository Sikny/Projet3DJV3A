﻿Shader "Unlit/difficulty"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 col: COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 col:COLOR;
                float4 vertex : SV_POSITION;
                float4 global : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                float4 couleur;
                if(v.col.r < 1/4.0){
                    couleur = float4(0,0,0.5,0);
                }else if(v.col.r < 2/4.0){
                    couleur = float4(0,0.5,0,0);
                }else if(v.col.r < 3/4.0){
                    couleur = float4(0.5,0,0,0);
                }else if(v.col.r < 1){
                    couleur = float4(0,0,0,0);
                }
                o.col = couleur;
                o.global = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                fixed4 col = i.col;
                if(!(-0.5 < i.global.y && i.global.y < 0.5)){
                    col = float4(0,0,0,0);
                }
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

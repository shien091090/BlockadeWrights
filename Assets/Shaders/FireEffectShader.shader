Shader "SNShien/Fire"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _SubTex ("Sub Texture", 2D) = "white" {}
        _DistortValue ("Distort Value", Range(0, 1)) = 0.5
        _TimeSpeed("Time Speed", float) = 0.5

        //============================DefaultSetting==================
        [Header(Blending)]
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Int) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Int) = 10

    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"
        }
        Blend [_BlendSrc] [_BlendDst]

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
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _SubTex;
            float4 _MainTex_ST;
            float _DistortValue;
            float _TimeSpeed;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float timeSpeed = _Time.x * _TimeSpeed;
                float distortValue = (tex2D(_SubTex, float2(i.uv.x + timeSpeed, i.uv.y)).a - 0.5f) * _DistortValue;
                fixed4 col = tex2D(_MainTex, float2(i.uv.x + distortValue, i.uv.y));
                return col;
            }
            ENDCG
        }
    }
}
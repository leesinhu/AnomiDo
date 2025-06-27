Shader "Unlit/Stencil_Shadow"
{
    Properties
    {
        _Color("Base Color", Color) = (1, 1, 1, 1) // 기본 색상
        _MainTex("Base (RGB)", 2D) = "white" {}   // 텍스처
    }

    SubShader
    {
        Tags 
        {
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
        }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp NotEqual
            }

            ZWrite Off
            ZTest Lequal

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_black
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag_black(v2f i) : SV_Target
            {
                return fixed4(0, 0, 0, 1); // 검은색 출력
            }

            ENDCG
        }

        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
            }

            Stencil
            {
                Ref 1        // 1번으로 설정
                Comp Equal   // 스텐실 버퍼 값이 1인 픽셀에만 그린다
                Pass Replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 기본 색상 (_Color) 속성, 텍스처 (_MainTex) 속성
            sampler2D _MainTex;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 텍스처 색상과 기본 색상 곱셈
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // 알파 값이 0에 가까운 경우 픽셀을 그리지 않음
                if (texColor.a < 0.2) discard;

                return texColor;
            }

            ENDCG
        }
    }
}

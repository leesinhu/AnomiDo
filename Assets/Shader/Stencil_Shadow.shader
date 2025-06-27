Shader "Unlit/Stencil_Shadow"
{
    Properties
    {
        _Color("Base Color", Color) = (1, 1, 1, 1) // �⺻ ����
        _MainTex("Base (RGB)", 2D) = "white" {}   // �ؽ�ó
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
                return fixed4(0, 0, 0, 1); // ������ ���
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
                Ref 1        // 1������ ����
                Comp Equal   // ���ٽ� ���� ���� 1�� �ȼ����� �׸���
                Pass Replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // �⺻ ���� (_Color) �Ӽ�, �ؽ�ó (_MainTex) �Ӽ�
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
                // �ؽ�ó ����� �⺻ ���� ����
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // ���� ���� 0�� ����� ��� �ȼ��� �׸��� ����
                if (texColor.a < 0.2) discard;

                return texColor;
            }

            ENDCG
        }
    }
}

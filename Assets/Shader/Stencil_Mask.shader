Shader "Unlit/Stencil_Mask"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Geometry-1"
        }
        Pass
        {
            Stencil
            {
                Ref 1        
                Comp Never   
                Fail Replace 
                Pass Replace
            }

             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"

             struct appdata
             {
                 float4 vertex : POSITION;
             };

             struct v2f
             {
                 float4 position : SV_POSITION;
             };

             v2f vert(appdata v)
             {
                 v2f o;
                 o.position = UnityObjectToClipPos(v.vertex);
                 return o;
             }

             half4 frag(v2f i) : SV_Target
             {
                 return half4(1, 1, 1, 1); // 색상은 단순히 흰색으로 설정
             }
             ENDCG
        }
    }
}

Shader "Custom/Stencil Writer"
{
    Properties
    {
        _StencilValue ("Stencil Value", Int) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
        Blend Zero One
        ZWrite Off
        
        Stencil
        {
            Ref [_StencilValue]
            Comp GEqual
            Pass Replace
        }

        Pass
        {

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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                return fixed4(0.0,0.0,0.0,0.0);
            }
            ENDCG
        }
    }
}

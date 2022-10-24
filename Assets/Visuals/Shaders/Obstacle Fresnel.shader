Shader "Unlit/Obstacle Fresnel"
{
    Properties
    {
        _Color1 ("Color Inner", Color) = (1,1,1,1)
        _Color2 ("Color Outer", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float fresnel : TEXCOORD0;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            uniform float _fresnelScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 i = normalize(ObjSpaceViewDir(v.vertex));
                o.fresnel = _fresnelScale * (1.0 + dot(i, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(_Color1, _Color2, i.fresnel);
                return col;
            }
            ENDCG
        }
    }
}

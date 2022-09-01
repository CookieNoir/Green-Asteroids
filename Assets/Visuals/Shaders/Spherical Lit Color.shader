Shader "Unlit/Spherical Lit Color"
{
    Properties
    {
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 direction : TEXCOORD1;
                float4 color : COLOR;
            };

            uniform float4 _SunDirection;
            uniform sampler2D _Shading;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dotProductDirection = 0.5 + dot(normalize(i.direction), _SunDirection) * 0.5;
                fixed4 col = i.color;
                col *= tex2D(_Shading, float2(dotProductDirection, 0.5));
                return col;
            }
            ENDCG
        }
    }
}

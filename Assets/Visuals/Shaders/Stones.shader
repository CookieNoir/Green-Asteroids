Shader "Unlit/Stones"
{
    Properties
    {
        _MainTex("Texture (1D)", 2D) = "white" {}
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
                float4 direction : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            sampler2D _MainTex;
            uniform sampler2D _Shading;
            uniform float4 _SunDirection;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex);
                o.normal = mul(unity_ObjectToWorld, v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 normalizedVertex = normalize(i.direction);
                float normalColor = dot(normalizedVertex, i.normal) * 0.5 + 0.5;
                float sunColor = dot(normalizedVertex, _SunDirection) * 0.5 + 0.5;

                fixed4 col = tex2D(_MainTex, float2(normalColor, 0.5))
                    *tex2D(_Shading, float2(sunColor, 0.5));
                return col;
            }
            ENDCG
        }
    }
}

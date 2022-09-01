Shader "Unlit/Spherical Normal Lit"
{
    Properties
    {
        _Color("Color", color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 direction : TEXCOORD2;
            };

            uniform float4 _SunDirection;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform sampler2D _Shading;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.direction = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dotProductNormal = 0.5 + i.normal.y * 0.5;
                float dotProductDirection = 0.5 + dot(normalize(i.direction), _SunDirection) * 0.5;
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                //col = float4(i.normal.x, i.normal.y, i.normal.z, 1);
                col *= tex2D(_Shading, float2(dotProductNormal, 0.5));
                col *= tex2D(_Shading, float2(dotProductDirection, 0.5));
                return col;
            }
            ENDCG
        }
    }
}
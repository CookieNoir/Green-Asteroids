Shader "Unlit/Star"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Configuration1 ("Configuration", Vector) = (1,1,1,1)
        _Configuration2 ("Configuration", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float intensity : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Configuration1;
            float4 _Configuration2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float4 origin = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));
                float max = _Configuration1.x + _Configuration1.z + _Configuration2.x;
                o.intensity.x = _Configuration1.x * cos(_Configuration1.y * origin.x)
                    + _Configuration1.z * sin(_Configuration1.w * origin.y)
                    + _Configuration2.x * cos(_Configuration2.y * origin.z);
                o.intensity.x = (o.intensity.x / max + 1.0) * 0.5;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= i.intensity.x;
                return col;
            }
            ENDCG
        }
    }
}

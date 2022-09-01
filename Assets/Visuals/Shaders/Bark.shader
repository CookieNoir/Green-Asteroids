Shader "Unlit/Bark"
{
    Properties
    {
        _MainTex ("Bark Color (1D)", 2D) = "white" {}
        _NormalColorPower("Normal Color Power", float) = 1.0
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
            float _NormalColorPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float normalColor = pow(dot(float3(0.0, 1.0, 0.0), i.normal) * 0.5 + 0.5, _NormalColorPower);
                float sunColor = dot(normalize(i.direction), _SunDirection) * 0.5 + 0.5;

                fixed4 col = tex2D(_MainTex, float2(normalColor, 0.5))
                    * tex2D(_Shading, float2(sunColor, 0.5));
                return col;
            }
            ENDCG
        }
    }
}

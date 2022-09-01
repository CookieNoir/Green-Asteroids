// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Rover"
{
    Properties
    {
        _MainTex ("Color Multiplier (1D)", 2D) = "white" {}
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 direction : TEXCOORD0;
                float3 normal : TEXCOORD1;
                fixed4 color : COLOR;
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
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float normalColor = pow(dot(i.direction.xyz, i.normal) * 0.5 + 0.5, _NormalColorPower);
                float sunColor = dot(normalize(i.direction), _SunDirection) * 0.5 + 0.5;

                fixed4 col = i.color * tex2D(_MainTex, float2(normalColor, 0.5))
                    * tex2D(_Shading, float2(sunColor, 0.5));
                return col;
            }
            ENDCG
        }
    }
}

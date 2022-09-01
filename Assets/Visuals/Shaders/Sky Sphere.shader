Shader "Unlit/Sky Sphere"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SkyColor1("Sky Color (0)", color) = (1,1,1,1)
        _SkyColor2("Sky Color (R)", color) = (1,1,1,1)
        _SkyColor3("Sky Color (RG)", color) = (1,1,1,1)
        _SunColor("Sun Color (RGB)", color) = (1,1,1,1)
        _SunInfluence("Sun Influence", float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 direction : TEXCOORD0;
            };

            sampler2D _MainTex;
            uniform float4 _SunDirection;
            fixed4 _SunColor;
            fixed4 _SkyColor1;
            fixed4 _SkyColor2;
            fixed4 _SkyColor3;
            float _SunInfluence;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dotProductDirection = 0.5 + dot(normalize(i.direction), _SunDirection) * 0.5;
                fixed4 mask = tex2D(_MainTex, float2(dotProductDirection, 0.5));
                float sunValue = pow(mask.b, _SunInfluence);
                fixed4 col = (1 - sunValue) * ((1.0 - mask.g) * ((1.0 - mask.r) * _SkyColor1 + mask.r * _SkyColor2) + mask.g * _SkyColor3) + sunValue * _SunColor;
                col.a *= mask.a;
                return col;
            }
            ENDCG
        }
    }
}

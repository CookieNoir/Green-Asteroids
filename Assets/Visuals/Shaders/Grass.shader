Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Grass Texture", 2D) = "white" {}
        _DirtTex ("Dirt Texture", 2D) = "white" {}
        _GrassColorsLower ("Grass Colors Lower (1D)", 2D) = "white" {}
        _GrassColorsUpper ("Grass Colors Upper (1D)", 2D) = "white" {}
        _GrassUpperPower ("Grass Upper Color Power", float) = 1.0
        _DirtColors ("Dirt Colors (1D)", 2D) = "white" {}
        _TextureScale ("Texture Scale", float) = 1.0
        _TriplanarPower ("Triplanar Power", float) = 1.0
        _ColorVariationNoise ("Color variation noise", vector) = (0,0,0,0)
        _MaskPower ("Mask Power", float) = 1.0
        _BlendPower ("Blend Power", float) = 1.0
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 direction : TEXCOORD0;
                float2 xy : TEXCOORD1;
                float2 zy : TEXCOORD2;
                float2 xz : TEXCOORD3;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _DirtTex;
            sampler2D _GrassColorsLower;
            sampler2D _GrassColorsUpper;
            float _GrassUpperPower;
            sampler2D _DirtColors;
            uniform sampler2D _Shading;
            uniform float4 _SunDirection;
            uniform float4 _NoiseX;
            uniform float4 _NoiseY;
            uniform float4 _NoiseZ;
            float _TextureScale;
            float _TriplanarPower;
            float4 _ColorVariationNoise;
            float _MaskPower;
            float _BlendPower;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex);
                o.xy = o.direction.xy * _TextureScale;
                o.zy = o.direction.zy * _TextureScale;
                o.xz = o.direction.xz * _TextureScale;
                o.color = v.color;
                return o;
            }

            float getNoise(float4 direction, float4 data)
            {
                return data.w
                * (sin(direction.x * data.x)
                + sin(direction.y * data.y)
                + sin(direction.z * data.z));
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 normalizedVertex = normalize(i.direction);
                float sunColor = dot(normalizedVertex, _SunDirection) * 0.5 + 0.5;

                float3 clippedPosition = pow(abs(normalizedVertex), _TriplanarPower);
                clippedPosition /= clippedPosition.x + clippedPosition.y + clippedPosition.z;

                float variation = 
                      _NoiseX.z * cos(i.direction.x * _NoiseX.x + _NoiseX.y)
                    + _NoiseY.z * cos(i.direction.y * _NoiseY.x + _NoiseY.y)
                    + _NoiseZ.z * cos(i.direction.z * _NoiseZ.x + _NoiseZ.y);
                variation /= _NoiseX.z + _NoiseY.z + _NoiseZ.z;
                variation = variation * 0.5 + 0.5;
                float variationNoise = getNoise(i.direction, _ColorVariationNoise);
                variation += variationNoise;

                fixed4 grass = clippedPosition.z * tex2D(_MainTex, i.xy)
                    + clippedPosition.x * tex2D(_MainTex, i.zy)
                    + clippedPosition.y * tex2D(_MainTex, i.xz);
                float grassValue = pow(grass.r, _GrassUpperPower);
                float2 uv = float2(variation, 0.5);
                fixed4 dirt = clippedPosition.z * tex2D(_DirtTex, i.xy)
                    + clippedPosition.x * tex2D(_DirtTex, i.zy)
                    + clippedPosition.y * tex2D(_DirtTex, i.xz);

                float maskValue = i.color.r * i.color.r * (3 - 2 * i.color.r);
                float blendValue = pow(clamp(maskValue + grass.a * pow(cos(3.14159 * (maskValue - 0.5)), _MaskPower), 0, 1), _BlendPower);
                
                fixed4 col = ((1.0 - blendValue) * (dirt * tex2D(_DirtColors, uv))
                + (blendValue) * (((1.0 - grassValue) * tex2D(_GrassColorsLower, uv)
                    + grassValue * tex2D(_GrassColorsUpper, uv))))
                * tex2D(_Shading, float2(sunColor, 0.5));
                return col;
            }
            ENDCG
        }
    }
}

Shader "Unlit/Simple Plant"
{
	Properties
	{
		_MainTex("Normal Color (1D)", 2D) = "white" {}
		_UVLight("UV Light (1D)", 2D) = "white" {}
		_NormalColorPower("Normal Color Power", float) = 1.0
		_UVLightPower("UV Light Power", float) = 1.0
		_WindInfluence("Wind Influence", float) = 1.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			Pass
			{
				Tags {"LightMode" = "ShadowCaster"}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					V2F_SHADOW_CASTER;
				};

				uniform float4 _WindProperties;
				float _WindInfluence;

				v2f vert(appdata v)
				{
					v2f o;
					float4 direction = mul(unity_ObjectToWorld, v.vertex);
					float windValue = v.uv.y * _WindInfluence;
					v.vertex += windValue * float4(
						_WindProperties.w * sin(_WindProperties.x * direction.x + _Time.y),
						_WindProperties.w * sin(_WindProperties.y * direction.y + _Time.y),
						_WindProperties.w * sin(_WindProperties.z * direction.z + _Time.y),
						0.0);
					TRANSFER_SHADOW_CASTER(o)
					return o;
				}

				float4 frag(v2f i) : COLOR
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG
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
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 direction : TEXCOORD1;
					float3 normal : TEXCOORD2;
				};

				sampler2D _MainTex;
				sampler2D _UVLight;
				uniform sampler2D _Shading;
				uniform float4 _SunDirection;
				float _NormalColorPower;
				float _UVLightPower;
				uniform float4 _WindProperties;
				float _WindInfluence;

				v2f vert(appdata v)
				{
					v2f o;
					o.uv = v.uv;
					o.direction = mul(unity_ObjectToWorld, v.vertex);

					float windValue = v.uv.y * _WindInfluence;
					o.vertex = UnityObjectToClipPos(v.vertex + windValue * float4(
					   _WindProperties.w * sin(_WindProperties.x * o.direction.x + _Time.y),
					   _WindProperties.w * sin(_WindProperties.y * o.direction.y + _Time.y),
					   _WindProperties.w * sin(_WindProperties.z * o.direction.z + _Time.y),
					   0.0));

					o.normal = v.normal;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float normalColor = pow(dot(float3(0.0, 1.0, 0.0), i.normal) * 0.5 + 0.5, _NormalColorPower);
					float sunColor = dot(normalize(i.direction), _SunDirection) * 0.5 + 0.5;

					fixed4 col = tex2D(_MainTex, float2(normalColor, 0.5))
						* tex2D(_Shading, float2(sunColor, 0.5))
						* tex2D(_UVLight, float2(pow(i.uv.y, _UVLightPower), 0.5));
					return col;
				}
				ENDCG
			}
		}
}

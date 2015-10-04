Shader "UnityRose/TreeWind"
{

	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_LightTex("Light (RGB)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_WindDir("Wind Forces", Vector) = (1,0,1,0)
		_Color("Main Color", Color) = (1,1,1,1)
		_Velocity("Wind Velocity", Range(0,200)) = 15
		_Elasticity("Tree elasticity", Range(-10,10)) = 0.5

	}

	SubShader
	{
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _LightTex;
			fixed4 _Color;
			fixed4 _WindDir;
			float _Cutoff;
			float _Velocity;
			float _Elasticity;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};


			float hash(float n)
			{
				return frac(sin(n)*43758.5453);
			}

			float noise(float3 x)
			{
				// The noise function returns a value in the range 0 -> 1.0f

				float3 p = floor(x);
				float3 f = frac(x);

				f = f*f*(3.0 - 2.0*f);
				float n = p.x + p.y*57.0 + 113.0*p.z;

				float result = lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
					lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
					lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
						lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);

				return (result + 1.0) / 2.0;
			}

			float GetWindLevel(float3 v, float3 pos) {
				float time = _Time * _Velocity;
				float3 noiseInput = 0;
				noiseInput.x = pos.x + v.x + time;
				noiseInput.y = pos.z + v.y + time;
				float micro = _Elasticity * sin(noise(noiseInput));
				time /= 4.0f;

				noiseInput.x = pos.x / 10.0f + v.x + time;
				noiseInput.y = pos.z / 10.0f + v.y + time;

				float macro = micro * pow(noise(noiseInput), 4.0f);
				return macro;
			}

			
			v2f vert (appdata v)
			{
				v2f o;
				float3 pos = mul(_Object2World, float4(0, 0, 0, 1)).xyz;
				float wind = GetWindLevel(v.vertex, pos);
				v.vertex = v.vertex + wind*_WindDir;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.uv2 = v.uv2;
				return o;
			}
			



			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv)*_Color;
				fixed4 light = tex2D(_LightTex, i.uv2);
				if (col.a < _Cutoff) discard;
				return col*light*2.0;
			}
			ENDCG
		}
	}
}

Shader "Custom/Glow" {
	Properties {
		_ColorTint("Color Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(1.0, 6.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float4 color : Color;
			float2 uv_MainTex;
			float3 viewDir;
		};
		
		float4 _ColorTint;
		sampler2D _MainTex;
		float4 _RimColor;
		float _RimPower;


		void surf (Input IN, inout SurfaceOutput o) {
			IN.color = _ColorTint;
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * IN.color;
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

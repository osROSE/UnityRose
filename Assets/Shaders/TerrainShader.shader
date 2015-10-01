// <copyright file="TerrainShader.shader" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

Shader "Custom/TerrainShader" {
	Properties {
		_BottomTex ("Bottom (RGBA)", 2D) = "white" {}
		_TopTex ("Top (RGBA)", 2D) = "black" {}
		_LightTex ("Light (RGBA)", 2D) = "white" {}
	}
	SubShader {
		Tags { "IgnoreProjector"="True" "RenderType"="Opaque" }

		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _BottomTex;
		sampler2D _TopTex;
		sampler2D _LightTex;

		struct Input {
			float2 uv_BottomTex;
			float2 uv2_LightTex;
		};

		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 bottom =  tex2D (_BottomTex, IN.uv_BottomTex);
			half4 top = tex2D (_TopTex, IN.uv_BottomTex);
			half4 light = tex2D (_LightTex, IN.uv2_LightTex);
			o.Albedo = bottom.rgb*(1.0f - top.a) + top.rgb*top.a;
			//o.Emission = o.Albedo*light*2.0;
			o.Albedo = o.Albedo*light*2.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

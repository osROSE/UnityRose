// <copyright file="TerrainShader2.shader" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

Shader "Custom/TerrainShader2" {
	Properties {
		_BottomTex ("Bottom (RGBA)", 2D) = "white" {}
		_TopTex ("Top (RGBA)", 2D) = "white" {}
		_LightTex ("Light (RGBA)", 2D) = "white" {}
		_NormalMapTop ("Normals (RGBA)", 2D) = "white" {}
		_NormalMapBottom ("Normals (RGBA)", 2D) = "white" {}
	}
	SubShader {
		Tags { "IgnoreProjector"="True" "RenderType"="Opaque" }

		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _BottomTex;
		sampler2D _TopTex;
		sampler2D _LightTex;
		//sampler2D _NormalMapTop;
		//sampler2D _NormalMapBottom;
		

		struct Input {
			float2 uv_BottomTex;
			float2 uv2_TopTex;
			//float2 uv_NormalMapBottom;
			//float2 uv2_NormalMapTop;
			float4 color : COLOR; // actually used for uv_Light
		};

		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 bottom =  tex2D (_BottomTex, IN.uv_BottomTex);
			half4 top = tex2D (_TopTex, IN.uv2_TopTex);
			//half4 nbottom = tex2D(_NormalMapBottom, IN.uv_NormalMapBottom);
			//half4 ntop = tex2D(_NormalMapTop, IN.uv2_NormalMapTop);
			
			
			// Get lightmap UV from color input
			float2 uvLight = IN.color.rg;
			half4 light = tex2D (_LightTex, uvLight);

			// Unpack normal
			
			//o.Normal = UnpackNormal( ntop );// nbottom*(1.0f - ntop.a) + ntop*ntop.a );
			o.Albedo = bottom.rgb*(1.0f - top.a) + top.rgb*top.a;
			o.Emission = o.Albedo*light*1.5;
			//o.Albedo = o.Albedo*light*2.0;
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

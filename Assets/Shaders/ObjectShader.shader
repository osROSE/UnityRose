Shader "Custom/ObjectShader" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_LightTex ("Light (RGB)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_Color ("Main Color", Color) = (1,1,1,1)
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100
	
	
	// Lightmapped, encoded as dLDR
	Pass {
		Alphatest Greater [_Cutoff]
		AlphaToMask True
		ColorMask RGB
		
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		SetTexture [_LightTex] {
			
			constantColor [_Color]
			combine texture * constant
		}
		SetTexture [_MainTex] {
			combine texture * previous DOUBLE, texture * primary
		}
	}
	
}

}

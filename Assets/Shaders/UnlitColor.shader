Shader "Custom/Unlit" {
 
	Properties {
		_Color ("Color", Color) = (1,1,1)
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry" }
		Color [_Color]
		Pass {}
	}
 
}
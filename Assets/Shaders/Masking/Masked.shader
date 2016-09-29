Shader "Masking/Masked"
{

Properties
{
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_StencilID ("Stencil ID", Int) = 0
}

SubShader
{
	Tags { "RenderType"="Opaque" "Queue"="Geometry" }

	Stencil
	{
		Ref [_StencilID]
		Comp equal
		Pass keep
	}

	Cull Off
	// ZWrite Off
	// Blend SrcAlpha OneMinusSrcAlpha
	Lighting Off

	CGPROGRAM
	#pragma surface surf NoLighting

	sampler2D _MainTex;
	fixed4 _Color;

	struct Input
	{
		float2 uv_MainTex;
	};

	fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
     {
		 return _Color;
     }

	void surf (Input IN, inout SurfaceOutput o)
	{
		o.Albedo = _Color.rgb;
		o.Alpha = 1;
	}
	
	ENDCG
}

Fallback "VertexLit"
}

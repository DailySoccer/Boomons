// Simplified Bumped shader. Differences from regular Bumped one:
// - no Main Color
// - Normalmap uses Tiling/Offset of the Base texture
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/Colored Overlay" {
Properties {
	_Color("Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}

//  _BumpMap ("Normalmap", 2D) = "bump" {}
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 250

CGPROGRAM
#pragma surface surf Lambert noforwardadd

float4 _Color;
sampler2D _MainTex;
// sampler2D _BumpMap;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

	o.Albedo.r = (c.r < 0.5) ? c.r * _Color.r * 2 : 1 - (2 * (1 - c.r) * (1 - _Color.r));
	o.Albedo.g = (c.g < 0.5) ? c.g * _Color.g * 2 : 1 - (2 * (1 - c.g) * (1 - _Color.g));
	o.Albedo.b = (c.b < 0.5) ? c.b * _Color.b * 2 : 1 - (2 * (1 - c.b) * (1 - _Color.b));

	o.Alpha = c.a;
//	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
}
ENDCG  
}

FallBack "Mobile/Diffuse"
}

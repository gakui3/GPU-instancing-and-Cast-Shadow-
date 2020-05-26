// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Computeshader/ParticleRenderer"
{

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	CGINCLUDE		
	#include "UnityCG.cginc"
	#include "AutoLight.cginc"

	struct Particle
	{
		float3 position;
		float2 uv;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float2 uv : TEXCOORD2;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	StructuredBuffer<Particle> _Particles;
	sampler2D _Tex;
			
	v2f vert (uint id : SV_VertexID)
	{
		v2f o;
		float4 _pos = float4(_Particles[id].position, 1);
		o.pos = UnityObjectToClipPos(_pos);
		o.uv = _Particles[id].uv;

		return o;
	}
			
	float4 frag (v2f i) : SV_Target
	{
//		return float4(i.pos.x/_ScreenParams.x, i.pos.y/_ScreenParams.y, 1, 1);
//		return float4(i.uv, 0, 1);
		return tex2D(_Tex, i.uv);
	}

//	v2f shadow_vert (appdata_full v) {
//		v2f o;
//		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
//		return o;
//	}
//			
//	float4 shadow_frag (v2f i) : SV_Target {
//		SHADOW_CASTER_FRAGMENT(i)
//	}

	ENDCG

	SubShader
	{

		Pass
		{
			Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
			LOD 100

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

//		Pass
//		{
//			Name "ShadowCaster"
//			Tags { "LightMode" = "ShadowCaster" }
//			ZWrite On ZTest LEqual
//
//			CGPROGRAM
//			#pragma vertex shadow_vert
//			#pragma shadow_fragment frag
//			#pragma multi_compile_shadowcaster
//			ENDCG
//		}
	}
	Fallback "Diffuse"
}

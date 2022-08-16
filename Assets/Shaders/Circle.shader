Shader "Learn/Circle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius ("Radius", float) = 0
		//_Center("Center", vector) = (500, 500, 0, 0)
		_CenterX("CenterX", float) = 0
		_CenterY("CenterY", float) = 0
	}
	SubShader
	{ 
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Opaque"
			//"UniversalMaterialType" = "Lit"
			//"Queue" = "Geometry"
		}

		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			
			CBUFFER_START(UnityPerMaterial)
				float _Radius;
				//float2 _Center;
				float _CenterX;
				float _CenterY;
				float4 _MainTex_TexelSize;
				float4 _MainTex_ST;
			CBUFFER_END

			sampler2D _MainTex;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				// get the source color of screen
				half4 col = tex2D(_MainTex, i.uv);
				// center pixel
				//half2 center = _MainTex_TexelSize.zw / 2;
				half2 pixelPos = i.uv * _MainTex_TexelSize.zw;
				// distance to the center by pixel
				half2 center = half2(_CenterX, _CenterY);
				half dist = distance(center, pixelPos);
				// get the radius by pixels
				half radius = _Radius * _MainTex_TexelSize.z;

				// 方法一 如果像素在半径内 输出原图；如果在外，输出黑色
				// 这样很简单，但是很粗暴，边缘全是锯齿
				// if (dist < radius) return col; else return 0;
				// 方法一结束

				// 方法二 纯数值算法，避免if语句带来的边缘锯齿
				float f = clamp(radius - dist, 0, 1);
				return col * f;
				// 方法二 结束
			}
			ENDHLSL
		}
	}
}

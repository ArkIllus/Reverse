Shader "Learn/OriTitle"
{
    Properties
    {
        //_BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("albedo", 2D) = "white" {}
        _MaskTex("anim mask", 2D) = "white" {}
        _SinMax("SinMax", float) = 0.2
        _DisFactor("DisFactor", float) = 0.2
        _TimeFactor("TimeFactor", float) = 0.2
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

        Pass
        {
            //Name "ForwardLit"
            //Tags{ "LightMode" = "UniversalForward" } //错误
            //Tags{ "LightMode" = "Universal2D" } //对的

            //Cull Back
            //ZTest LEqual
            //ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _MainTex_ST;
                float4 _MaskTex_ST;

                float _SinMax;
                float _DisFactor;
                float _TimeFactor;
            CBUFFER_END

            // all sampler2D don't need to put inside CBUFFER
            sampler2D _MainTex;
            sampler2D _MaskTex;
            //TEXTURE2D(_MainTex);
            //SAMPLER(sampler_MainTex);
            //TEXTURE2D(_MaskTex);
            //SAMPLER(sampler_MaskTex);

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


            float Remap(float old1 ,float old2,float new1,float new2,float x)
            {
                return new1 + (x - old1) * (new2 - new1) / (old2 - old1);
            }

            v2f vert(appdata v)
            {
                //v2f o;
                v2f o = (v2f)0;

                //o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex = TransformObjectToHClip(v.vertex);
                //[注] Shader warning: 'TransformObjectToHClip': implicit truncation of vector type at line 69 (on d3d11)
                o.vertex = TransformObjectToHClip(v.vertex.xyz);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                return o;
            }

            //[注] HLSL 中 scalar 标量类型是没有 fixed
            half4 frag(v2f i) : SV_Target
            {
                //[注] 应用_MaskTex的offset
                float2 uvNew = TRANSFORM_TEX(i.uv, _MaskTex);
                half4 mask = tex2D(_MaskTex, uvNew);
                //half4 mask = tex2D(_MaskTex, i.uv);

                float R = Remap(0.3 , 0.9, 0, 1, mask.r);
                float centerX = (1 - R) * i.uv.x + R;
                float centerY = clamp(R - 0.3, 0, 0.6);
                float2 mid = float2(centerX, centerY);

                float2 fragDir = normalize(i.uv - mid);   //片段和uv波动中心的方向
                float dis = distance(mid, i.uv);          //片段与uv波动中心的距离

                i.uv += _SinMax * sin(_Time.y * _TimeFactor + dis * _DisFactor) * fragDir * mask.r * mask.r;
                //fixed4 col = tex2D(_MainTex, i.uv);
                half4 col = tex2D(_MainTex, i.uv);

                return col;

                ////half4 col = tex2D(_MainTex, i.uv);
                //half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                //return col;

                //return _BaseColor;
            }
            ENDHLSL
        }
    }
}
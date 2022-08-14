Shader "Learn/2DBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius("BlurRadius", float) = 5.0 //模糊半径
        _TextureSize("TextureSize", float) = 640
    }
    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalRenderPipeline"
            //"RenderType" = "Transparent"
            ////"UniversalMaterialType" = "Lit"
            //"Queue" = "Transparent"
            //"IgnoreProjector" = "true"
        }
        Cull Off
        ZWrite Off
        ZTest Always
        //Blend SrcAlpha OneMinusSrcAlpha

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        CBUFFER_START(UnityPerMaterial)
            float _BlurRadius;
            //float4 _MainTex_TexelSize;
            //float4 _MainTex_ST;
            float _TextureSize;
        CBUFFER_END
        sampler2D _MainTex; 
        //TEXTURE2D(_MainTex);
        //SAMPLER(sampler_MainTex);

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
        ENDHLSL

        Pass
        {
            //Tags{ "LightMode" = "UniversalForward" } //错误
            //Tags{ "LightMode" = "Universal2D" } //对的

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            //TODO: 可以用计算出来的常量替代，不需要在循环中每一步计算
            float GetGaussWeight(float x, float y, float sigma) 
            {
                float sigma2 = pow(sigma, 2.0f);
                float left = 1 / (2 * sigma2 * 3.1415926f);
                float right = exp(-(x * x + y * y) / (2 * sigma2));
                return left * right;
            }

            half4 GaussBlur(float2 uv) 
            {
                float sigma = (float)_BlurRadius / 3.0f;
                float4 col = float4(0, 0, 0, 0);
                for (int x = -_BlurRadius; x <= _BlurRadius; ++x)
                {
                    for (int y = -_BlurRadius; y <= _BlurRadius; ++y)
                    {
                        float4 color = tex2D(_MainTex, uv + float2(x / _TextureSize, y / _TextureSize));
                        float weight = GetGaussWeight(x, y, sigma);
                        col += color * weight;
                    }
                }
                return col;
            }

            half4 SimpleBlur(float2 uv)
            {
                float4 col = float4(0, 0, 0, 0);
                for (int x = -_BlurRadius; x <= _BlurRadius; ++x)
                {
                    for (int y = -_BlurRadius; y <= _BlurRadius; ++y)
                    {
                        float4 color = tex2D(_MainTex, uv + float2(x / _TextureSize, y / _TextureSize));
                        col += color;
                    }
                }
                col = col / pow(_BlurRadius * 2 + 1, 2.0f);
                return col;
            }

            v2f vert (appdata v)
            {
                v2f o;
                //o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                //half4 col = GaussBlur(i.uv);
                half4 col = SimpleBlur(i.uv);
                return col;
            }
            ENDHLSL
        }
    }






    //Properties
    //{
    //    //[HideInInspector] 
    //    _MainTex("Texture", 2D) = "white" {}
    //    //_BlurSize("Blur Size", Float) = 1.0 //改名
    //    _BlurRadius("BlurRadius", Float) = 1
    //}
    //SubShader
    //{
    //    Tags
    //    {
    //        "RenderPipeline" = "UniversalRenderPipeline"
    //    }
    //    Cull Off
    //    ZWrite Off
    //    ZTest Always

    //    HLSLINCLUDE
    //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    //    CBUFFER_START(UnityPerMaterial)
    //    float _BlurRadius;
    //    float4 _MainTex_TexelSize;
    //    CBUFFER_END
    //    TEXTURE2D(_MainTex);
    //    SAMPLER(sampler_MainTex);
    //    struct Attributes
    //    {
    //        float4 positionOS: POSITION;
    //        float2 texcoord: TEXCOORD0;
    //    };
    //    struct Varyings
    //    {
    //        float4 positionCS:SV_POSITION;
    //        float2 uv:TEXCOORD0;
    //    };
    //    ENDHLSL

    //    Pass
    //    {
    //        HLSLPROGRAM
    //        #pragma vertex vert
    //        #pragma fragment frag

    //        Varyings vert(Attributes input)
    //        {
    //            Varyings output;
    //            output.positionCS = TransformObjectToHClip(input.positionOS);
    //            output.uv = input.texcoord;
    //            return output;
    //        }

    //        half4 frag(Varyings input) : SV_Target
    //        {
    //            half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
    //            float2 uv = input.uv;
    //            half4 col = float4(0,0,0,0);
    //            col += 0.060 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-1,-1) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.098 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0,-1) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.060 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(1,-1) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.098 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-1,0) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.162 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    //            col += 0.098 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(1,0) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.060 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(1,-1) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.022 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(1,0) * _MainTex_TexelSize.xy * _BlurRadius);
    //            col += 0.060 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(1,1) * _MainTex_TexelSize.xy * _BlurRadius);

    //            return col;
    //        }
    //        ENDHLSL
    //    }
    //}




    //Properties{
    //    //_MainTex("Base (RGB)", 2D) = "" {}
    //    _BlurTexture("Base (RGB)", 2D) = "" {}
    //    _BlurRadius("BlurRadius", Range(2,150)) = 5 //模糊半径  无用
    //}

    //HLSLINCLUDE

    //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    //struct appdata_img {
    //    float4 vertex:POSITION;
    //    float2 uv:TEXCOORD0;
    //};
    //struct v2f {
    //    float4 pos : POSITION;
    //    float2 uv : TEXCOORD0;
    //};

    //float4 offsets;

    //sampler2D _BlurTexture;

    //v2f vert(appdata_img v) {
    //    v2f o;
    //    VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
    //    o.pos = vertexInput.positionCS;
    //    o.uv = v.uv;
    //    return o;
    //}

    //half4 frag(v2f i) : SV_Target{
    //    //???uv
    //    half2 screenUV = (i.pos.xy / _ScreenParams.xy);
    //    half4 mainCol = tex2D(_BlurTexture, screenUV);
    //    return mainCol;
    //}

    //ENDHLSL

    //Subshader {
    //    Pass{
    //       Tags
    //       {
    //            //"LightMode" = "UniversalForward"
    //            "RenderPipeline" = "UniversalRenderPipeline"
    //       }

    //        //ZTest Always Cull Off ZWrite Off

    //        HLSLPROGRAM
    //        //#pragma fragmentoption ARB_precision_hint_fastest
    //        #pragma vertex vert
    //        #pragma fragment frag
    //        ENDHLSL
    //    }
    //}




    //Properties{
    //        _MainTex("Base (RGB)", 2D) = "" {}
    //}

    //HLSLINCLUDE

    //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    //struct appdata_img {
    //    float4 vertex:POSITION;
    //    float2 texcoord:TEXCOORD0;
    //};
    //struct v2f {
    //    float4 pos : POSITION;
    //    float2 uv : TEXCOORD0;

    //    float4 uv01 : TEXCOORD1;
    //    float4 uv23 : TEXCOORD2;
    //    float4 uv45 : TEXCOORD3;
    //};

    //float4 offsets;

    //sampler2D _MainTex;

    //v2f vert(appdata_img v) {
    //    v2f o;
    //    VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
    //    o.pos = vertexInput.positionCS;
    //    o.uv.xy = v.texcoord.xy;

    //    o.uv01 = v.texcoord.xyxy + offsets.xyxy * float4(1, 1, -1, -1);
    //    o.uv23 = v.texcoord.xyxy + offsets.xyxy * float4(1, 1, -1, -1) * 2.0;
    //    o.uv45 = v.texcoord.xyxy + offsets.xyxy * float4(1, 1, -1, -1) * 3.0;
    //    return o;
    //}

    //half4 frag(v2f i) : COLOR{
    //    half4 color = float4 (0,0,0,0);

    //    color += 0.40 * tex2D(_MainTex, i.uv);
    //    color += 0.15 * tex2D(_MainTex, i.uv01.xy);
    //    color += 0.15 * tex2D(_MainTex, i.uv01.zw);
    //    color += 0.10 * tex2D(_MainTex, i.uv23.xy);
    //    color += 0.10 * tex2D(_MainTex, i.uv23.zw);
    //    color += 0.05 * tex2D(_MainTex, i.uv45.xy);
    //    color += 0.05 * tex2D(_MainTex, i.uv45.zw);

    //    return color;
    //}

    //ENDHLSL

    //Subshader {
    //    Pass{
    //         ZTest Always Cull Off ZWrite Off

    //         HLSLPROGRAM
    //         #pragma vertex vert
    //         #pragma fragment frag
    //         ENDHLSL
    //    }
    //}
}

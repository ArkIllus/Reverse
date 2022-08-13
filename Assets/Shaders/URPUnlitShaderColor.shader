Shader "Learn/Unlit/URPUnlitShaderColor" {
    Properties{
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }

        SubShader{
            Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

            Pass {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes {
                    float4 positionOS   : POSITION;
                };

                struct Varyings {
                    float4 positionHCS  : SV_POSITION;
                };

                // 要使 Unity shader 兼容 SRP Batcher，请在名为 UnityPerMaterial 的 CBUFFER 块中声明与材质相关的所有属性。
                CBUFFER_START(UnityPerMaterial)
                    half4 _BaseColor;
                CBUFFER_END

                Varyings vert(Attributes IN) {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    return OUT;
                }

                half4 frag() : SV_Target {
                    // 返回 _BaseColor 值.
                    return _BaseColor;
                }
                ENDHLSL
            }
    }
}


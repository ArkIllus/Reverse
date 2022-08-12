Shader "Learn/ShaderAnim/OriTitle"
{
    Properties
    {
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
            "UniversalMaterialType" = "Lit"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags{ "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            float4 _MaskTex_ST;

            float _SinMax;
            float _DisFactor;
            float _TimeFactor;

            float Remap(float old1 ,float old2,float new1,float new2,float x)
            {
                return new1 + (x - old1) * (new2 - new1) / (old2 - old1);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mask = tex2D(_MaskTex, i.uv);

                float R = Remap(0.3 , 0.9, 0, 1, mask.r);
                float centerX = (1 - R) * i.uv.x + R;
                float centerY = clamp(R - 0.3, 0, 0.6);
                float2 mid = float2(centerX, centerY);

                float2 fragDir = normalize(i.uv - mid);   //Ƭ�κ�uv�������ĵķ���
                float dis = distance(mid, i.uv);          //Ƭ����uv�������ĵľ���

                i.uv += _SinMax * sin(_Time.y * _TimeFactor + dis * _DisFactor) * fragDir * mask.r * mask.r;
                        fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDHLSL
        }
    }
}
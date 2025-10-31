Shader "Hidden/MaskBrush"
{
    Properties
    {
        _DrawPosition ("Draw Position", Vector) = (-1, -1, 0, 0)
        _DrawRadius ("Draw Radius", Float) = .1
        _DrawHardness ("Draw Hardness", Float) = 0
    }

    SubShader
    {
        Lighting Off
        Blend One Zero
        
        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
            
            float4 _DrawPosition;
            float _DrawRadius;
            float _DrawHardness;
            
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 prevColor = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 drawColor = 1 - smoothstep(lerp(0, _DrawRadius, _DrawHardness), _DrawRadius, distance(IN.localTexcoord.xy, _DrawPosition));
                
                
                //return min(prevColor, drawColor);
                return saturate(prevColor + drawColor);
            }
            ENDCG
        }
    }
}

//Shader "Hidden/MaskBrush"
//{
//    Properties {}
//    SubShader
//    {
//        Tags { "RenderType"="Opaque" }
//        Pass
//        {
//            ZTest Always Cull Off ZWrite Off
//            HLSLPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include <UnityShaderUtilities.cginc>
//
//            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
//            struct v2f     { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };
//
//            sampler2D _MainTex;         // текущая маска (из Blit)
//            float4 _MainTex_TexelSize;
//            float4 _BrushUV;            // xy — центр кисти в UV
//            float4 _BrushParams;        // x=radius, y=hardness
//            float4 _BrushColor;         // rgba
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.pos = UnityObjectToClipPos(v.vertex);
//                o.uv  = v.uv;
//                return o;
//            }
//
//            float4 frag (v2f i) : SV_Target
//            {
//                float4 baseMask = tex2D(_MainTex, i.uv);
//                float2 d = i.uv - _BrushUV.xy;
//                float dist = length(d) / max(_BrushParams.x, 1e-5);
//
//                // мягкий круг: 0 вне радиуса, 1 в центре, с жёсткостью
//                float t = saturate(1.0 - dist);
//                t = pow(t, lerp(1.0, 8.0, saturate(_BrushParams.y)));
//
//                if (t > 0.5)
//                {
//                    t = 1;
//                }
//                else
//                {
//                    t = 0;
//                }
//                
//                // additive max — чтобы «закрашивать» маску, но не убирать уже белое
//                float painted = max(baseMask.r, _BrushColor.r * t);
//
//                return float4(painted, painted, painted, 1);
//            }
//            ENDHLSL
//        }
//    }
//}

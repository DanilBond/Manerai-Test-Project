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
                
                return saturate(prevColor + drawColor);
            }
            ENDCG
        }
    }
}


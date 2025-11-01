using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPainter : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material brushMaterial;
    
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    private static readonly int Mask = Shader.PropertyToID("_Mask");

    private CustomRenderTexture _renderTexture;

    void Start()
    {
        _renderTexture = new CustomRenderTexture(512, 512)
        {
            wrapMode = TextureWrapMode.Repeat,
            material = brushMaterial,
            initializationMode = CustomRenderTextureUpdateMode.OnDemand,
            initializationColor = Color.black,
            updateMode = CustomRenderTextureUpdateMode.OnDemand,
            doubleBuffered = true
        };
        
        _renderTexture.Initialize();
        skinnedMeshRenderer.material.SetTexture(Mask, _renderTexture);
    }
    
    public void PaintUV(Vector2 uv)
    {
        brushMaterial.SetVector(DrawPosition, new Vector4(uv.x, uv.y, 0, 0));
        _renderTexture.Update(2);
    }
}

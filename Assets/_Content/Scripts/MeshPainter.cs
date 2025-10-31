using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPainter : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture renderTexture;
    [SerializeField] private Material brushMaterial;
    
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    
    void Start()
    {
       renderTexture.Initialize();
    }
    
    public void PaintUV(Vector2 uv)
    {
        brushMaterial.SetVector(DrawPosition, new Vector4(uv.x, uv.y, 0, 0));
        renderTexture.Update(2);
    }
}

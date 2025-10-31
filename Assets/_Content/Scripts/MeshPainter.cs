using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPainter : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture renderTexture;
    [SerializeField] private Material brushMaterial;
    
    void Start()
    {
       renderTexture.Initialize();
    }
    
    public void PaintUV(Vector2 uv)
    {
        brushMaterial.SetVector("_DrawPosition", new Vector4(uv.x, uv.y, 0, 0));
        renderTexture.Update(2);
    }
}

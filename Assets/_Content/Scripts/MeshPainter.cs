using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPainter : MonoBehaviour
{
    public Material targetMaterial;          // Материал манекена (тот, где маска используется)
    public Texture2D baseMask;               // Базовая маска (может быть белая/чёрная)
    public Shader paintShader;               // Шейдер кисти (см. ниже)
    public string maskPropertyName = "_Mask";// Имя свойства маски в targetMaterial

    public float brushRadius = 0.02f;        // Радиус в UV (0..1)
    public float brushHardness = 0.5f;       // Жёсткость края 0..1
    public Color brushColor = Color.white;   // Что «добавляем» в маску (обычно белый)

    SkinnedMeshRenderer skinned;
    MeshCollider meshCol;
    Mesh bakedMesh;
    RenderTexture maskRT;
    Material paintMat;
    
    void Awake()
    {
        skinned = GetComponent<SkinnedMeshRenderer>();

        // Готовим RenderTexture под маску
        int w = Mathf.NextPowerOfTwo(baseMask ? baseMask.width : 1024);
        int h = Mathf.NextPowerOfTwo(baseMask ? baseMask.height : 1024);
        maskRT = new RenderTexture(w, h, 0, RenderTextureFormat.Default);
        maskRT.wrapMode = TextureWrapMode.Clamp;
        maskRT.filterMode = FilterMode.Bilinear;
        maskRT.Create();

        // Инициализируем маску базовой текстурой
        if (baseMask)
            Graphics.Blit(baseMask, maskRT);
        else
            Graphics.Blit(Texture2D.blackTexture, maskRT);

        // Подменяем маску в материале
        targetMaterial.SetTexture(maskPropertyName, maskRT);

        // Материал для рисования
        paintMat = new Material(paintShader);
    }
    
    public void PaintUV(Vector2 uv)
    {
        paintMat.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));
        paintMat.SetVector("_BrushParams", new Vector4(brushRadius, brushHardness, 0, 0));
        paintMat.SetColor("_BrushColor", brushColor);

        // Рисуем в ту же RT (через временный буфер)
        RenderTexture tmp = RenderTexture.GetTemporary(maskRT.descriptor);
        Graphics.Blit(maskRT, tmp);
        Graphics.Blit(tmp, maskRT, paintMat);
        RenderTexture.ReleaseTemporary(tmp);
    }
}

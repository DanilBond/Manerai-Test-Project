using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityTimer;
using Debug = UnityEngine.Debug;

public class DummyController : MonoBehaviour, IDamagable
{
    [Header("Prefab References")]
    public ParticleSystem bloodPrefab;
    public LayerMask mannequinLayer;
    
    //Private fields
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private MeshCollider _meshCollider;
    private ObjectPool<ParticleSystem> _bloodPool;

    private void Awake()
    {
        InitPools();
        
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _meshCollider = GetComponentInChildren<MeshCollider>();
    }

    private void InitPools()
    {
        _bloodPool = new ObjectPool<ParticleSystem>(
            //OnCreate
            () =>
            {
                var particle = Instantiate(bloodPrefab);
                particle.gameObject.SetActive(false);
                return particle;
            },
            //OnGet
            particle =>
            {
                particle.gameObject.SetActive(true);
                Timer.Register(particle.main.duration, () => { _bloodPool.Release(particle); });
            },
            //OnRelease
            particle => particle.gameObject.SetActive(false), particle => Destroy(particle.gameObject),
            10,
            25);
    }

    void FixedUpdate()
    {
        
    }

    public MeshPainter Painter;
    
    public void ProcessDamage(Vector3 hitPosition, Vector3 hitNormal)
    {
        ParticleSystem particleInstance = _bloodPool.Get();
        particleInstance.transform.position = hitPosition;
        particleInstance.transform.rotation = Quaternion.LookRotation(hitNormal);
        particleInstance.Play();

        Mesh dynamicMesh = new Mesh();
        _skinnedMeshRenderer.BakeMesh(dynamicMesh, true);
        _meshCollider.sharedMesh = dynamicMesh;
        _meshCollider.enabled = true;
        
        Vector3 origin = hitPosition + hitNormal * 0.1f;  // выносим старт из меша
        Vector3 dir = -hitNormal;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, 0.15f, mannequinLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider && hit.collider.GetComponent<MeshCollider>())
            {
                Painter.PaintUV(hit.textureCoord);
                //painter.PaintUV(hit.textureCoord);
                Debug.Log(hit.textureCoord);
            }
        }
        
        _meshCollider.enabled = false;
    }
}

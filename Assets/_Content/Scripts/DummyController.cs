using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityTimer;

public class DummyController : MonoBehaviour
{
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private MeshCollider _meshCollider;
    public Mesh mesh;

    public ParticleSystem bloodPrefab;
    private ObjectPool<ParticleSystem> _bloodPool;

    private void Awake()
    {
        InitPools();
        
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _meshCollider = GetComponentInChildren<MeshCollider>();
        
        mesh = new Mesh();
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

    public void ProceedDamage(Vector3 pos, Vector3 norm)
    {
        ParticleSystem particleInstance = _bloodPool.Get();
        particleInstance.transform.position = pos;
        particleInstance.transform.rotation = Quaternion.LookRotation(norm);
        particleInstance.Play();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityTimer;
using Debug = UnityEngine.Debug;

public class DummyController : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] private LayerMask dummyLayer;
    [SerializeField] private DummyConfig dummyConfig;
    
    [Header("References")]
    [SerializeField] private ParticleSystem bloodPrefab;
    [SerializeField] private ConfigurableJoint standJoint;
    [SerializeField] private ConfigurableJoint spineJoint;
    [SerializeField] private ConfigurableJoint headJoint;
    
    //Props
    public HealthBehaviour HealthBehaviour { get; private set; }
    
    //Private fields
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private MeshCollider _meshCollider;
    private MeshPainter _painter;
    private ObjectPool<ParticleSystem> _bloodPool;
    
    private void Awake()
    {
        InitPools();
        
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _meshCollider = GetComponentInChildren<MeshCollider>();
        _painter = GetComponentInChildren<MeshPainter>();
        HealthBehaviour = GetComponentInChildren<HealthBehaviour>();
        
        HealthBehaviour.Died += OnDied;
    }

    private void OnDestroy()
    {
        _bloodPool.Clear();
        HealthBehaviour.Died -= OnDied;
    }

    private void OnDied()
    {
        spineJoint.angularXDrive = dummyConfig.DiedSpineJointData.ToJointDrive();
        spineJoint.angularYZDrive = dummyConfig.DiedSpineJointData.ToJointDrive();
        headJoint.angularXDrive = dummyConfig.DiedHeadJointData.ToJointDrive();
        headJoint.angularYZDrive = dummyConfig.DiedHeadJointData.ToJointDrive();
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
                GameObject owner = gameObject;
                Timer.Register(particle.main.duration, () =>
                {
                    if (owner != null) 
                        _bloodPool.Release(particle);
                    else 
                        Destroy(particle.gameObject);
                });
            },
            //OnRelease
            particle => particle.gameObject.SetActive(false),
            //OnDestroy
            particle =>
            {
                if (particle) Destroy(particle.gameObject);
            },
            10,
            25);
    }
    
    public void ProcessDamage(float amount, Vector3 hitPosition, Vector3 hitNormal)
    {
        HealthBehaviour.ApplyDamage(amount);
        
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
        if (Physics.Raycast(origin, dir, out RaycastHit hit, 0.15f, dummyLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider && hit.collider.GetComponent<MeshCollider>())
            {
                _painter.PaintUV(hit.textureCoord);
            }
        }
        
        _meshCollider.enabled = false;
    }
}

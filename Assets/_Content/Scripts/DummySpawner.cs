using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [SerializeField] private DummyController dummyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float aliveDelay;
    [SerializeField] private float spawnDelay;
    
    private DummyController _currentDummy;
    private ObjectPool<DummyController> _dummyPool;

    private void Awake()
    {
        InitPools();
    }

    private void Start()
    {
        SpawnNewDummy();
    }

    private void InitPools()
    {
        _dummyPool = new ObjectPool<DummyController>(
            //OnCreate
            () =>
            {
                DummyController dummyController = Instantiate(dummyPrefab);
                dummyController.gameObject.SetActive(false);
                return dummyController;
            },
            //OnGet
            dummy => dummy.gameObject.SetActive(true),
            //OnRelease
            dummy => dummy.gameObject.SetActive(false),
            //OnDestroy
            dummy => Destroy(dummy.gameObject),
            3,
            5);
    }

    private void SpawnNewDummy()
    {
        if (_currentDummy != null) return;
        _currentDummy = _dummyPool.Get();
        _currentDummy.transform.position = spawnPoint.position;
        _currentDummy.HealthBehaviour.Died += OnDummyDead;
    }

    private void OnDummyDead() => StartCoroutine(SpawnDummyIE());

    IEnumerator SpawnDummyIE()
    {
        _currentDummy.HealthBehaviour.Died -= OnDummyDead;
        yield return new WaitForSeconds(aliveDelay);
        _dummyPool.Release(_currentDummy);
        _currentDummy = null;
        yield return new WaitForSeconds(spawnDelay);
        SpawnNewDummy();
    }
}

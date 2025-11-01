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

    private void Start()
    {
        // DummyController existingDummy = FindObjectOfType<DummyController>(false);
        // if (existingDummy)
        // {
        //     _currentDummy = existingDummy;
        //     _currentDummy.HealthBehaviour.Died += OnDummyDead;
        //     _dummyPool.Add(_currentDummy, false, false);
        //     return;
        // }
        
        SpawnNewDummy();
    }

    private void SpawnNewDummy()
    {
        if (_currentDummy != null) return;
        _currentDummy = Instantiate(dummyPrefab, spawnPoint.position, Quaternion.identity);
        _currentDummy.HealthBehaviour.Died += OnDummyDead;
    }

    private void OnDummyDead() => StartCoroutine(SpawnDummyIE());

    IEnumerator SpawnDummyIE()
    {
        yield return new WaitForSeconds(aliveDelay);
        _currentDummy.HealthBehaviour.Died -= OnDummyDead;
        Destroy(_currentDummy.gameObject);
        yield return new WaitForSeconds(spawnDelay);
        SpawnNewDummy();
    }
}

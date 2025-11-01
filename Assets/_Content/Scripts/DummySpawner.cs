using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        yield return _currentDummy.StartDissolveIE();
        Destroy(_currentDummy.gameObject);
        
        yield return new WaitForSeconds(spawnDelay);
        SpawnNewDummy();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float followSpeed;
    
    [Space]
    [SerializeField] private HealthBehaviour health;
    [SerializeField] private Slider slider;

    private Transform _camera;
    
    private void Awake()
    {
        _camera = Camera.main.transform;
        
        if (!health || !slider) return;
        health.Changed += OnChanged;
        health.Died += OnDied;
    }

    private void Start()
    {
        // Инициализация текущими значениями
        var h = health.Health;
        if (h != null) OnChanged(h.Current, h.Max);
    }

    private void OnDestroy()
    {
        if (!health) return;
        health.Changed -= OnChanged;
        health.Died -= OnDied;
    }

    private void OnChanged(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    private void OnDied()
    {
        
    }

    private void Update()
    {
        if (!followTarget) return;
        transform.position = Vector3.Lerp(
            transform.position,
            followTarget.position + followOffset,
            Time.deltaTime * followSpeed);
        
        if (!_camera) return;
        //transform.LookAt(_camera);
        Vector3 viewDir = (_camera.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(viewDir, Vector3.up) * Quaternion.Euler(0, 180, 0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField] private HealthConfig healthConfig;
    public IHealth Health { get; private set; }

    public event Action<float,float> Changed;
    public event Action Died;
    
    private void Awake()
    {
        Health = healthConfig ? new HealthModel(healthConfig) : new HealthModel(100, 100);
        Health.Changed += (c, m) => Changed?.Invoke(c, m);
        Health.Died += () => Died?.Invoke();
    }

    public void ApplyDamage(float amount, object source = null) => Health.Damage(amount, source);
    
    [ContextMenu("Damage 10")] private void DebugDamage() => ApplyDamage(10);
    [ContextMenu("Heal 10")] private void DebugHeal() => Health.Heal(10);
}

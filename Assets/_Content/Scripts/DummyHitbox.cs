using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHitbox : MonoBehaviour, IDamageable
{
    [SerializeField] private DummyController dummy;
    [SerializeField] private float damageMultiplier = 1f;

    public void ProcessDamage(float amount, Vector3 hitPosition, Vector3 hitNormal)
    {
        dummy.ProcessDamage(amount * damageMultiplier, hitPosition, hitNormal);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float baseDamage;
    [SerializeField] private AnimationCurve damageCurve;
    [SerializeField] private float punchForce;
    [SerializeField] private float minExpectedForce;
    [SerializeField] private float maxExpectedForce;
    [SerializeField] private float hitCooldownPerTarget;

    // --- runtime state ---
    private Vector3 _lastPosition;
    private float _velocity;
    private float _lastHitTime;
    
    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
       CalculateVelocity();
    }
    
    private void CalculateVelocity()
    {
        //Сами ищем скорость через дельту т.к нам не важна скорость манекена, от этого только проблемы будут
        Vector3 currentPosition = transform.position;
        Vector3 difference = _lastPosition - currentPosition;
        _velocity = difference.magnitude;
        _lastPosition = currentPosition;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        var contact = other.GetContact(0);
        Vector3 point = contact.point;
        Vector3 normal = contact.normal;
        
        float forceScalar = punchForce * _velocity;
        if (forceScalar < minExpectedForce) return;
        
        float now = Time.time;
        if (now - _lastHitTime < hitCooldownPerTarget)
            return;
        _lastHitTime = now;

        var targetRb = other.rigidbody;
        if (targetRb != null && targetRb.isKinematic == false)
        {
            Vector3 impulse = -normal * forceScalar;
            targetRb.AddForceAtPosition(impulse, point, ForceMode.Impulse);
        }
        
        if (other.gameObject.TryGetComponent(out IDamageable damagable))
        {
            float remapped = Mathf.Clamp01(math.remap(minExpectedForce, maxExpectedForce, 0.0f, 1.0f, forceScalar));
            float multiplier = damageCurve.Evaluate(remapped);
            float finalDamage = baseDamage * multiplier;

            damagable.ProcessDamage(finalDamage, point, normal);
            
            AudioManager.Instance.PlayPunchSoundAtLocation(point);
        }
    }
}

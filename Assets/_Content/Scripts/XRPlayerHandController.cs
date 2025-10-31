using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRPlayerHandController : MonoBehaviour
{
    //Public fields
    [SerializeField] private InputActionProperty gripAction;
    
    //References
    private Animator _animator;
    
    //Consts
    private static readonly int GRIP_HASH = Animator.StringToHash("Grip");
    
    //Private fields
    private Vector3 _lastPosition;
    private float _velocity;
    
    public float punchForce;
    public float minForceToDamage;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _lastPosition = transform.position;
    }

    private void Update()
    {
        CalculateVelocity();
        Animate();
    }

    private void Animate()
    {
        if (!gripAction.reference) return;
        
        _animator.SetFloat(GRIP_HASH, gripAction.action.ReadValue<float>());
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
        if (other.gameObject.TryGetComponent(out IDamageable damagable))
        {
            float finalVelocity = punchForce * _velocity;
            if (finalVelocity < minForceToDamage) return;
        
            Vector3 force = other.GetContact(0).normal.normalized * -1 * finalVelocity;
        
            other.rigidbody.AddForceAtPosition(force, other.GetContact(0).point);
            other.transform.root.GetComponent<DummyController>().ProcessDamage(10, other.GetContact(0).point, other.GetContact(0).normal);
        }
    }
}

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
    
    //Private firelds
    private Vector3 _lastPosition;
    private float _velocity;
    
    public float punchForce;
    
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
        Vector3 currentPosition = transform.position;
        Vector3 difference = _lastPosition - currentPosition;
        _velocity = difference.magnitude;
        _lastPosition = currentPosition;
    }

    private void OnCollisionEnter(Collision other)
    {
        Vector3 force = other.GetContact(0).normal.normalized * -1;
        //Сами ищем скорость через дельту т.к нам не важна скорость манекена, от этого только проблемы будут
        force *= punchForce * _velocity;
        
        other.rigidbody.AddForceAtPosition(force, other.GetContact(0).point);
        other.transform.root.GetComponent<DummyController>().ProceedDamage(other.GetContact(0).point, other.GetContact(0).normal);
    }

    [ContextMenu("GC")]
    public void GC()
    {
        System.GC.Collect();
    }
}

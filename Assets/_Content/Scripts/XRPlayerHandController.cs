using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRPlayerHandController : MonoBehaviour
{
    [SerializeField] private InputActionProperty gripAction;
    
    private static readonly int GRIP_HASH = Animator.StringToHash("Grip");
    
    private Animator _animator;

    
    public float punchForce;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat(GRIP_HASH, gripAction.action.ReadValue<float>());
    }

    private void OnCollisionEnter(Collision other)
    {
        Vector3 force = other.GetContact(0).normal.normalized * -1 * punchForce;
        other.rigidbody.AddForceAtPosition(force, other.GetContact(0).point);
    }
}

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
    
    
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (!gripAction.reference) return;
        
        _animator.SetFloat(GRIP_HASH, gripAction.action.ReadValue<float>());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRPlayerHandController : MonoBehaviour
{
    //Public fields
    [SerializeField] private InputActionProperty weaponSwitchAction;
    [SerializeField] private DamageDealer[] weapons;
    
    //Private fields
    private int _weaponIndex;

    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        if (weaponSwitchAction.action.triggered)
        {
            _weaponIndex++;
            if (_weaponIndex >= weapons.Length) _weaponIndex = 0;
            
            SelectWeapon();
        }
    }

    private void SelectWeapon()
    {
        for (int i = 0; i < weapons.Length; i++) weapons[i].gameObject.SetActive(_weaponIndex == i);
    }
}

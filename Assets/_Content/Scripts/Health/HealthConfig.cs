using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "Data/Health Config")]
public class HealthConfig : ScriptableObject
{
    [SerializeField] private float _healthInitial;
    [SerializeField] private float _healthMax;
    [SerializeField] private bool _allowOverheal;
    
    public float HealthInitial => _healthInitial;
    public float HealthMax => _healthMax;
    public bool AllowOverheal => _allowOverheal;
}

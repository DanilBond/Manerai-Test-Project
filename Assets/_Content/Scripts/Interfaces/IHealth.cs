using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float Current { get; }
    float Max { get; }
    event Action<float,float> Changed; // (current, max)
    event Action Died;

    void Damage(float amount, object source = null);
    void Heal(float amount);
    void SetMax(float newMax, bool keepRatio = true);
}
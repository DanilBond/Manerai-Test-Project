using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HealthModel : IHealth
{
    public float Current { get; private set; }
    public float Max { get; private set; }

    public event Action<float,float> Changed;
    public event Action Died;

    private readonly bool _allowOverheal;
    private bool _dead;

    public HealthModel(float max, float start, bool allowOverheal=false)
    {
        if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
        Max = max;
        _allowOverheal = allowOverheal;
        Current = (start < 0) ? Max : Math.Clamp(start, 0, Max);
        OnChanged();
        if (Current <= 0) MarkDead();
    }
    
    public HealthModel(HealthConfig config)
    {
        if (config.HealthMax <= 0) throw new ArgumentOutOfRangeException(nameof(config.HealthMax));
        Max = config.HealthMax;
        _allowOverheal = config.AllowOverheal;
        Current = (config.HealthInitial < 0) ? Max : Math.Clamp(config.HealthInitial, 0, Max);
        OnChanged();
        if (Current <= 0) MarkDead();
    }

    public void Damage(float amount, object source = null)
    {
        if (_dead || amount <= 0) return;
        Current = Math.Max(0, Current - amount);
        OnChanged();
        if (Current == 0) MarkDead();
    }

    public void Heal(float amount)
    {
        if (_dead || amount <= 0) return;
        var cap = _allowOverheal ? float.MaxValue : Max;
        Current = Math.Min(cap, Current + amount);
        OnChanged();
    }

    public void SetMax(float newMax, bool keepRatio = true)
    {
        if (newMax <= 0) throw new ArgumentOutOfRangeException(nameof(newMax));
        float ratio = keepRatio && Max > 0 ? (float)Current / Max : 1f;
        Max = newMax;
        Current = Math.Clamp((float)Math.Round(Max * ratio), 0, Max);
        OnChanged();
        if (Current == 0) MarkDead();
    }

    private void MarkDead()
    {
        if (_dead) return;
        _dead = true;
        Died?.Invoke();
    }

    private void OnChanged() => Changed?.Invoke(Current, Max);
}
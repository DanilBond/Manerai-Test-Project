using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ProcessDamage(float amount, Vector3 hitPosition, Vector3 hitNormal);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void ProcessDamage(Vector3 hitPosition, Vector3 hitNormal);
}

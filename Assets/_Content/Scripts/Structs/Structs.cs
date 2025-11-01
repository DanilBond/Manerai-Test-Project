using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableJointDrive 
{
    public float positionSpring;
    public float positionDamper;
    public float maximumForce;

    public JointDrive ToJointDrive()
    {
        return new JointDrive
        {
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            maximumForce = maximumForce
        };
    }
}
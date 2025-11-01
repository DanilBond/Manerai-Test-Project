using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DummyConfig", menuName = "Data/Dummy Config")]
public class DummyConfig : ScriptableObject
{
    [SerializeField] private SerializableJointDrive diedStandJointData;
    [SerializeField] private SerializableJointDrive diedSpineJointData;
    [SerializeField] private SerializableJointDrive diedHeadJointData;
    ///////////////////////////
    public SerializableJointDrive DiedStandJointData => diedStandJointData;
    public SerializableJointDrive DiedSpineJointData => diedSpineJointData;
    public SerializableJointDrive DiedHeadJointData => diedHeadJointData;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DummyConfig", menuName = "Data/Dummy Config")]
public class DummyConfig : ScriptableObject
{
    [SerializeField] private SerializableJointDrive defaultStandJointData;
    [SerializeField] private SerializableJointDrive defaultSpineJointData;
    [SerializeField] private SerializableJointDrive defaultHeadJointData;
    
    [SerializeField] private SerializableJointDrive diedStandJointData;
    [SerializeField] private SerializableJointDrive diedSpineJointData;
    [SerializeField] private SerializableJointDrive diedHeadJointData;

    ///////////////////////////

    public SerializableJointDrive DefaultStandJointData => defaultStandJointData;
    public SerializableJointDrive DefaultSpineJointData => defaultSpineJointData;
    public SerializableJointDrive DefaultHeadJointData => defaultHeadJointData;
    
    public SerializableJointDrive DiedStandJointData => defaultStandJointData;
    public SerializableJointDrive DiedSpineJointData => defaultSpineJointData;
    public SerializableJointDrive DiedHeadJointData => defaultHeadJointData;
}

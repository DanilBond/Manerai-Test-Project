using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "DummyConfig", menuName = "Data/Dummy Config")]
public class DummyConfig : ScriptableObject
{
    [SerializeField] private float dissolveDuration;
    [SerializeField] private Ease dissolveEase;
    [SerializeField] private SerializableJointDrive diedStandJointData;
    [SerializeField] private SerializableJointDrive diedSpineJointData;
    [SerializeField] private SerializableJointDrive diedHeadJointData;
    ///////////////////////////
    public float DissolveDuration => dissolveDuration;
    public Ease DissolveEase => dissolveEase;
    public SerializableJointDrive DiedStandJointData => diedStandJointData;
    public SerializableJointDrive DiedSpineJointData => diedSpineJointData;
    public SerializableJointDrive DiedHeadJointData => diedHeadJointData;
}

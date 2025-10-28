using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private void Awake()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //_skinnedMeshRenderer.BakeMesh();
    }
}

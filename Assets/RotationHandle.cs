using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHandle : Handle
{
    protected override void DoTransform(float magnitude)
    {
        transform.parent.transform.RotateAround(transform.parent.position, transform.up, magnitude);
    }

    void Update() {
        MaintainScale();
    }

    private void MaintainScale() {
        // Assumes ring scale has not been altered, so ring minor radius = 1.0f
        transform.localScale = GetMaxMeshDimension() * Vector3.one;
    }
}

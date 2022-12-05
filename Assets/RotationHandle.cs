using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* <summary>
* UI Handle that rotates and object about the y axis.
* Note: User interaction still happens along the z axis like all other <c>Handle</c>s
*       Since this causes the z axis to rotate, the appropriate user action
*       is to trace a circle around the y axis
* </summary>
*/
public class RotationHandle : Handle
{
    protected override void DoTransform(float magnitude)
    {
        transform.parent.transform.RotateAround(transform.parent.position, transform.up, magnitude);
    }

    void Update()
    {
        MaintainScale();
    }

    /**
    * <summary>
    * Modifies scale to ensure this element always surrounds the subject
    * </summary>
    */
    private void MaintainScale()
    {
        // Assumes ring scale has not been altered, so ring minor radius = 1.0f
        transform.localScale = GetMaxMeshDimension() * Vector3.one;
    }
}

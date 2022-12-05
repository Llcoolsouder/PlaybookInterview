using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* <summary>
* UI Handle that changes the scale of an object along it's Z axis
* </summary>
*/
public class ScaleHandle : Handle
{
    void Update()
    {
        MaintainRelativeDistance(1.25f);
    }

    protected override void DoTransform(float magnitude)
    {
        Vector3 deltaScale = mLocalMainAxis * magnitude * GetPixelSize() * 0.5f;
        mSubject.transform.localScale = Vector3.Max(Vector3.zero, mSubject.transform.localScale + deltaScale);
    }
}

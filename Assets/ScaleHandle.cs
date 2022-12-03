using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleHandle : Handle
{
    void Update()
    {
        MaintainRelativeDistance(1.25f);
    }

    protected override void DoTransform(float magnitude)
    {
        mSubject.transform.localScale +=
            mLocalMainAxis * magnitude * GetPixelSize() * 0.5f;
    }
}

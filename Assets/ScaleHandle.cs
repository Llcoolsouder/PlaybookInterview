using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleHandle : Handle
{
    private Vector3 mScaleAxis;

    protected override void Start()
    {
        base.Start();
        float dotX = Mathf.Abs(Vector3.Dot(transform.forward, transform.parent.right));
        float dotY = Mathf.Abs(Vector3.Dot(transform.forward, transform.parent.up));
        float dotZ = Mathf.Abs(Vector3.Dot(transform.forward, transform.parent.forward));
        if (dotX >= dotY && dotX >= dotZ)
        {
            mScaleAxis = Vector3.right;
        }
        else if (dotY >= dotZ)
        {
            mScaleAxis = Vector3.up;
        }
        else
        {
            mScaleAxis = Vector3.forward;
        }
    }

    void Update()
    {
        transform.position = mSubject.transform.position +
            transform.forward *
            (Vector3.Dot(mScaleAxis, mSubject.transform.localScale) * 0.5f + 0.75f);
    }

    protected override void DoTransform(float magnitude)
    {
        mSubject.transform.localScale +=
            mScaleAxis * magnitude * GetPixelSize() * 0.5f;
    }
}

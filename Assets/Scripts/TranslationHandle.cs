using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* <summary>
* UI Handle that translates an object along it's Z axis
* </summary>
*/
public class TranslationHandle : Handle
{
    void Update()
    {
        MaintainRelativeDistance(1.5f);
    }

    protected override void DoTransform(float magnitude)
    {
        transform.parent.transform.Translate(
            transform.forward * magnitude * GetPixelSize(),
            Space.World);
    }
}

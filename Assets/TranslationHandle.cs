using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationHandle : Handle
{
    protected override void DoTransform(float magnitude)
    {
        transform.parent.Translate(
            transform.forward * magnitude * GetPixelSize(),
            Space.World);
    }

    /// <returns>Size of pixel at parent's distance from camera</return>
    float GetPixelSize()
    {
        float distance = (Camera.main.transform.position - transform.position).magnitude;
        float frustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight / Camera.main.pixelHeight;
    }
}

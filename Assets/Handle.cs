using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public abstract class Handle : MonoBehaviour
{
    protected Vector3 mMousePreviousPosition;

    /**
    * <summary>Override with behaviour for handle during interaction</summary
    * <param name="magnitude">Magnitude of transformation to be applied</param>
    */
    protected abstract void DoTransform(float magnitude);

    /**
    * <summary>Sets the color of the handle material</summary>
    * <param name="color">New color used to render handle</param>
    */
    public void SetColor(Color color)
    {
        Material material = gameObject.GetComponent<Renderer>().material;
        material.color = color;
    }

    void OnMouseDown()
    {
        mMousePreviousPosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 mouseCurrentPosition = Input.mousePosition;
        Vector3 screenSpaceLocalForward = (
            Camera.main.WorldToScreenPoint(transform.position + transform.forward) -
            Camera.main.WorldToScreenPoint(transform.position)).normalized;
        Vector3 mouseMove = mouseCurrentPosition - mMousePreviousPosition;
        float mouseChangeMagnitude = Vector3.Dot(
            screenSpaceLocalForward,
            mouseMove.normalized) * mouseMove.magnitude;
        DoTransform(mouseChangeMagnitude);
        mMousePreviousPosition = mouseCurrentPosition;
    }
}

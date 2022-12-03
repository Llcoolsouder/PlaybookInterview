using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public abstract class Handle : MonoBehaviour
{
    protected GameObject mSubject;
    protected Vector3 mLocalMainAxis;
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

    /// <returns>Size of pixel at parent's distance from camera</return>
    protected float GetPixelSize()
    {
        float distance = (Camera.main.transform.position - mSubject.transform.position).magnitude;
        float frustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight / Camera.main.pixelHeight;
    }

    /**
    * <summary>
    * Keeps handle a specified distance away from the subject.
    * Distance will be scaled by the greatest dimension of the subject
    * <param name="distance">Distance at max dimension equal to 1</param>
    * </summary>
    */
    protected void MaintainRelativeDistance(float distance)
    {
        transform.position = mSubject.transform.position +
            (transform.forward * (distance * GetMaxMeshDimension()));
    }

    protected Vector3 GetMeshDimensions() {
        return Vector3.Scale(
            mSubject.GetComponent<MeshFilter>().mesh.bounds.size,
            mSubject.transform.localScale);
    }

    protected float GetMaxMeshDimension() {
        Vector3 meshDimensions = GetMeshDimensions();
        return Mathf.Max(meshDimensions.x, meshDimensions.y, meshDimensions.z);
    }

    protected virtual void Start()
    {
        foreach (Transform child in transform.parent)
        {
            if (child.GetComponent<Handle>() == null)
            {
                mSubject = child.gameObject;
                break;
            }
        }
        if (mSubject == null) {
            Debug.LogWarning("Handle objects should have a single non-Handle sibling");
        }

        float dotX = Mathf.Abs(Vector3.Dot(transform.forward, mSubject.transform.right));
        float dotY = Mathf.Abs(Vector3.Dot(transform.forward, mSubject.transform.up));
        float dotZ = Mathf.Abs(Vector3.Dot(transform.forward, mSubject.transform.forward));
        if (dotX >= dotY && dotX >= dotZ)
        {
            mLocalMainAxis = Vector3.right;
        }
        else if (dotY >= dotZ)
        {
            mLocalMainAxis = Vector3.up;
        }
        else
        {
            mLocalMainAxis = Vector3.forward;
        }
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

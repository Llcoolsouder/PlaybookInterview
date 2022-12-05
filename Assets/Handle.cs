using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* <summary>
* Handles all of the behaviour necessary for a single interactive 3D handle.
* Click and drag behaviour is implemented to determine *how much* an object should be transformed.
* The <c>DoTransform</c> function should be overridden to specify *how* an object should be transformed.
* </summary>
*/
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public abstract class Handle : MonoBehaviour
{
    /// <value>The object to be manipulated by this handle</value>
    protected GameObject mSubject;

    /// <value>The axis in <c>mSubject</c>'s local space that aligns with the forward axis of this handle</value>
    protected Vector3 mLocalMainAxis;

    ///<value>The location of the mouse in the previous frame</value>
    protected Vector3 mMousePreviousPosition;

    /**
    * <summary>Sets the color of the handle material</summary>
    * <param name="color">New color used to render handle</param>
    */
    public void SetColor(Color color)
    {
        Material material = gameObject.GetComponent<Renderer>().material;
        material.color = color;
    }

    /**
    * <summary>Override with behaviour for handle during interaction</summary>
    * <param name="magnitude">Magnitude of transformation to be applied</param>
    */
    protected abstract void DoTransform(float magnitude);

    /// <returns>Size of pixel at subjects's distance from camera</return>
    protected float GetPixelSize()
    {
        float distance = (Camera.main.transform.position - mSubject.transform.position).magnitude;
        float frustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight / Camera.main.pixelHeight;
    }

    /**
    * <summary>
    * Keeps handle a specified distance away from the subject.
    * Distance will be scaled by the greatest dimension of the subject.
    * </summary>
    * <param name="distance">Distance where max dimension equal to 1</param>
    */
    protected void MaintainRelativeDistance(float distance)
    {
        transform.position = mSubject.transform.position +
            (transform.forward * (distance * GetMaxMeshDimension()));
    }

    /// <returns>Mesh dimensions in meters</returns>
    protected Vector3 GetMeshDimensions()
    {
        return Vector3.Scale(
            mSubject.GetComponent<MeshFilter>().mesh.bounds.size,
            mSubject.transform.localScale);
    }

    ///<returns>Length of greatest mesh dimension in meters</returns>
    protected float GetMaxMeshDimension()
    {
        Vector3 meshDimensions = GetMeshDimensions();
        return Mathf.Max(meshDimensions.x, meshDimensions.y, meshDimensions.z);
    }

    protected virtual void Start()
    {
        mSubject = FindSubject();
        mLocalMainAxis = DetermineLocalMainAxis();
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

    /**
    * <summary>
    * Locates the subject that this handle belongs to by searching for 
    * non-Handle siblings
    * </summary>
    * <returns>The subject gameobject if found, else null</returns>
    */
    private GameObject FindSubject()
    {
        foreach (Transform child in transform.parent)
        {
            if (child.GetComponent<Handle>() == null)
            {
                return child.gameObject;
            }
        }
        Debug.LogWarning("Handle objects should have a single non-Handle sibling");
        // If we need to allow additional siblings,
        // remove this warning, and set the subject via a tag or SerializedField
        return null;
    }

    /**
    * <summary>
    * Determines which of the subject's axes align with the forward axis of this handle.
    * Assumes, lies on one of the subject's axes.
    * </summary>
    * <returns>Unit vector denoting the main axis in the subject's local space</returns>
    */
    private Vector3 DetermineLocalMainAxis()
    {
        float dotX = Mathf.Abs(Vector3.Dot(transform.forward, mSubject.transform.right));
        float dotY = Mathf.Abs(Vector3.Dot(transform.forward, mSubject.transform.up));
        float dotZ = Mathf.Abs(Vector3.Dot(transform.forward, mSubject.transform.forward));
        if (dotX >= dotY && dotX >= dotZ)
        {
            return Vector3.right;
        }
        else if (dotY >= dotZ)
        {
            return Vector3.up;
        }
        else
        {
            return Vector3.forward;
        }
    }
}

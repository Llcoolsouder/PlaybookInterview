using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* <summary>
* This component spawns UI elements that allow translation, rotation, 
* and scaling on the object's local axes
* </summary>
*/
public class Interactive : MonoBehaviour
{
    [SerializeField] private Handle mTranslationHandle;
    [SerializeField] private Handle mRotationHandle;
    [SerializeField] private Handle mScaleHandle;

    void Start()
    {
        Reparent();

        var parameters = new List<HandleParams> {
            new HandleParams(Color.red, transform.right, transform.up),
            new HandleParams(Color.green, transform.up, - transform.right),
            new HandleParams(Color.blue, transform.forward, transform.up)
        };
        foreach (var parameter in parameters)
        {
            // Note, rotation handles have *up* axis aligned with their axis of rotation,
            // but are still interacted with along their forward axis
            Quaternion angularHandleRotation = Quaternion.LookRotation(parameter.up, parameter.forward);
            GameObject rotationHandle = Object.Instantiate(
                mRotationHandle.gameObject,
                transform.position,
                angularHandleRotation,
                transform.parent);
            rotationHandle.GetComponent<Handle>().SetColor(parameter.color);

            Quaternion linearHandleRotation = Quaternion.LookRotation(parameter.forward, parameter.up);
            GameObject translationHandle = Object.Instantiate(
                mTranslationHandle.gameObject,
                transform.position + parameter.forward,
                linearHandleRotation,
                transform.parent);
            translationHandle.GetComponent<Handle>().SetColor(parameter.color);

            GameObject scaleHandle = Object.Instantiate(
                mScaleHandle.gameObject,
                transform.position + (parameter.forward * 0.75f),
                linearHandleRotation,
                transform.parent);
            scaleHandle.GetComponent<Handle>().SetColor(parameter.color);
        }
    }

    /**
    * <summary>
    * Creates a new parent at the same position as this object,
    * and sets this object as a child of the new parent.
    * If this object already had a parent, it will become a grandparent. CONGRATS!
    * </summary>
    */
    private void Reparent()
    {
        Transform oldParent = transform.parent;
        GameObject newParent = new GameObject(string.Format("Interactive{0}", transform.name));
        transform.parent = newParent.transform;
        newParent.transform.position = transform.position;
        newParent.transform.rotation = transform.rotation;
        newParent.transform.parent = oldParent;
    }

    /// <summary>Specifies color and orientation for handle instantiation</summary>
    private struct HandleParams
    {
        public HandleParams(Color color, Vector3 forward, Vector3 up)
        {
            this.color = color;
            this.forward = forward;
            this.up = up;
        }

        public readonly Color color { get; }
        public readonly Vector3 forward { get; }
        public readonly Vector3 up { get; }
    }
}

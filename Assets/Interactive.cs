using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* <summary>
* Set this as the *parent* of an object to spawn UI elements that allow
* translation, rotation, and scaling on the object's local axes
* </summary>
*/
public class Interactive : MonoBehaviour
{
    [SerializeField] private Handle mTranslationHandle;
    [SerializeField] private Handle mRotationHandle;
    [SerializeField] private Handle mScaleHandle;

    void Start()
    {
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
                transform);
            rotationHandle.GetComponent<Handle>().SetColor(parameter.color);

            Quaternion linearHandleRotation = Quaternion.LookRotation(parameter.forward, parameter.up);
            GameObject translationHandle = Object.Instantiate(
                mTranslationHandle.gameObject,
                transform.position + parameter.forward,
                linearHandleRotation,
                transform);
            translationHandle.GetComponent<Handle>().SetColor(parameter.color);

            GameObject scaleHandle = Object.Instantiate(
                mScaleHandle.gameObject,
                transform.position + (parameter.forward * 0.75f),
                linearHandleRotation,
                transform);
            scaleHandle.GetComponent<Handle>().SetColor(parameter.color);
        }
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

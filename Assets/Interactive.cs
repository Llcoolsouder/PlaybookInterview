using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] private Handle mTranslationHandle;
    [SerializeField] private Handle mRotationHandle;
    [SerializeField] private Handle mScaleHandle;

    // Start is called before the first frame update
    void Start()
    {
        var parameters = new List<HandleParams> {
            new HandleParams(Color.red, transform.right, transform.up),
            new HandleParams(Color.green, transform.up, - transform.right),
            new HandleParams(Color.blue, transform.forward, transform.up)
        };
        foreach (var parameter in parameters)
        {
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

    private struct HandleParams
    {

        public HandleParams(Color color, Vector3 forward, Vector3 up)
        {
            this.color = color;
            this.forward = forward;
            this.up = up;
        }

        public Color color { get; }
        public Vector3 forward { get; }
        public Vector3 up { get; }
    }
}

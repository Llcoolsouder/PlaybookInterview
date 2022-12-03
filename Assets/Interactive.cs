using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] private Handle mTranslationHandle;
    [SerializeField] private Handle mRotationHandle;

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
            Quaternion rotation = Quaternion.LookRotation(parameter.forward, parameter.up);
            GameObject handle = Object.Instantiate(
                mTranslationHandle.gameObject,
                transform.position + parameter.forward,
                rotation,
                transform);
            handle.GetComponent<Handle>().SetColor(parameter.color);
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
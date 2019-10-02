using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    [SerializeField] Transform camTransform = null;

    Quaternion originalRotation = Quaternion.identity;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        transform.rotation = camTransform.rotation * originalRotation;
    }
}
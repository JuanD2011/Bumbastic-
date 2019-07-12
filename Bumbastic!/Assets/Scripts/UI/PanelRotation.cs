using UnityEngine;

public class PanelRotation : MonoBehaviour
{
    [SerializeField] Vector3 vector3 = Vector3.forward;
    [SerializeField] private float speed = 1f;

    void Update()
    {
        transform.eulerAngles += vector3 * speed * Time.deltaTime;
    }
}

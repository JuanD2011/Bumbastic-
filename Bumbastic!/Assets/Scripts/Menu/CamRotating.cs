using UnityEngine;

public class CamRotating : MonoBehaviour
{
    [SerializeField] Transform axis;
    [SerializeField] float vel;

    [SerializeField] GameObject principalCamera;
    float elapsedTime = 0f;
    private void Update()
    {
        if (principalCamera.activeInHierarchy)
        {
            elapsedTime += Time.deltaTime;
            axis.eulerAngles = new Vector3(0, elapsedTime * vel, 0);
        }
    }
}

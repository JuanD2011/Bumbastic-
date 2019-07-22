using UnityEngine;

public class MenuCamManager : MonoBehaviour
{
    [SerializeField] Transform axis = null;
    [SerializeField] GameObject principalCamera = null;
    [SerializeField] GameObject matchmakingCamera = null;

    float elapsedTime = 0f;

    [SerializeField] float angularVel = 0f;
    [SerializeField] float amplitude = 0f;

    private void Start()
    {
        MenuCanvas.onMatchmaking += SetMatchmakingCamera;
    }

    private void SetMatchmakingCamera(bool _bool)
    {
        matchmakingCamera.SetActive(_bool);
    }

    private void Update()
    {
        if (principalCamera.activeInHierarchy && !matchmakingCamera.activeInHierarchy)
        {
            elapsedTime += Time.deltaTime;
            axis.eulerAngles = new Vector3(0, amplitude * Mathf.Sin(angularVel * elapsedTime), 0);
        }
    }
}

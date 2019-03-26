using UnityEngine;

public class MenuCamManager : MonoBehaviour
{
    [SerializeField] Transform axis;
    [SerializeField] GameObject principalCamera;
    [SerializeField] GameObject matchmakingCamera;
    [SerializeField] MenuUI menuUI;

    float elapsedTime = 0f;

    [SerializeField] float angularVel;
    [SerializeField] float amplitude;

    private void Start()
    {
        menuUI.OnMatchmaking += SetMatchmakingCamera;
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

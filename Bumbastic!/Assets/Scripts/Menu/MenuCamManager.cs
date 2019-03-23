using UnityEngine;

public class MenuCamManager : MonoBehaviour
{
    [SerializeField] float vel;

    [SerializeField] Transform axis;
    [SerializeField] GameObject principalCamera;
    [SerializeField] GameObject matchmakingCamera;
    [SerializeField] MenuUI menuUI;

    float elapsedTime = 0f;

    private void Start()
    {
        menuUI.OnSetMatchmakingCamera += SetMatchmakingCamera;
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
            axis.eulerAngles = new Vector3(0, elapsedTime * vel, 0);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCamManager : MonoBehaviour
{
    [SerializeField] Transform axis;
    [SerializeField] GameObject principalCamera;
    [SerializeField] GameObject matchmakingCamera;

    float elapsedTime = 0f;

    [SerializeField] float angularVel;
    [SerializeField] float amplitude;

    private void Start()
    {
        MenuUI.OnMatchmaking += SetMatchmakingCamera;
    }

    private void SetMatchmakingCamera(bool _bool)
    {
        matchmakingCamera.SetActive(_bool);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }

        if (principalCamera.activeInHierarchy && !matchmakingCamera.activeInHierarchy)
        {
            elapsedTime += Time.deltaTime;
            axis.eulerAngles = new Vector3(0, amplitude * Mathf.Sin(angularVel * elapsedTime), 0);
        }
    }
}

using System;
using UnityEngine;

public class MenuCamManager : MonoBehaviour
{
    [SerializeField] float vel;

    [SerializeField] Transform axis;
    [SerializeField] GameObject principalCamera;
    [SerializeField] GameObject matchmakingCamera;

    float elapsedTime = 0f;

    private void Start()
    {
        MenuUI.OnSetMatchmakingCamera += SetMatchmakingCamera;
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
            Debug.Log(elapsedTime);
            axis.eulerAngles = new Vector3(0, elapsedTime * vel, 0);
        }
    }
}

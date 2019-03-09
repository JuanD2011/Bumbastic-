using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CanvasBillboard : MonoBehaviour
{
    TextMeshProUGUI nicknameText;
    byte playersSpawned = 0;

    public TextMeshProUGUI NicknameText { private get => nicknameText; set => nicknameText = value; }

    private void Start()
    {
        NicknameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void SetPlayersNickname()
    {
        
    }

    private void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                       Camera.main.transform.rotation * Vector3.up);
        else Debug.LogWarning("Set to the camera the MainCamera tag");
    }
}

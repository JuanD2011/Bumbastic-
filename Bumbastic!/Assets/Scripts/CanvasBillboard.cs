using UnityEngine;
using TMPro;

public class CanvasBillboard : MonoBehaviour
{
    TextMeshProUGUI nicknameText;
    Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        nicknameText = GetComponentInChildren<TextMeshProUGUI>();
        nicknameText.text = string.Format("P{0}", player.Id + 1);
    }

    private void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                       Camera.main.transform.rotation * Vector3.up);
        else Debug.LogWarning("Set to the camera the MainCamera tag");
    }
}

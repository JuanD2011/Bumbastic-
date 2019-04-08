using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasBillboard : MonoBehaviour
{
    [SerializeField] Settings settings;

    Player player;

    TextMeshProUGUI[] playersText;
    Image playerColor;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        playersText = GetComponentsInChildren<TextMeshProUGUI>();
        playerColor = GetComponentInChildren<Image>();

        playersText[0].text = string.Format("P{0}", player.Id + 1);
        playerColor.color = settings.playersColor[player.Id];

        playersText[1].text = string.Format("{0}", player.PrefabName);
    }

    private void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                       Camera.main.transform.rotation * Vector3.up);
        else Debug.LogWarning("Set to the camera the MainCamera tag");
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasBillboard : MonoBehaviour
{
    [SerializeField] Settings settings = null;

    Player player;

    TextMeshProUGUI[] playersText;
    Image playerColor;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        playersText = GetComponentsInChildren<TextMeshProUGUI>();
        playerColor = GetComponentInChildren<Image>();

        switch (Translation.GetCurrentLanguage())
        {
            case Languages.en:
                playersText[0].text = string.Format("P{0}", player.Id + 1);
                break;
            case Languages.es:
                playersText[0].text = string.Format("J{0}", player.Id + 1);
                break;
            case Languages.unknown:
                playersText[0].text = string.Format("P{0}", player.Id + 1);
                break;
            default:
                break;
        }
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

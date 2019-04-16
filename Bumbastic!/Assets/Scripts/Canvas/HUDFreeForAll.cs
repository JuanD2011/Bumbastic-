using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDFreeForAll : MonoBehaviour
{
    [SerializeField] Settings settings;
    [SerializeField] Image[] skinSprites;
    [SerializeField] Image[] playersImageColor;
    [SerializeField] TextMeshProUGUI[] playerKills;

    private void Start()
    {
        SetPlayerFeatures();
    }

    private void SetPlayerFeatures()
    {
        for (int i = 0; i < GameManager.manager.Players.Count; i++)
        {
            playersImageColor[i].color = settings.playersColor[i];
            skinSprites[i].sprite = GameManager.manager.Players[i].SkinSprite;
        }
    }
}

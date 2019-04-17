using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDFreeForAll : MonoBehaviour
{
    [SerializeField] Image[] skinSprites;
    [SerializeField] TextMeshProUGUI[] playerKills;
    [SerializeField] Image[] playerColors;

    private void Start()
    {
        SetPlayerFeatures();
    }

    private void SetPlayerFeatures()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            skinSprites[i].sprite = InGame.playerSettings[i].skinSprite;
            playerColors[i].color = InGame.playerSettings[i].color;
            playerKills[i].text = 0.ToString();
        }
    }

    private void UpdateKills(byte _playerID)
    {
        //playerKills[_playerID].text = string.Format("{0}", GameManager.Manager.Players[_playerID].Kills);
    }
}

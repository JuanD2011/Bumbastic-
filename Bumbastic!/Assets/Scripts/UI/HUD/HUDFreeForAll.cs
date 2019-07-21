using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDFreeForAll : MonoBehaviour
{
    [SerializeField] GameObject[] playerHUD = new GameObject[0];

    protected Image[] skinSprites;
    protected Image[] playerColors;

    protected TextMeshProUGUI[] points;

    private void Start()
    {
        Initialize();
        SetFeatures();
        FreeForAllManager.FreeForAll.OnPlayerKilled += UpdateScore;
    }

    protected void Initialize()
    {
        skinSprites = new Image[playerHUD.Length];
        points = new TextMeshProUGUI[playerHUD.Length];
        playerColors = new Image[playerHUD.Length];

        for (int i = 0; i < playerHUD.Length; i++)
        {
            skinSprites[i] = playerHUD[i].GetComponent<Image>();
            points[i] = playerHUD[i].GetComponentInChildren<TextMeshProUGUI>();
            playerColors[i] = playerHUD[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        }
    }

    protected virtual void SetFeatures()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            skinSprites[i].gameObject.SetActive(true);
            skinSprites[i].sprite = InGame.playerSettings[i].skinSprite;
            playerColors[i].color = InGame.playerSettings[i].color;
            points[i].text = FreeForAllManager.FreeForAll.LifePoints[i].ToString();
        }
    }

    protected virtual void UpdateScore(byte _playerID)
    {
        points[_playerID].text = string.Format("{0}", FreeForAllManager.FreeForAll.LifePoints[_playerID]);
    }
}

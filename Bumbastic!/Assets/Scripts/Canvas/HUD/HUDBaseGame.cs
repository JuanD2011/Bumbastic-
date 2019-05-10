public class HUDBaseGame : HUDFreeForAll
{
    private void Start()
    {
        Initialize();
        SetFeatures();

        Base.OnBaseDamage += UpdateScore;
    }

    protected override void SetFeatures()
    {
        for (int i = 0; i < InputManager.playerSettings.Count; i++)
        {
            skinSprites[i].gameObject.SetActive(true);
            skinSprites[i].sprite = InputManager.playerSettings[i].skinSprite;
            playerColors[i].color = InputManager.playerSettings[i].color;
            points[i].text = BasesGameManager.basesGame.bases[i].LifePoints.ToString();
        }
    }

    private void UpdateScore(byte _baseID, byte _lifePoints)
    {
        points[_baseID].text = _lifePoints.ToString();
    }
}
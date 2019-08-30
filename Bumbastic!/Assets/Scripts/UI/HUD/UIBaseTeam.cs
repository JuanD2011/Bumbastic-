using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBaseTeam : MonoBehaviour
{
    [SerializeField] Image[] playerImages = new Image[2];
    [SerializeField] TextMeshProUGUI m_basesPoints = null;
    [SerializeField] Image teamColor = null;
    [SerializeField] Base m_Team = null;

    private void Start()
    {
        m_basesPoints.text = m_Team.LifePoints.ToString();
        teamColor.color = m_Team.TeamColor;

        for (int i = 0; i < m_Team.Members.Count; i++)
        {
            playerImages[i].sprite = InGame.playerSettings[m_Team.Members[i].Id].skinSprite;
            playerImages[i].color = Color.white;
        }

        Base.OnBaseDamage += UpdateScore;
    }

    private void UpdateScore(byte _baseID, byte _lifePoints)
    {
        if (_baseID != m_Team.Id) return;

        m_basesPoints.text = _lifePoints.ToString();
    }
}
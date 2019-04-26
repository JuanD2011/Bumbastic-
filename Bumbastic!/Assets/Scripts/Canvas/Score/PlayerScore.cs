using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    TextMeshProUGUI m_name;
    Image[] m_Stars;
    Image playerSkinSprite;

    public Image[] Stars { get => m_Stars; private set => m_Stars = value; }
    public TextMeshProUGUI Name { get => m_name; private set => m_name = value; }
    public Image PlayerSkinSprite { get => playerSkinSprite; private set => playerSkinSprite = value; }

    public void InitComponents()
    {
        PlayerSkinSprite = GetComponent<Image>();
        Name = GetComponentInChildren<TextMeshProUGUI>();
        Stars = Name.GetComponentsInChildren<Image>();
    }
}

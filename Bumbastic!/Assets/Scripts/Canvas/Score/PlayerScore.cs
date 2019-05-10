using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] GameObject starsContainer;
    Image[] m_Stars;
    Image playerSkinSprite;

    public Image[] Stars { get => m_Stars; private set => m_Stars = value; }
    public Image PlayerSkinSprite { get => playerSkinSprite; private set => playerSkinSprite = value; }

    public void InitComponents()
    {
        PlayerSkinSprite = GetComponent<Image>();
        Stars = starsContainer.GetComponentsInChildren<Image>();
    }
}

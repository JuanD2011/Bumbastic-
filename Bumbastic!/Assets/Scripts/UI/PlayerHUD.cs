using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] byte iD = 0;
    [SerializeField] Image m_image = null;
    [SerializeField] TextMeshProUGUI m_ReadyText = null;
    [SerializeField] TextMeshProUGUI m_SkinName = null;

    [SerializeField] Settings settings = null;

    private void Start()
    {
        MenuManager.menu.OnPlayerReadyOrNot += SetReadyText;
        SkinManager.OnSkinChanged += UpdateMyInfo;

        MenuManager.menu.Players[iD].Color = settings.playersColor[iD];
        m_image.color = settings.playersColor[iD];

        SetReadyText(iD);
        UpdateMyInfo(iD);
    }

    private void UpdateMyInfo(int _playerID)
    {
        if (_playerID != iD) return;
        m_SkinName.SetText(MenuManager.menu.Players[iD].PrefabName);
    }

    private void SetReadyText(byte _id)
    {
        if (_id != iD) return;

        switch (Translation.GetCurrentLanguage())
        {
            case Languages.en:
                m_ReadyText.text = (MenuManager.menu.Players[_id].Ready) ? "Ready!" : "Press Start";
                break;
            case Languages.es:
                m_ReadyText.text = (MenuManager.menu.Players[_id].Ready) ? "¡Listo!" : "Presiona Start";
                break;
            case Languages.unknown:
                m_ReadyText.text = (MenuManager.menu.Players[_id].Ready) ? "Ready!" : "Press Start";
                break;
            default:
                break;
        }
    }
}

using UnityEngine;
using TMPro;

public class SkinManager : MonoBehaviour
{
    [SerializeField] Skins skinsData;

    [SerializeField] TextMeshProUGUI[] skinNames;

    public delegate void DelSkinManager(bool _CanActive);
    public static DelSkinManager OnSkinsSet;

    private void Awake()
    {
        OnSkinsSet = null;
    }

    private void Start()
    {
        MenuUI.OnMatchmaking += SetSkins;
    }

    private void SetSkins(bool _canActive)
    {
        for (int i = 0; i < skinsData.skins.Count; i++)//CHANGE IT TO MENUMANAGER.MENU.PLAYERS.COUNT
        {
            if (_canActive)
            {
                if (MenuManager.menu.Players.Count != 0)
                {
                    MenuManager.menu.Players[i].Avatar = skinsData.skins[i].prefab; 
                }

                if (skinNames[i].text != skinsData.skins[i].name)
                {
                    skinNames[i].text = skinsData.skins[i].name;  
                }
            }
            skinNames[i].enabled = _canActive;
            skinsData.skins[i].choosed = _canActive;
        }
        OnSkinsSet?.Invoke(_canActive);//MenuManager
    }
}

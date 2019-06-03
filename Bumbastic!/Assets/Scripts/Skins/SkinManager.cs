using UnityEngine;
using TMPro;

public class SkinManager : MonoBehaviour
{
    [SerializeField] Skins skinsData = null;

    [SerializeField] SkinSelector[] skinSelectors = new SkinSelector[0];

    [SerializeField] TextMeshProUGUI[] skinNames = new TextMeshProUGUI[0];

    [SerializeField]
    private GameObject[] spawnPoints = new GameObject[0];

    public delegate void DelSkinManager(bool _CanActive);
    public static DelSkinManager OnSkinsSet;

    private void Awake()
    {
        OnSkinsSet = null;
    }

    private void Start()
    {
        MenuCanvas.onMatchmaking += SetSkins;
        MenuCanvas.onMatchmaking += InitSkinSelectors;

        foreach (SkinSelector skinSelector in skinSelectors)
        {
            skinSelector.OnChangeSkin += ChangeSkin;
            skinSelector.InitSkinSelector();
        }
    }

    private void InitSkinSelectors(bool _canActive)
    {
        if (MenuManager.menu.Players.Count != 0)
        {
            for (int i = 0; i < MenuManager.menu.Players.Count; i++)
            {
                for (int j = 0; j < skinSelectors[i].Buttons.Length; j++)
                {
                    skinSelectors[i].Buttons[j].enabled = _canActive;
                    skinSelectors[i].Images[j].enabled = _canActive;
                }
            }
        } 
    }

    private void ChangeSkin(int _player, int _skinPosition)
    {
        skinsData.skins[_skinPosition].choosed = true;

        if (MenuManager.menu.Players.Count != 0)
        {
            if (MenuManager.menu.Players[_player].Avatar != skinsData.skins[_skinPosition].prefab)
            {
                MenuManager.menu.Players[_player].PrefabName = skinsData.skins[_skinPosition].name;
                MenuManager.menu.Players[_player].SkinSprite = skinsData.skins[_skinPosition].skinSprite;
                skinNames[_player].text = skinsData.skins[_skinPosition].name;

                if (MenuManager.menu.Players[_player].transform.GetChild(0) != null)
                {
                    Destroy(MenuManager.menu.Players[_player].transform.GetChild(0).gameObject);
                }
                
                MenuManager.menu.Players[_player].Avatar = skinsData.skins[_skinPosition].prefab;
                Instantiate(MenuManager.menu.Players[_player].Avatar, spawnPoints[_player].transform.localPosition, spawnPoints[_player].transform.rotation, MenuManager.menu.Players[_player].transform); 
            }
        }
    }

    private void SetSkins(bool _canActive)
    {
        foreach (Skin skin in skinsData.skins)
        {
            skin.choosed = false;
        }

        for (int i = 0; i < MenuManager.menu.Players.Count; i++)
        {
            if (_canActive)
            {
                if (i > skinsData.skins.Count)
                {
                    MenuManager.menu.Players[i].Avatar = skinsData.skins[0].prefab;
                }
                else
                {
                    MenuManager.menu.Players[i].Avatar = skinsData.skins[skinSelectors[i].Position].prefab;
                }

                skinNames[i].text = skinsData.skins[skinSelectors[i].Position].name;  
                skinsData.skins[skinSelectors[i].Position].choosed = _canActive;

                MenuManager.menu.Players[i].PrefabName = skinsData.skins[skinSelectors[i].Position].name;
                MenuManager.menu.Players[i].SkinSprite = skinsData.skins[skinSelectors[i].Position].skinSprite;
                Instantiate(MenuManager.menu.Players[i].Avatar, spawnPoints[i].transform.localPosition, spawnPoints[i].transform.rotation, MenuManager.menu.Players[i].transform);
            }
            else
            {
                Destroy(MenuManager.menu.Players[i].transform.GetChild(0).gameObject);
            }
            skinNames[i].enabled = _canActive;
        }
        OnSkinsSet?.Invoke(_canActive);//MenuManager
    }
}

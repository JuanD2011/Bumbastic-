using UnityEngine;
using TMPro;

public class SkinManager : MonoBehaviour
{
    [SerializeField] Skins skinsData;

    [SerializeField] TextMeshProUGUI[] skinNames;

    [SerializeField]
    private GameObject[] spawnPoints;

    public delegate void DelSkinManager(bool _CanActive);
    public static DelSkinManager OnSkinsSet;

    private void Awake()
    {
        OnSkinsSet = null;
    }

    private void Start()
    {
        MenuUI.OnMatchmaking += SetSkins;
        SkinSelector.OnChangeSkin += ChangeSkin;
    }

    private void ChangeSkin(int _player, int _position)
    {
        Destroy(MenuManager.menu.Players[_player].transform.GetChild(0).gameObject);
        MenuManager.menu.Players[_player].Avatar = skinsData.skins[_position].prefab;
        Instantiate(MenuManager.menu.Players[_player].Avatar, spawnPoints[_player].transform.localPosition, spawnPoints[_player].transform.rotation, MenuManager.menu.Players[_player].transform);
    }

    private void SetSkins(bool _canActive)
    {
        for (int i = 0; i < MenuManager.menu.Players.Count; i++)
        {
            if (_canActive)
            {
                if (MenuManager.menu.Players.Count != 0)
                {
                    if (i > skinsData.skins.Count)
                    {
                        MenuManager.menu.Players[i].Avatar = skinsData.skins[0].prefab;
                    }
                    else
                    {
                        MenuManager.menu.Players[i].Avatar = skinsData.skins[i].prefab;
                    }
                    Instantiate(MenuManager.menu.Players[i].Avatar, spawnPoints[i].transform.localPosition, spawnPoints[i].transform.rotation, MenuManager.menu.Players[i].transform);
                }

                if (skinNames[i].text != skinsData.skins[i].name)
                {
                    skinNames[i].text = skinsData.skins[i].name;  
                }
            }
            else
            {
                Destroy(MenuManager.menu.Players[i].transform.GetChild(0).gameObject);
            }
            skinNames[i].enabled = _canActive;
            skinsData.skins[i].choosed = _canActive;
        }
        OnSkinsSet?.Invoke(_canActive);//MenuManager
    }
}

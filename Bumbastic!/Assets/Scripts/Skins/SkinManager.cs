using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinsDatabase skinsData = null;

    [SerializeField]
    private GameObject[] spawnPoints = new GameObject[0];

    public static event System.Action OnSkinsSet;

    public static event System.Action OnUpdateHUD;

    private void Awake()
    {
        OnUpdateHUD = null;
        OnSkinsSet = null;

        skinsData = Resources.Load<SkinsDatabase>("ScriptableObjects/Skins data");
    }

    private void Start()
    {
        MenuCanvas.OnMatchmaking += SetSkins;

        MenuManager.menu.OnNewPlayerAdded += OnSetNewPlayerSkin;

        SkinSelector.OnChangeSkin += ChangeSkin;
    }

    private void OnSetNewPlayerSkin(byte _playerId)
    {
        for (int i = 0; i < skinsData.skins.Count; i++)
        {
            if (!skinsData.skins[i].choosed)
            {
                MenuManager.menu.Players[_playerId].Avatar = skinsData.skins[_playerId].prefab;
                MenuManager.menu.Players[_playerId].PrefabName = skinsData.skins[_playerId].name;
                MenuManager.menu.Players[_playerId].SkinSprite = skinsData.skins[_playerId].skinSprite;

                skinsData.skins[i].choosed = true;

                if (MenuManager.menu.Players[_playerId].transform.childCount <= 0)
                    Instantiate(MenuManager.menu.Players[_playerId].Avatar, spawnPoints[_playerId].transform.localPosition, spawnPoints[_playerId].transform.rotation, MenuManager.menu.Players[_playerId].transform);
                break;
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
                if (MenuManager.menu.Players[_player].transform.GetChild(0) != null) Destroy(MenuManager.menu.Players[_player].transform.GetChild(0).gameObject);

                MenuManager.menu.Players[_player].Avatar = skinsData.skins[_skinPosition].prefab;
                MenuManager.menu.Players[_player].PrefabName = skinsData.skins[_skinPosition].name;
                MenuManager.menu.Players[_player].SkinSprite = skinsData.skins[_skinPosition].skinSprite;

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
                if (i > skinsData.skins.Count) MenuManager.menu.Players[i].Avatar = skinsData.skins[0].prefab;
                else MenuManager.menu.Players[i].Avatar = skinsData.skins[i].prefab;

                skinsData.skins[i].choosed = _canActive;

                MenuManager.menu.Players[i].PrefabName = skinsData.skins[i].name;
                MenuManager.menu.Players[i].SkinSprite = skinsData.skins[i].skinSprite;

                if (MenuManager.menu.Players[i].transform.childCount <= 0)
                    Instantiate(MenuManager.menu.Players[i].Avatar, spawnPoints[i].transform.localPosition, spawnPoints[i].transform.rotation, MenuManager.menu.Players[i].transform); 
            }
        }
        OnSkinsSet?.Invoke();
    }

    private void OnDisable()
    {
        SkinSelector.OnChangeSkin = null;
    }
}

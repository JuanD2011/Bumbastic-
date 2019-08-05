using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinsDatabase skinsData = null;

    bool alreadySetSkins = false;

    [SerializeField] SpawnLine spawnLine = null;
    [SerializeField] Quaternion initialRotiation = Quaternion.identity;

    public static event System.Action OnSkinsSet;
    public static event System.Action<int> OnSkinChanged;

    private void Awake()
    {
        OnSkinsSet = null;
        OnSkinChanged = null;

        skinsData = Resources.Load<SkinsDatabase>("ScriptableObjects/Skins data");
    }

    private void Start()
    {
        MenuCanvas.OnMatchmaking += SetSkins;

        MenuManager.menu.OnNewPlayerAdded += OnSetNewPlayerSkin;

        SkinSelector.OnChangeSkin += ChangeSkin;
    }

    private void SetSkins(bool _isMatchmaking)
    {
        if (!_isMatchmaking || alreadySetSkins) return;

        foreach (Skin skin in skinsData.skins)
        {
            skin.choosed = false;
        }

        for (int i = 0; i < MenuManager.menu.Players.Count; i++)
        {
            MenuManager.menu.Players[i].Avatar = skinsData.skins[i].prefab;

            skinsData.skins[i].choosed = true;

            MenuManager.menu.Players[i].PrefabName = skinsData.skins[i].name;
            MenuManager.menu.Players[i].SkinSprite = skinsData.skins[i].skinSprite;

            if (MenuManager.menu.Players[i].transform.childCount <= 0)
                Instantiate(MenuManager.menu.Players[i].Avatar, spawnLine.GetSpawnPoint(i + 1), initialRotiation, MenuManager.menu.Players[i].transform);
        }

        alreadySetSkins = true;
        OnSkinsSet?.Invoke();
    }

    private void ChangeSkin(int _playerID, int _skinPosition)
    {
        skinsData.skins[_skinPosition].choosed = true;

        Destroy(MenuManager.menu.Players[_playerID].transform.GetChild(0).gameObject);

        MenuManager.menu.Players[_playerID].Avatar = skinsData.skins[_skinPosition].prefab;
        MenuManager.menu.Players[_playerID].PrefabName = skinsData.skins[_skinPosition].name;
        MenuManager.menu.Players[_playerID].SkinSprite = skinsData.skins[_skinPosition].skinSprite;

        Instantiate(MenuManager.menu.Players[_playerID].Avatar, spawnLine.GetSpawnPoint(_playerID + 1), initialRotiation, MenuManager.menu.Players[_playerID].transform);

        if (MenuManager.menu.Players[_playerID].transform.childCount > 1) Destroy(MenuManager.menu.Players[_playerID].transform.GetChild(0).gameObject);

        OnSkinChanged?.Invoke(_playerID);
    }

    private void OnSetNewPlayerSkin(byte _playerID)
    {
        foreach (PlayerMenu player in MenuManager.menu.Players)
        {
            if (player.transform.childCount <= 0)
                player.transform.GetChild(0).position = spawnLine.GetSpawnPoint(player.Id);
        }

        for (int i = 0; i < skinsData.skins.Count; i++)
        {
            if (!skinsData.skins[i].choosed)
            {
                MenuManager.menu.Players[_playerID].Avatar = skinsData.skins[i].prefab;
                MenuManager.menu.Players[_playerID].PrefabName = skinsData.skins[i].name;
                MenuManager.menu.Players[_playerID].SkinSprite = skinsData.skins[i].skinSprite;

                skinsData.skins[i].choosed = true;

                if (MenuManager.menu.Players[_playerID].transform.childCount <= 0)
                    Instantiate(MenuManager.menu.Players[_playerID].Avatar, spawnLine.GetSpawnPoint(_playerID + 1), initialRotiation, MenuManager.menu.Players[_playerID].transform);
                break;
            }
        }
    }

    private void OnDisable()
    {
        SkinSelector.OnChangeSkin = null;
    }
}

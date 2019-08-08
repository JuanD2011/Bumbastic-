using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public static SkinsDatabase skinsData = null;

    [SerializeField] SpawnLine spawnLine = null;
    [SerializeField] Quaternion initialRotiation = Quaternion.identity;

    Queue<PlayerMenu> newPlayersJoined = new Queue<PlayerMenu>();

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
        MenuCanvas.OnMatchmaking += InitSkinsWCurrentPlayers;
        MenuCanvas.OnMatchmaking += InitSkinsWNewPlayers;

        MenuManager.menu.OnNewPlayerAdded += OnQueuePlayer;

        PlayerInputHandler.OnPlayerDeviceLost += UpdateSkinDeviceChanged;

        SkinSelector.OnChangeSkin += ChangeSkin;
    }

    private void UpdateSkinDeviceChanged(byte _playerID)
    {
        PlayerMenu playerMenuDeviceModified = MenuManager.menu.Players[_playerID];

        if (playerMenuDeviceModified.transform.GetChild(0) != null)
        {
            for (int i = 0; i < skinsData.skins.Count; i++)
            {
                if (skinsData.skins[i].prefab == playerMenuDeviceModified.Avatar)
                {
                    skinsData.skins[i].choosed = false;
                    playerMenuDeviceModified.Avatar = null;
                    Destroy(playerMenuDeviceModified.transform.GetChild(0));
                    UpdateSkinsPosition();
                    break;
                } 
            }
        }
        else
        {
            for (int i = 0; i < skinsData.skins.Count; i++)
            {
                if (skinsData.skins[i].choosed == false)
                {
                    skinsData.skins[i].choosed = true;
                    playerMenuDeviceModified.Avatar = skinsData.skins[i].prefab;
                    Instantiate(playerMenuDeviceModified.Avatar, spawnLine.GetSpawnPoint(playerMenuDeviceModified.Id + 1), initialRotiation, playerMenuDeviceModified.transform);
                    UpdateSkinsPosition();
                    break;
                }
            }
        }
    }

    private void InitSkinsWNewPlayers(bool _isMatchmaking)
    {
        if (!_isMatchmaking && newPlayersJoined.Count <= 0) return;

        PlayerMenu playerMenu = null;

        while (newPlayersJoined.Count > 0)
        {
            for (int i = 0; i < skinsData.skins.Count; i++)
            {
                if (!skinsData.skins[i].choosed)
                {
                    skinsData.skins[i].choosed = true;
                    playerMenu = newPlayersJoined.Dequeue();
                    playerMenu.Avatar = skinsData.skins[i].prefab;
                    playerMenu.PrefabName = skinsData.skins[i].name;
                    playerMenu.SkinSprite = skinsData.skins[i].skinSprite;

                    if (playerMenu.transform.childCount == 0)
                        Instantiate(playerMenu.Avatar, spawnLine.GetSpawnPoint(playerMenu.Id + 1), initialRotiation, playerMenu.transform);
                    break;
                }
            }
        }

        UpdateSkinsPosition();
    }

    private void UpdateSkinsPosition()
    {
        spawnLine.InitDistanceBetweenPlayers();

        foreach (PlayerMenu _playerMenu in MenuManager.menu.Players)
        {
            _playerMenu.transform.GetChild(0).position = spawnLine.GetSpawnPoint(_playerMenu.Id + 1);
        }
    }

    private void OnQueuePlayer(byte _playerID)
    {
        newPlayersJoined.Enqueue(MenuManager.menu.Players[_playerID]);
        if (MenuCanvas.isMatchmaking) InitSkinsWNewPlayers(true);
    }

    private void InitSkinsWCurrentPlayers(bool _isMatchmaking)
    {
        newPlayersJoined.Clear();

        foreach (Skin skin in skinsData.skins)
        {
            skin.choosed = false;
        }

        spawnLine.InitDistanceBetweenPlayers();

        for (int i = 0; i < MenuManager.menu.Players.Count; i++)
        {
            skinsData.skins[i].choosed = true;

            MenuManager.menu.Players[i].Avatar = skinsData.skins[i].prefab;
            MenuManager.menu.Players[i].PrefabName = skinsData.skins[i].name;
            MenuManager.menu.Players[i].SkinSprite = skinsData.skins[i].skinSprite;

            if (MenuManager.menu.Players[i].transform.childCount <= 0)
                Instantiate(MenuManager.menu.Players[i].Avatar, spawnLine.GetSpawnPoint(i + 1), initialRotiation, MenuManager.menu.Players[i].transform);
        }

        OnSkinsSet?.Invoke();
        MenuCanvas.OnMatchmaking -= InitSkinsWCurrentPlayers;
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

    private void OnDisable()
    {
        SkinSelector.OnChangeSkin = null;
        PlayerInputHandler.OnPlayerDeviceLost -= UpdateSkinDeviceChanged;
    }
}

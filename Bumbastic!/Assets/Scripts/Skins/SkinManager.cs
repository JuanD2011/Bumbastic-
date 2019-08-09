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

    public static event System.Action<int, byte> OnUpdatePosition;

    private void Awake()
    {
        OnSkinsSet = null;
        OnSkinChanged = null;
        OnUpdatePosition = null;

        skinsData = Resources.Load<SkinsDatabase>("ScriptableObjects/Skins data");
    }

    private void Start()
    {
        MenuCanvas.OnMatchmaking += InitSkinsWCurrentPlayers;

        MenuManager.menu.OnNewPlayerAdded += OnQueuePlayer;
        SkinSelector.OnChangeSkin += ChangeSkin;

        PlayerInputHandler.OnPlayerDeviceLost += UpdateSkinDeviceChanged;
        PlayerInputHandler.OnPlayerDeviceRegained += UpdateSkinDeviceChanged;
    }

    private void UpdateSkinDeviceChanged(byte _playerID)
    {
        PlayerMenu playerMenuToBeUpdated = MenuManager.menu.Players[_playerID];

        for (int i = 0; i < skinsData.skins.Count; i++)
        {
            if (skinsData.skins[i].prefab == playerMenuToBeUpdated.Avatar && playerMenuToBeUpdated.transform.childCount > 0)
            {
                skinsData.skins[i].choosed = false;
                playerMenuToBeUpdated.Avatar = null;
                Destroy(playerMenuToBeUpdated.transform.GetChild(0).gameObject);
                break;
            }
            else if (playerMenuToBeUpdated.transform.childCount <= 0 && skinsData.skins[i].choosed == false)
            {
                skinsData.skins[i].choosed = true;

                OnUpdatePosition?.Invoke(i, playerMenuToBeUpdated.Id);

                UpdatePlayersSkinInfo(playerMenuToBeUpdated, skinsData.skins[i]);

                Instantiate(playerMenuToBeUpdated.Avatar, spawnLine.GetSpawnPoint(playerMenuToBeUpdated.Id + 1), initialRotiation, playerMenuToBeUpdated.transform);
                break;
            }
        }

        UpdateSkinsPosition();
    }

    private void InitSkinsWNewPlayers(bool _isMatchmaking)
    {
        if (!_isMatchmaking || newPlayersJoined.Count <= 0) return;

        PlayerMenu playerMenu = null;

        while (newPlayersJoined.Count > 0)
        {
            for (int i = 0; i < skinsData.skins.Count; i++)
            {
                if (!skinsData.skins[i].choosed)
                {
                    skinsData.skins[i].choosed = true;
                    playerMenu = newPlayersJoined.Dequeue();

                    UpdatePlayersSkinInfo(playerMenu, skinsData.skins[i]);

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

        foreach (PlayerMenu playerMenu in MenuManager.menu.Players)
        {
            if (playerMenu.transform.childCount > 0) playerMenu.transform.GetChild(0).position = spawnLine.GetSpawnPoint(playerMenu.Id + 1);
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

            UpdatePlayersSkinInfo(MenuManager.menu.Players[i], skinsData.skins[i]);

            if (MenuManager.menu.Players[i].transform.childCount <= 0)
                Instantiate(MenuManager.menu.Players[i].Avatar, spawnLine.GetSpawnPoint(i + 1), initialRotiation, MenuManager.menu.Players[i].transform);
        }

        OnSkinsSet?.Invoke();
        MenuCanvas.OnMatchmaking -= InitSkinsWCurrentPlayers;
        MenuCanvas.OnMatchmaking += InitSkinsWNewPlayers;
    }

    private void ChangeSkin(int _playerID, int _skinPosition)
    {
        skinsData.skins[_skinPosition].choosed = true;

        Destroy(MenuManager.menu.Players[_playerID].transform.GetChild(0).gameObject);

        UpdatePlayersSkinInfo(MenuManager.menu.Players[_playerID], skinsData.skins[_skinPosition]);

        Instantiate(MenuManager.menu.Players[_playerID].Avatar, spawnLine.GetSpawnPoint(_playerID + 1), initialRotiation, MenuManager.menu.Players[_playerID].transform);

        if (MenuManager.menu.Players[_playerID].transform.childCount > 1) Destroy(MenuManager.menu.Players[_playerID].transform.GetChild(0).gameObject);

        OnSkinChanged?.Invoke(_playerID);
    }

    private void UpdatePlayersSkinInfo(PlayerMenu _playerMenu, Skin _skinSource)
    {
        _playerMenu.Avatar = _skinSource.prefab;
        _playerMenu.PrefabName = _skinSource.name;
        _playerMenu.SkinSprite = _skinSource.skinSprite;
    }

    private void OnDisable()
    {
        SkinSelector.OnChangeSkin = null;
        PlayerInputHandler.OnPlayerDeviceLost -= UpdateSkinDeviceChanged;
        PlayerInputHandler.OnPlayerDeviceRegained -= UpdateSkinDeviceChanged;
    }
}

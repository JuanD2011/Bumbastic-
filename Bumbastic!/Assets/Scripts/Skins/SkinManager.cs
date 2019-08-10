using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public static SkinsDatabase skinsData = null;

    [SerializeField] SpawnLine spawnLine = null;

    Queue<PlayerMenu> newPlayersJoined = new Queue<PlayerMenu>();

    public static event System.Action OnSkinsSet;
    public static event System.Action<int> OnSkinChanged;

    public static event System.Action<int, byte> OnUpdateSkinSelectorsPosition;

    private void Awake()
    {
        OnSkinsSet = null;
        OnSkinChanged = null;
        OnUpdateSkinSelectorsPosition = null;
        SkinSelector.OnChangeSkin = null;

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
            else if (playerMenuToBeUpdated.transform.childCount <= 0 && skinsData.skins[i].choosed == false && MenuCanvas.isMatchmaking)
            {
                skinsData.skins[i].choosed = true;

                OnUpdateSkinSelectorsPosition?.Invoke(i, playerMenuToBeUpdated.Id);

                UpdatePlayersSkinInfo(playerMenuToBeUpdated, skinsData.skins[i]);

                Instantiate(playerMenuToBeUpdated.Avatar, playerMenuToBeUpdated.transform);
                break;
            }
        }
        spawnLine.InitPlayersPosition();
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
                        Instantiate(playerMenu.Avatar, playerMenu.transform);
                    break;
                }
            }
        }
        spawnLine.InitPlayersPosition();
    }

    private void OnQueuePlayer(byte _playerID)
    {
        foreach (PlayerMenu playerMenu in MenuManager.menu.Players)
        {
            if (playerMenu.Id == _playerID)
            {
                newPlayersJoined.Enqueue(playerMenu);
                break;
            }
        }
        if (MenuCanvas.isMatchmaking) InitSkinsWNewPlayers(true);
    }

    private void InitSkinsWCurrentPlayers(bool _isMatchmaking)
    {
        newPlayersJoined.Clear();

        foreach (Skin skin in skinsData.skins)
        {
            skin.choosed = false;
        }

        for (int i = 0; i < MenuManager.menu.Players.Count; i++)
        {
            skinsData.skins[i].choosed = true;

            UpdatePlayersSkinInfo(MenuManager.menu.Players[i], skinsData.skins[i]);

            if (MenuManager.menu.Players[i].transform.childCount <= 0)
                Instantiate(MenuManager.menu.Players[i].Avatar, MenuManager.menu.Players[i].transform);
        }

        spawnLine.InitPlayersPosition();

        OnSkinsSet?.Invoke();
        MenuCanvas.OnMatchmaking -= InitSkinsWCurrentPlayers;
        MenuCanvas.OnMatchmaking += InitSkinsWNewPlayers;
    }

    private void ChangeSkin(int _playerID, int _skinPosition)
    {
        skinsData.skins[_skinPosition].choosed = true;

        Destroy(MenuManager.menu.Players[_playerID].transform.GetChild(0).gameObject);

        UpdatePlayersSkinInfo(MenuManager.menu.Players[_playerID], skinsData.skins[_skinPosition]);
        Instantiate(MenuManager.menu.Players[_playerID].Avatar, MenuManager.menu.Players[_playerID].transform);

        if (MenuManager.menu.Players[_playerID].transform.childCount > 1) Destroy(MenuManager.menu.Players[_playerID].transform.GetChild(0).gameObject);

        spawnLine.SetPlayerPosition(_playerID);
        OnSkinChanged?.Invoke(_playerID);
    }

    private void UpdatePlayersSkinInfo(PlayerMenu _playerMenu, Skin _skinSource)
    {
        _playerMenu.Avatar = _skinSource.prefab;
        _playerMenu.PrefabName = _skinSource.name;
        _playerMenu.SkinSprite = _skinSource.skinSprite;
    }
}

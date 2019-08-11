﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private GameModeDataBase gameMode = null;

    [SerializeField]
    Settings settings = null;

    [SerializeField]
    private float startTimer = 5f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI countdownText = null;

    [SerializeField]
    private GameObject playerMenuPrefab = null;

    private bool countdown = false;
    bool go = false;

    private int playersReady = 0;
    private int maxPlayers = 0;

    List<PlayerMenu> players = new List<PlayerMenu>();

    public event System.Action<string> OnStartGame;
    public event System.Action<bool> OnCountdown;
    public event System.Action<byte> OnNewPlayerAdded = null;

    public List<PlayerMenu> Players { get => players; private set => players = value; }

    public MenuCanvas menuCanvas;

    private void Awake()
    {
        if (menu == null) menu = this;
        else Destroy(this);

        Memento.LoadData();
        PlayerMenu.ResetDel();

        SetLanguage();
    }

    private void SetLanguage()
    {
        Translation.currentLanguageId = settings.languageID;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.currentLanguageId]);
    }

    void Start()
    {
        timer = startTimer;

        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips.inGameMusic, 0.6f, 0.6f, 0.6f);

        PlayerMenu.OnReady += PlayersReady;
        PlayerMenu.OnNotReady += PlayerNotReady;
        InputManager.inputManager.OnDeviceAdded += AddNewPlayer;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }

        if (countdown)
        {
            timer -= Time.deltaTime;

            if (Mathf.RoundToInt(timer) <= 0)
            {
                if (!go)
                {
                    countdownText.text = Translation.Fields["Go"];
                    go = true;
                }
            }
            else
                countdownText.text = string.Format("{0}", Mathf.RoundToInt(timer));

            if (timer <= 0f)
            {
                if (countdownText.text != "")
                {
                    countdownText.text = "";
                }
                StartGame();
                countdown = false;
            }
        }
    }

    private void AddNewPlayer()
    {
        PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
        player.Id = (byte)Players.Count;
        Players.Add(player);
        maxPlayers = Players.Count;

        OnNewPlayerAdded?.Invoke(player.Id);
    }

    private void StartGame()
    {
        InGame.playerSettings.Clear();
        for (int i = 0; i < Players.Count; i++)
        {
            InGame.playerSettings.Add(new PlayerSettings(Players[i].PrefabName, Players[i].Avatar, Players[i].SkinSprite, Players[i].Color));
        }
        gameMode.GetNextGameMode();
        OnStartGame?.Invoke("GameMode");
    }

    public void InitializeFirstPlayers(int _gamepadCount)
    {
        for (int i = 0; i < _gamepadCount; i++)
        {
            PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
            player.Id = (byte) i;
            Players.Add(player);
        }
        maxPlayers = _gamepadCount;
    }

    public void PlayersReady(byte _id)
    {
        playersReady = (playersReady > maxPlayers) ? playersReady = maxPlayers : playersReady += 1;

        if (playersReady == maxPlayers && maxPlayers > 1)
        {
            countdown = true;
            OnCountdown?.Invoke(true);
        }
    }

    public void PlayerNotReady(byte _id)
    {
        playersReady = (playersReady > 0) ? playersReady -= 1 : playersReady = 0;

        countdown = false;
        timer = startTimer;

        OnCountdown?.Invoke(false);
    }

    public void SaveData(int _id)
    {
        Memento.SaveData(_id);
    }
}

﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    Settings settings;

    [SerializeField]
    private float startTimer = 5f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI[] texts;

    [SerializeField]
    private TextMeshProUGUI[] playersIDs;

    [SerializeField]
    private Image[] playerColors;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private GameObject playerMenuPrefab;

    [SerializeField]
    private GameObject[] spawnPoints;

    private bool countdown = false;

    private byte playersReady = 0;

    List<PlayerMenu> players = new List<PlayerMenu>();

    public delegate void DelMenuManager(string _sceneName);
    public DelMenuManager OnStartGame;

    public delegate void SetCountdown(bool _bool);
    public SetCountdown OnCountdown;

    public delegate void InputsDelegate(List<string> _players);
    public InputsDelegate OnFirstPlayers;

    public List<PlayerMenu> Players { get => players; private set => players = value; }

    private void Awake()
    {
        if (menu == null) menu = this;
        else Destroy(this);
    }

    void Start()
    {
        timer = startTimer;

        PlayerMenu.OnReady += PlayersReady;
        PlayerMenu.OnNotReady += PlayerNotReady;
        SkinManager.OnSkinsSet += SetUpMatchMaking;

        SetPlayersColor();
    }

    private void SetPlayersColor()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            playerColors[i].color = settings.playersColor[i];
        }
    }

    private void SetUpMatchMaking(bool _canActive)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            playerColors[i].enabled = _canActive;
            playersIDs[i].enabled = _canActive;
            texts[i].enabled = _canActive;

            if (_canActive)
            {
                Instantiate(Players[i].Avatar, spawnPoints[i].transform.localPosition, spawnPoints[i].transform.rotation, Players[i].transform);
            }
            else
            {
                Destroy(Players[i].transform.GetChild(0).gameObject);
            }
        }
    }

    void Update()
    {
        if (countdown)
        {
            timer -= Time.deltaTime;

            countdownText.text = string.Format("{0}", Mathf.RoundToInt(timer));

            if (Mathf.RoundToInt(timer) == 0)
            {
                if (countdownText.text != "Go!")
                {
                    countdownText.text = string.Format("Go!");
                }
            }

            if (timer <= 0f)
            {
                if (countdownText.text != "")
                {
                    countdownText.text = "";
                }
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        inGame.playerSettings.Clear();
        for (int i = 0; i < Players.Count; i++)
        {
            inGame.playerSettings.Add(new PlayerSettings("Danson", Players[i].Avatar, Players[i].Controls));
        }
        OnStartGame?.Invoke("Game");//MenuUI hears it.
        Debug.Log("The game has started");
    }

    public void InitializeFirstPlayers(List<string> _joysticks)
    {
        for (int i = 0; i < _joysticks.Count; i++)
        {
            PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
            player.Id = (byte) i;
            Players.Add(player);
        }

        InputManager.inputManager.AssignController(_joysticks);
    }

    public void PlayersReady(byte _id)
    {
        playersReady++;
        texts[_id].text = "Ready";
        if (playersReady == inGame.maxPlayers)
        {
            countdown = true;
            OnCountdown?.Invoke(true);//MenuUI hears it.
        }
        else
        {
            countdown = false;
        }
    }

    public void PlayerNotReady(byte _id)
    {
        playersReady--;
        texts[_id].text = "Press Start";
        countdown = false;
        timer = startTimer;
    }
}

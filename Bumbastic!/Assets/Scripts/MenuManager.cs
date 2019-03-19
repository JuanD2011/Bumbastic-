﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private float startTimer = 5f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI[] texts;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private GameObject playerMenuPrefab;

    private bool countdown = false;

    private byte playersReady = 0;

    private List<Controls> playerControls = new List<Controls>();
    List<PlayerMenu> players = new List<PlayerMenu>();

    public delegate void DelMenuManager(string _sceneName);
    public DelMenuManager OnStartGame;

    public delegate void SetCountdown(bool _bool);
    public SetCountdown OnCountdown;

    public delegate void InputsDelegate(List<string> _players);
    public InputsDelegate OnFirstPlayers;

    public List<PlayerMenu> Players { get => players; }

    private void Awake()
    {
        if (menu == null) menu = this;
        else Destroy(this);
    }

    void Start()
    {
        timer = startTimer;
        InputManager.StartInputs += InitializeFirstPlayers;
    }

    void Update()
    {
        if (countdown)
        {
            timer -= Time.deltaTime;

            countdownText.text = string.Format("{0}", Mathf.RoundToInt(timer));

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
        for (int i = 0; i < Players.Count; i++)
        {
            inGame.playerSettings.Add(new PlayerSettings("Danson", Players[i].Avatar, Players[i].Controls));
        }
        OnStartGame?.Invoke("Game");//MenuUI hears it.
        Debug.Log("The game has started");
    }

    private void InitializeFirstPlayers(List<string> _joysticks)
    {
        for (int i = 0; i < _joysticks.Count; i++)
        {
            PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
            Players.Add(player);
        }

        OnFirstPlayers?.Invoke(_joysticks);
    }

    public void PlayersReady(byte id)
    {
        playersReady++;
        texts[id].text = "Ready";
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

    public void PlayerNotReady(byte id)
    {
        playersReady--;
        texts[id].text = "Press Start";
        countdown = false;
        timer = startTimer;
    }
}

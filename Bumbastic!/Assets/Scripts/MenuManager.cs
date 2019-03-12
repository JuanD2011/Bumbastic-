using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    private List<PlayerInput> players = new List<PlayerInput>();

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
    private GameObject player;

    private bool countdown = false;

    private byte playersReady = 0;

    public delegate void DelMenuManager(string _sceneName);
    public DelMenuManager OnStartGame;

    public Action OnSetCountdown;
  
    private void Awake()
    {
        if (menu == null) menu = this;
        else Destroy(this);
    }

    void Start()
    {
        timer = startTimer;
        InputSystem.onDeviceChange += (device, change) => DeviceChange(device, change);
        InitializeControls();
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

    public void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log("Player Joined");
        players.Add(player);
        if (player.playerIndex != 0)
        {
            texts[player.playerIndex].enabled = true; 
        }
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        Debug.Log("Player Left");
    }

    private void StartGame()
    {
        OnStartGame?.Invoke("Game");//MenuUI hears it.
        inGame.players = players;
        Debug.Log("The game has started");
    }

    private void DeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                Instantiate(player);
                break;
            case InputDeviceChange.Removed:
                Debug.Log("Removed");
                Disconnect(device);
                break;
            case InputDeviceChange.Reconnected:
                Instantiate(player);
                break;
            case InputDeviceChange.Disabled:
                Debug.Log("Disabled");
                break;
            default:
                break;
        }
    }

    private void InitializeControls()
    {
        foreach (Gamepad gamePad in Gamepad.all)
        {
            Instantiate(player);
        }
    }

    private void Disconnect(InputDevice device)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].devices[0] == device)
            {
                texts[i].enabled = false;
                if (players[i].gameObject != null)
                {
                    Destroy(players[i].gameObject); 
                }
            }
        }
    }

    public void PlayersReady(byte id)
    {
        playersReady++;
        texts[id].text = "Ready";
        if (playersReady == inGame.maxPlayers)
        {
            countdown = true;
            OnSetCountdown?.Invoke();//MenuUI hears it.
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

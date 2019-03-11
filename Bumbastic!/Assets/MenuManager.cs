using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();

    [SerializeField]
    private InGame settings;

    [SerializeField]
    private float startTimer = 5f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI[] texts;

    [SerializeField]
    private GameObject player;

    private bool countdown = false;

    private byte playersReady = 0;

    void Start()
    {
        timer = startTimer;
        InputSystem.onDeviceChange += (device, change) => DeviceChange(device, change);
        FindObjectOfType<PlayerMenu>().OnReady += PlayersReady;
        FindObjectOfType<PlayerMenu>().OnNotReady += PlayerNotReady;
        InitializeControls();
    }

    void Update()
    {
        if (countdown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
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

    private void PlayersReady(byte id)
    {
        playersReady++;
        texts[id].text = "Ready";
        if (playersReady == settings.maxPlayers)
        {
            countdown = true;
        }
        else
        {
            countdown = false;
        }
    }

    private void PlayerNotReady(byte id)
    {
        playersReady--;
        texts[id].text = "Press Start";
        countdown = false;
        timer = startTimer;
    }
}

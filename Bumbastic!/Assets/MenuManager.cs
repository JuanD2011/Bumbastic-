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

    [SerializeField]
    private TextMeshProUGUI[] texts;

    [SerializeField]
    private GameObject player;

    private bool countdown = false;

    void Start()
    {
        InputSystem.onDeviceChange += (device, change) => DeviceChange(device, change);
        InitializeControls();
    }

    void Update()
    {
        if (countdown)
        {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0f)
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
        if (players.Count == settings.maxPlayers)
        {
            countdown = true;
        }
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
                Disconnect(device);
                break;
            case InputDeviceChange.Disconnected:
                Disconnect(device);
                break;
            case InputDeviceChange.Reconnected:
                Instantiate(player);
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
        foreach (PlayerInput player in players)
        {
            if (player.devices[0] == device)
            {
                Destroy(player.gameObject);
            }
        }
    }
}

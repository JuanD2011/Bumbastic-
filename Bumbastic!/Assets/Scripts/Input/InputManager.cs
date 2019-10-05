﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;

    public event System.Action OnDeviceDisconnected, OnDeviceReconnected;
    public event System.Action OnDeviceAdded;

    private void Awake()
    {
        if (inputManager == null) inputManager = this;
        else Destroy(this);

        PlayerInputHandler.ResetMyEvents();
        InputSystem.onDeviceChange -= UpdateGamepadState;
        GetGamepads();
    }

    private void Start()
    {
        InputSystem.onDeviceChange += UpdateGamepadState;
    }

    private void UpdateGamepadState(InputDevice _device, InputDeviceChange _change)
    {
        if (Application.isPlaying)
        {
            List<InputDevice> inputList = new List<InputDevice>();

            foreach (PlayerMenu item in MenuManager.menu.Players)
            {
                inputList.Add(item.InputDevice);
            }

            switch (_change)
            {
                case InputDeviceChange.Added:
                    if (!inputList.Contains(_device))
                    {
                        OnDeviceAdded?.Invoke();
                        Debug.Log("Device added with id: " + (_device.deviceId));
                    }
                    break;
                case InputDeviceChange.Removed:
                    //Device completely removed
                    break;
                case InputDeviceChange.Disconnected:
                    Debug.Log("Device disconnected with id: " + (_device.deviceId));
                    OnDeviceDisconnected?.Invoke();
                    break;
                case InputDeviceChange.Reconnected:
                    Debug.Log("Device reconnected with id: " + (_device.deviceId));
                    OnDeviceReconnected?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

    private void GetGamepads()
    {
        int gamepadCount = Gamepad.all.Count;
        MenuManager.menu.InitializeFirstPlayers(gamepadCount);
    }
}

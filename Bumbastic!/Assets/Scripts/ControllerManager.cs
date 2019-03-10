using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class ControllerManager : MonoBehaviour
{
    [SerializeField]
    private InputManager controls;

    void Start()
    {
        AssignFirstController();
        InputSystem.onDeviceChange += (device, change) => DeviceChange(device, change);
    }

    private void DeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                /* New Device */
                break;
            case InputDeviceChange.Disconnected:
                /* Device got unplugged */
                break;
            case InputDeviceChange.Reconnected:
                /* Plugged back in */
                break;
            case InputDeviceChange.Removed:
                /* Remove from input system entirely; by default, devices stay in the system once discovered */
                break;
        }
    }

    private void AssignFirstController()
    {
        foreach(InputDevice device in InputSystem.devices)
        {
            if (device is Gamepad)
            {
                GameManager.manager.Players[0].Device = device;
            }
        }
    }
}

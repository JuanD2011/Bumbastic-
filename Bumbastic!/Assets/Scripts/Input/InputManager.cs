using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;

    public event System.Action OnDeviceDisconnected, OnDeviceReconnected, OnDeviceAdded;

    private void Awake()
    {
        if (inputManager == null)
            inputManager = this;
        else
            Destroy(this);

        GetGamepads();

        InputSystem.onDeviceChange += UpdateGamepadState;
    }

    private void UpdateGamepadState(InputDevice _device, InputDeviceChange _change)
    {
        if (Application.isPlaying)
        {
            switch (_change)
            {
                case InputDeviceChange.Added:
                    OnDeviceAdded?.Invoke();
                    Debug.Log("Device added with id: " + (_device.id - 10));
                    break;
                case InputDeviceChange.Removed:
                    //Device completely removed
                    break;
                case InputDeviceChange.Disconnected:
                    Debug.Log("Device disconnected with id: " + (_device.id - 10));
                    OnDeviceDisconnected?.Invoke();
                    break;
                case InputDeviceChange.Reconnected:
                    Debug.Log("Device reconnected with id: " + (_device.id - 10));
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

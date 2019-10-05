using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInputManager : MonoBehaviour
{
    private void Start()
    {
        InputSystem.onDeviceChange += UpdateGamepadState;
    }
    private void UpdateGamepadState(InputDevice _device, InputDeviceChange _change)
    {
        if (Application.isPlaying)
        {
            switch (_change)
            {
                case InputDeviceChange.Added:
                    Debug.Log("Device added with id: " + (_device.deviceId));
                    break;
                case InputDeviceChange.Removed:
                    //Device completely removed
                    break;
                case InputDeviceChange.Disconnected:
                    Debug.Log("Device disconnected with id: " + (_device.deviceId));
                    break;
                case InputDeviceChange.Reconnected:
                    Debug.Log("Device reconnected with id: " + (_device.deviceId));
                    break;
                default:
                    break;
            }
        }
    }
}

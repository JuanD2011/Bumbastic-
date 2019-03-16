using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public delegate void InputDelegate(byte _number);
    public static InputDelegate StartInputs;

    private string[] joysticks;
    private List<string> activeJoysticks;
    private byte joystickNumber;

    private void Start()
    {
        joysticks = Input.GetJoystickNames();
        joystickNumber = (byte)joysticks.Length;

        for (int i = 0; i < joysticks.Length; ++i)
        {
            if (!string.IsNullOrEmpty(joysticks[i]))
            {
                Debug.Log("Controller " + i + " is connected using: " + joysticks[i]);
                if (!activeJoysticks.Contains(joysticks[i]))
                {
                    activeJoysticks.Add(joysticks[i]);
                }
            }
            else
            {
                Debug.Log("Controller: " + i + " is disconnected.");
                if (activeJoysticks.Contains(joysticks[i]))
                {
                    activeJoysticks.Remove(joysticks[i]);
                }
            }
        }

        StartInputs?.Invoke((byte)activeJoysticks.Count);

        MenuManager.menu.OnFirstPlayers += AssignController;
    }

    private void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            joysticks = Input.GetJoystickNames();

            if (joysticks.Length != joystickNumber)
            {
                for (int i = 0; i < joysticks.Length; ++i)
                {
                    if (!string.IsNullOrEmpty(joysticks[i]))
                    {
                        Debug.Log("Controller " + i + " is connected using: " + joysticks[i]);
                        if (!activeJoysticks.Contains(joysticks[i]))
                        {
                            activeJoysticks.Add(joysticks[i]); 
                            joystickNumber = (byte)joysticks.Length;
                        }
                    }
                    else
                    {
                        Debug.Log("Controller: " + i + " is disconnected.");
                        if (activeJoysticks.Contains(joysticks[i]))
                        {
                            activeJoysticks.Remove(joysticks[i]); 
                            joystickNumber = (byte)joysticks.Length;
                        }
                    }
                }
            }
        }
    }

    private void AssignController(List<PlayerMenu> _players)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Controls = new Controls((byte)i);
        }
    }
}

public struct Controls
{
    public string aButton;
    public string bButton;
    public string startButton;
    public string ljoystickHorizontal;
    public string ljoystickVertical;
    public string rjoystickHorizontal;
    public string rjoystickVertical;
    public string rightTrigger;

    public Controls(byte _joystickNumber) : this()
    {
        switch (_joystickNumber)
        {
            case 0:
                aButton = "0AButton";
                bButton = "0BButton";
                startButton = "0StartButton";
                ljoystickHorizontal = "0Horizontal";
                ljoystickVertical = "0Vertical";
                rjoystickVertical = "0RVertical";
                rjoystickHorizontal = "0RHorizontal";
                rightTrigger = "0RightTrigger";
                break;
            case 1:
                aButton = "1AButton";
                bButton = "1BButton";
                startButton = "1StartButton";
                ljoystickHorizontal = "1Horizontal";
                ljoystickVertical = "1Vertical";
                rjoystickVertical = "1RVertical";
                rjoystickHorizontal = "1RHorizontal";
                rightTrigger = "1RightTrigger";
                break;
            case 2:
                aButton = "2AButton";
                bButton = "2BButton";
                startButton = "2StartButton";
                ljoystickHorizontal = "2Horizontal";
                ljoystickVertical = "2Vertical";
                rjoystickVertical = "2RVertical";
                rjoystickHorizontal = "2RHorizontal";
                rightTrigger = "2RightTrigger";
                break;
            case 3:
                aButton = "3AButton";
                bButton = "3BButton";
                startButton = "3StartButton";
                ljoystickHorizontal = "3Horizontal";
                ljoystickVertical = "3Vertical";
                rjoystickVertical = "3RVertical";
                rjoystickHorizontal = "3RHorizontal";
                rightTrigger = "3RightTrigger";
                break;
        }
    }
}

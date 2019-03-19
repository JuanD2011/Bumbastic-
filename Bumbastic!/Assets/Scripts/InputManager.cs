using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;

    private string[] joysticks;
    private List<string> activeJoysticks = new List<string>();
    private byte joystickNumber;
    private List<PlayerMenu> playerMenus = new List<PlayerMenu>();

    private void Awake()
    {
        if (inputManager == null)
        {
            inputManager = this;
        }
    }

    private void Start()
    {
        GetJoysticks();

        MenuManager.menu.OnFirstPlayers += AssignController;
    }

    private void GetJoysticks()
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

            Debug.Log(activeJoysticks[i]);
        }

        MenuManager.menu.InitializeFirstPlayers(activeJoysticks);
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
                            UpdateControllers();
                        }
                    }
                    else
                    {
                        Debug.Log("Controller: " + i + " is disconnected.");
                        if (activeJoysticks.Contains(joysticks[i]))
                        {
                            activeJoysticks.Remove(joysticks[i]); 
                            joystickNumber = (byte)joysticks.Length;
                            UpdateControllers();
                        }
                    }
                }
            }
        }
    }

    public void AssignController(List<string> _joysticks)
    {
        Debug.Log("Assign Controller");
        for (int i = 0; i < _joysticks.Count; i++)
        {
            MenuManager.menu.Players[i].Controls = new Controls((byte)_joysticks.Count, joysticks[i]);
        }
    }

    private void UpdateControllers()
    {

    }
}

public struct Controls
{
    public string controllerName;
    public string aButton;
    public string bButton;
    public string startButton;
    public string ljoystickHorizontal;
    public string ljoystickVertical;
    public string rjoystickHorizontal;
    public string rjoystickVertical;
    public string rightTrigger;

    public Controls(byte _joystickNumber, string _controllerName) : this()
    {
        switch (_joystickNumber)
        {
            case 0:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = "0AButton";
                    bButton = "0BButton";
                    startButton = "0StartButton";
                    rightTrigger = "0RightTrigger";
                    rjoystickHorizontal = "0RHorizontal";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = "P0AButton";
                    bButton = "P0BButton";
                    startButton = "P0StartButton";
                    rightTrigger = "P0RightTrigger";
                    rjoystickHorizontal = "P0RHorizontal";
                }
                rjoystickVertical = "0RVertical";
                ljoystickHorizontal = "0Horizontal";
                ljoystickVertical = "0Vertical";  
                break;

            case 1:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = "1AButton";
                    bButton = "1BButton";
                    startButton = "1StartButton";
                    rightTrigger = "1RightTrigger";
                    rjoystickHorizontal = "1RHorizontal";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = "P1AButton";
                    bButton = "P1BButton";
                    startButton = "P1StartButton";
                    rightTrigger = "P1RightTrigger";
                    rjoystickHorizontal = "P1RHorizontal";
                }
                rjoystickVertical = "1RVertical";
                ljoystickHorizontal = "1Horizontal";
                ljoystickVertical = "1Vertical";
                break;
            case 2:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = "2AButton";
                    bButton = "2BButton";
                    startButton = "2StartButton";
                    rightTrigger = "2RightTrigger";
                    rjoystickHorizontal = "2RHorizontal";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = "P2AButton";
                    bButton = "P2BButton";
                    startButton = "P2StartButton";
                    rightTrigger = "P2RightTrigger";
                    rjoystickHorizontal = "P2RHorizontal";
                }
                rjoystickVertical = "2RVertical";
                ljoystickHorizontal = "2Horizontal";
                ljoystickVertical = "2Vertical";
                break;
            case 3:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = "3AButton";
                    bButton = "3BButton";
                    startButton = "3StartButton";
                    rightTrigger = "3RightTrigger";
                    rjoystickHorizontal = "3RHorizontal";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = "P3AButton";
                    bButton = "P3BButton";
                    startButton = "P3StartButton";
                    rightTrigger = "P3RightTrigger";
                    rjoystickHorizontal = "P3RHorizontal";
                }
                rjoystickVertical = "3RVertical";
                ljoystickHorizontal = "3Horizontal";
                ljoystickVertical = "3Vertical";
                break;
        }
    }
}

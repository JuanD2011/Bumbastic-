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
                activeJoysticks.Add(joysticks[i]);
            }
            else
            {
                Debug.Log("Controller: " + i + " is disconnected.");
                activeJoysticks.Remove(joysticks[i]);
            }
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
            MenuManager.menu.Players[i].Controls = new Controls((byte)i, joysticks[i]);
        }
    }

    private void UpdateControllers()
    {

    }
}

public struct Controls
{
    public string controllerName;
    public KeyCode aButton;
    public KeyCode bButton;
    public KeyCode startButton;
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
                    aButton = KeyCode.Joystick1Button0;
                    bButton = KeyCode.Joystick1Button1;
                    startButton = KeyCode.Joystick1Button7;
                    rightTrigger = "0RightTrigger";
                    rjoystickHorizontal = "0RHorizontal";
                    rjoystickVertical = "0RVertical";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = KeyCode.Joystick1Button1;
                    bButton = KeyCode.Joystick1Button2;
                    startButton = KeyCode.Joystick1Button9;
                    rightTrigger = "P0RightTrigger";
                    rjoystickHorizontal = "P0RHorizontal";
                    rjoystickVertical = "P0RVertical";
                }              
                ljoystickHorizontal = "0Horizontal";
                ljoystickVertical = "0Vertical";  
                break;

            case 1:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = KeyCode.Joystick2Button0;
                    bButton = KeyCode.Joystick2Button1;
                    startButton = KeyCode.Joystick2Button7;
                    rightTrigger = "1RightTrigger";
                    rjoystickHorizontal = "1RHorizontal";
                    rjoystickVertical = "1RVertical";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = KeyCode.Joystick2Button1;
                    bButton = KeyCode.Joystick2Button2;
                    startButton = KeyCode.Joystick2Button9;
                    rightTrigger = "P1RightTrigger";
                    rjoystickHorizontal = "P1RHorizontal";
                    rjoystickVertical = "P1RVertical";
                }
                ljoystickHorizontal = "1Horizontal";
                ljoystickVertical = "1Vertical";
                break;
            case 2:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = KeyCode.Joystick3Button0;
                    bButton = KeyCode.Joystick3Button1;
                    startButton = KeyCode.Joystick3Button7;
                    rightTrigger = "2RightTrigger";
                    rjoystickHorizontal = "2RHorizontal";
                    rjoystickVertical = "2RVertical";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = KeyCode.Joystick3Button1;
                    bButton = KeyCode.Joystick3Button2;
                    startButton = KeyCode.Joystick3Button9;
                    rightTrigger = "P2RightTrigger";
                    rjoystickHorizontal = "P2RHorizontal";
                    rjoystickVertical = "P2RVertical";
                }
                ljoystickHorizontal = "2Horizontal";
                ljoystickVertical = "2Vertical";
                break;
            case 3:
                controllerName = _controllerName;
                if (controllerName.Length > 31)
                {
                    aButton = KeyCode.Joystick4Button0;
                    bButton = KeyCode.Joystick4Button1;
                    startButton = KeyCode.Joystick4Button7;
                    rightTrigger = "3RightTrigger";
                    rjoystickHorizontal = "3RHorizontal";
                    rjoystickVertical = "3RVertical";
                }
                else if (controllerName.Length > 18)
                {
                    aButton = KeyCode.Joystick4Button1;
                    bButton = KeyCode.Joystick4Button2;
                    startButton = KeyCode.Joystick4Button9;
                    rightTrigger = "P3RightTrigger";
                    rjoystickHorizontal = "P3RHorizontal";
                    rjoystickVertical = "P3RVertical";
                }
                ljoystickHorizontal = "3Horizontal";
                ljoystickVertical = "3Vertical";
                break;
        }
    }
}

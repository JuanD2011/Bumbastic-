using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.Input;

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
            inputManager = this;
        else
            Destroy(this);

        GetGamepads();
    }

    private void Start()
    {
        //GetJoysticks();
    }

    private void GetGamepads()
    { 
        int gamepadCount = Gamepad.all.Count + (Joystick.all.Count / 4);
        MenuManager.menu.InitializeFirstPlayers(gamepadCount);
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

        //MenuManager.menu.InitializeFirstPlayers(activeJoysticks);
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

public class Controls
{
    public string controllerName;
    public KeyCode aButton;
    public KeyCode bButton;
    public KeyCode startButton;
    public KeyCode rightButtonTrigger;
    public KeyCode leftBumper;
    public KeyCode rightBumper;
    public string ljoystickHorizontal;
    public string ljoystickVertical;
    public string rjoystickHorizontal;
    public string rjoystickVertical;
    public string rightAxisTrigger;

    public Controls(byte _joystickNumber, string _controllerName)
    {
        switch (_joystickNumber)
        {
            case 0:
                controllerName = _controllerName;
                if (controllerName.Length == 33 || controllerName.Length == 32 || controllerName.Length == 23)
                {
                    aButton = KeyCode.Joystick1Button0;
                    bButton = KeyCode.Joystick1Button1;
                    startButton = KeyCode.Joystick1Button7;
                    rightAxisTrigger = "0RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "0RHorizontal";
                    rjoystickVertical = "0RVertical";
                }
                else if (controllerName.Length == 19)
                {
                    aButton = KeyCode.Joystick1Button1;
                    bButton = KeyCode.Joystick1Button2;
                    startButton = KeyCode.Joystick1Button9;
                    rightAxisTrigger = "P0RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "P0RHorizontal";
                    rjoystickVertical = "P0RVertical";
                }
                else if (controllerName.Length == 22 || controllerName.Length == 25)
                {
                    aButton = KeyCode.Joystick1Button2;
                    bButton = KeyCode.Joystick1Button1;
                    startButton = KeyCode.Joystick1Button9;
                    rightButtonTrigger = KeyCode.Joystick1Button7;
                    rightAxisTrigger = "";
                    rjoystickHorizontal = "G0RHorizontal";
                    rjoystickVertical = "G0RVertical";
                }
                ljoystickHorizontal = "0Horizontal";
                ljoystickVertical = "0Vertical";
                leftBumper = KeyCode.Joystick1Button4;
                rightBumper = KeyCode.Joystick1Button5;
                break;

            case 1:
                controllerName = _controllerName;
                if (controllerName.Length == 33 || controllerName.Length == 32 || controllerName.Length == 23)
                {
                    aButton = KeyCode.Joystick2Button0;
                    bButton = KeyCode.Joystick2Button1;
                    startButton = KeyCode.Joystick2Button7;
                    rightAxisTrigger = "1RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "1RHorizontal";
                    rjoystickVertical = "1RVertical";
                }
                else if (controllerName.Length == 19)
                {
                    aButton = KeyCode.Joystick2Button1;
                    bButton = KeyCode.Joystick2Button2;
                    startButton = KeyCode.Joystick2Button9;
                    rightAxisTrigger = "P1RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "P1RHorizontal";
                    rjoystickVertical = "P1RVertical";
                }
                else if (controllerName.Length == 22 || controllerName.Length == 25)
                {
                    aButton = KeyCode.Joystick2Button2;
                    bButton = KeyCode.Joystick2Button1;
                    startButton = KeyCode.Joystick2Button9;
                    rightButtonTrigger = KeyCode.Joystick2Button7;
                    rightAxisTrigger = "";
                    rjoystickHorizontal = "G1RHorizontal";
                    rjoystickVertical = "G1RVertical";
                }
                ljoystickHorizontal = "1Horizontal";
                ljoystickVertical = "1Vertical";
                leftBumper = KeyCode.Joystick2Button4;
                rightBumper = KeyCode.Joystick2Button5;
                break;
            case 2:
                controllerName = _controllerName;
                if (controllerName.Length == 33 || controllerName.Length == 32 || controllerName.Length == 23)
                {
                    aButton = KeyCode.Joystick3Button0;
                    bButton = KeyCode.Joystick3Button1;
                    startButton = KeyCode.Joystick3Button7;
                    rightAxisTrigger = "2RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "2RHorizontal";
                    rjoystickVertical = "2RVertical";
                }
                else if (controllerName.Length == 19)
                {
                    aButton = KeyCode.Joystick3Button1;
                    bButton = KeyCode.Joystick3Button2;
                    startButton = KeyCode.Joystick3Button9;
                    rightAxisTrigger = "P2RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "P2RHorizontal";
                    rjoystickVertical = "P2RVertical";
                }
                else if (controllerName.Length == 22 || controllerName.Length == 25)
                {
                    aButton = KeyCode.Joystick3Button2;
                    bButton = KeyCode.Joystick3Button1;
                    startButton = KeyCode.Joystick3Button9;
                    rightButtonTrigger = KeyCode.Joystick3Button7;
                    rightAxisTrigger = "";
                    rjoystickHorizontal = "G2RHorizontal";
                    rjoystickVertical = "G2RVertical";
                }
                ljoystickHorizontal = "2Horizontal";
                ljoystickVertical = "2Vertical";
                leftBumper = KeyCode.Joystick3Button4;
                rightBumper = KeyCode.Joystick3Button5;
                break;
            case 3:
                controllerName = _controllerName;
                if (controllerName.Length == 33 || controllerName.Length == 32 || controllerName.Length == 23)
                {
                    aButton = KeyCode.Joystick4Button0;
                    bButton = KeyCode.Joystick4Button1;
                    startButton = KeyCode.Joystick4Button7;
                    rightAxisTrigger = "3RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "3RHorizontal";
                    rjoystickVertical = "3RVertical";
                }
                else if (controllerName.Length == 19)
                {
                    aButton = KeyCode.Joystick4Button1;
                    bButton = KeyCode.Joystick4Button2;
                    startButton = KeyCode.Joystick4Button9;
                    rightAxisTrigger = "P3RightTrigger";
                    rightButtonTrigger = KeyCode.None;
                    rjoystickHorizontal = "P3RHorizontal";
                    rjoystickVertical = "P3RVertical";
                }
                else if (controllerName.Length == 22 || controllerName.Length == 25)
                {
                    aButton = KeyCode.Joystick4Button2;
                    bButton = KeyCode.Joystick4Button1;
                    startButton = KeyCode.Joystick4Button9;
                    rightButtonTrigger = KeyCode.Joystick4Button7;
                    rightAxisTrigger = "";
                    rjoystickHorizontal = "G3RHorizontal";
                    rjoystickVertical = "G3RVertical";
                }
                ljoystickHorizontal = "3Horizontal";
                ljoystickVertical = "3Vertical";
                leftBumper = KeyCode.Joystick4Button4;
                rightBumper = KeyCode.Joystick4Button5;
                break;
        }
    }
}

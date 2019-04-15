using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    string prefabName;
    private bool ready = false;

    public bool Ready { get => ready; }
    public Controls Controls { get => controls; set => controls = value; }
    public GameObject Avatar { get => avatar; set => avatar = value; }
    public byte Id { get => id; set => id = value; }
    public string PrefabName { get => prefabName; set => prefabName = value; }

    public delegate void ReadyDelegate(byte id);
    public static ReadyDelegate OnReady;
    public static ReadyDelegate OnNotReady;

    public delegate void ButtonsDelegate(byte id);
    public static ButtonsDelegate OnBackButton;
    public static ButtonsDelegate OnAcceptButton;
    public static ButtonsDelegate OnStartButton;
    public static ButtonsDelegate OnLeftTrigger;
    public static ButtonsDelegate OnRightTrigger;

    private Controls controls;

    [SerializeField]
    private GameObject avatar;

    private void Awake()
    {
        OnBackButton = null;
        OnAcceptButton = null;
        OnStartButton = null;
        OnLeftTrigger = null;
        OnRightTrigger = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(controls.startButton) || Input.GetKeyDown(controls.aButton))
        {
            OnStartButton?.Invoke(Id);

            if (MenuUI.isMatchmaking)
            {
                if (!ready)
                {
                    ready = true;
                    OnReady?.Invoke(Id);//MenuManager hears it
                }
            }
        }

        if (Input.GetKeyDown(controls.bButton) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Back");
            Debug.Log(controls.bButton);

            if (MenuUI.isMatchmaking)
            {
                if (ready)
                {
                    ready = false;
                    OnNotReady?.Invoke(id);//MenuManager hears it
                }
            }
            OnBackButton?.Invoke(Id);//MenuUI hears it
        }

        if (controls.rightButtonTrigger != KeyCode.None)
        {
            if (Input.GetKeyDown(controls.rightButtonTrigger))
            {
                OnRightTrigger?.Invoke(Id);
            }
        }
        else if (controls.rightAxisTrigger != "")
        {
            if (Input.GetAxis(controls.rightAxisTrigger) == 1f)
            {
                OnRightTrigger?.Invoke(Id);
            }
        }

        if (controls.leftButtonTrigger != KeyCode.None)
        {
            if (Input.GetKeyDown(controls.leftButtonTrigger))
            {
                OnLeftTrigger?.Invoke(Id);
            }
        }
        else if (controls.leftAxisTrigger != "")
        {
            if (Input.GetAxis(controls.leftAxisTrigger) == 1f)
            {
                OnLeftTrigger?.Invoke(Id);
            }
        }

        //if (Input.GetKeyDown(controls.aButton))
        //{
        //    Debug.Log("A");
        //    OnAcceptButton?.Invoke();
        //}


    }
}

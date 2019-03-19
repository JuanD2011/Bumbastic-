using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    private bool ready = false;

    public bool Ready { get => ready; }
    public Controls Controls { get => controls; set => controls = value; }
    public GameObject Avatar { get => avatar; }
    public byte Id { get => id; set => id = value; }

    public delegate void ReadyDelegate(byte id);
    public static ReadyDelegate OnReady;
    public static ReadyDelegate OnNotReady;

    public delegate void ButtonsDelegate();
    public static ButtonsDelegate OnBackButton;
    public static ButtonsDelegate OnAcceptButton;
    public static ButtonsDelegate OnStartButton;

    private Controls controls;

    [SerializeField]
    private GameObject avatar;

    private void Update()
    {
        if (controls.aButton != null)
        {
            if (Input.GetKeyDown(controls.startButton))
            {
                Debug.Log("Start");
                Debug.Log(controls.startButton);
                OnStartButton?.Invoke();
                if (!ready)
                {
                    ready = true;
                    OnReady?.Invoke(id);
                }
                else if (ready)
                {
                    ready = false;
                    OnNotReady?.Invoke(id);
                }
            }

            if (Input.GetKeyDown(controls.aButton))
            {
                Debug.Log("A");
                OnAcceptButton?.Invoke();
            }

            if (Input.GetKeyDown(controls.bButton))
            {
                Debug.Log("Back");
                Debug.Log(controls.bButton);
                OnBackButton?.Invoke();
            } 
        }
    }
}

using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    private bool ready = false;

    public bool Ready { get => ready; }
    public byte Id { get => id; }
    public Controls Controls { get => controls; set => controls = value; }
    public GameObject Avatar { get => avatar; }

    public delegate void ReadyDelegate(byte id);
    public ReadyDelegate OnReady;
    public ReadyDelegate OnNotReady;

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
            if (Input.GetButtonDown(controls.startButton))
            {
                Debug.Log("Start");
                OnStartButton?.Invoke();
            }

            if (Input.GetButtonDown(controls.aButton))
            {
                Debug.Log("A");
                OnAcceptButton?.Invoke();
            }

            if (Input.GetButtonDown(controls.bButton))
            {
                Debug.Log("Back");
                OnBackButton?.Invoke();
            } 
        }
    }
}

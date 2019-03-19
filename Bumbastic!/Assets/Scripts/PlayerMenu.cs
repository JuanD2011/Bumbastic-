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

    private Controls controls;

    [SerializeField]
    private GameObject avatar;

    private void Update()
    {
        if (Input.GetButtonDown(controls.startButton))
        {

        }

        if (Input.GetButtonDown(controls.aButton))
        {

        }

        if (Input.GetButtonDown(controls.bButton))
        {

        }
    }
}

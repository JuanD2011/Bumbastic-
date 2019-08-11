using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.PlayerInput;

public class PlayerMenu : MonoBehaviour
{
    public bool Ready { get; private set; } = false;
    public GameObject Avatar { get; set; } = null;
    public byte Id { get; set; } = 0;
    public string PrefabName { get; set; } = "";
    public Sprite SkinSprite { get; set; } = null;
    public Color Color { get; set; } = Color.white;
    public InputDevice InputDevice { get; private set; } = null;

    public static event System.Action<byte> OnReady, OnNotReady;

    public static System.Action<byte> OnBackButton, OnAcceptButton, OnStartButton, OnLeftBumper, OnRightBumper;
    public static event System.Action<float> OnScrollingAxis;

    [SerializeField]
    private GameObject avatar;

    private void Awake()
    {
        InputDevice = GetComponent<PlayerInput>().devices[0];
    }

    private void Start()
    {
        MenuCanvas.OnMatchmaking += OnResetStatus;
    }

    private void OnResetStatus(bool _isMatchmaking)
    {
        if (!_isMatchmaking)
        {
            Ready = false;
            OnNotReady?.Invoke(Id);
        }
    }

    public static void ResetDel()
    {
        OnBackButton = null;
        OnStartButton = null;
        OnAcceptButton = null;
        OnLeftBumper = null;
        OnRightBumper = null;
        OnReady = null;
        OnNotReady = null;
        OnScrollingAxis = null;
    }

    public void OnSubmit()
    {
        if (MenuCanvas.isMatchmaking)
        {
            if (!Ready)
            {
                Ready = true;
                OnReady?.Invoke(Id);
            }
        }
        OnAcceptButton?.Invoke(Id);
    }

    public void OnScrolling(InputValue context)
    {
        OnScrollingAxis?.Invoke(context.Get<Vector2>().normalized.y);
    }

    public void OnStart()
    {
        if (MenuCanvas.isMatchmaking)
        {
            if (!Ready)
            {
                Ready = true;
                OnReady?.Invoke(Id);
            }
        }
        OnStartButton?.Invoke(Id);
    }

    public void OnBack()
    {
        if (!Ready)
        {
            OnBackButton?.Invoke(Id);
        }
        if (MenuCanvas.isMatchmaking)
        {
            if (Ready)
            {
                Ready = false;
                OnNotReady?.Invoke(Id);
            }
        }
    }

    public void OnLeftSkin()
    {
        if (!Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            OnLeftBumper?.Invoke(Id);
        }
    }

    public void OnRightSkin()
    {
        if (!Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            OnRightBumper?.Invoke(Id);
        }
    }
}

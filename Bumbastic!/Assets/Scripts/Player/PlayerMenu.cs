using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Plugins.PlayerInput;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    string prefabName;
    private bool ready = false;
    Sprite skinSprite;
    Color color;

    public bool Ready { get => ready; }
    public GameObject Avatar { get => avatar; set => avatar = value; }
    public byte Id { get => id; set => id = value; }
    public string PrefabName { get => prefabName; set => prefabName = value; }
    public Sprite SkinSprite { get => skinSprite; set => skinSprite = value; }
    public Color Color { get => color; set => color = value; }

    public delegate void ReadyDelegate(byte id);
    public static ReadyDelegate OnReady;
    public static ReadyDelegate OnNotReady;

    public delegate void ButtonsDelegate(byte id);
    public static ButtonsDelegate OnBackButton;
    public static ButtonsDelegate OnAcceptButton;
    public static ButtonsDelegate OnStartButton;
    public static ButtonsDelegate OnLeftBumper;
    public static ButtonsDelegate OnRightBumper;

    public static System.Action<float> OnScrollingAxis;

    [SerializeField]
    private GameObject avatar;

    private void Start()
    {
        MenuCanvas.OnMatchmaking += OnResetStatus;
    }

    private void OnResetStatus(bool _canActive)
    {
        if (!_canActive)
        {
            if (ready)
            {
                ready = false;
                OnNotReady?.Invoke(Id); 
            }
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
            if (!ready)
            {
                ready = true;
                OnReady?.Invoke(Id);//MenuManager hears it
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
            if (!ready)
            {
                ready = true;
                OnReady?.Invoke(Id);//MenuManager hears it
            }
        }
        OnStartButton?.Invoke(Id);
    }

    public void OnBack()
    {
        if (!ready)
        {
            OnBackButton?.Invoke(Id);//MenuUI hears it 
        }
        if (MenuCanvas.isMatchmaking)
        {
            if (ready)
            {
                ready = false;
                OnNotReady?.Invoke(id);//MenuManager hears it
            }
        }
    }

    public void OnLeftSkin()
    {
        if (!Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            OnLeftBumper?.Invoke(Id);//SkinSelector hears it.  
        }
    }

    public void OnRightSkin()
    {
        if (!Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            OnRightBumper?.Invoke(Id);//SkinSelector hears it.  
        }
    }
}

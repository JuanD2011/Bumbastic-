using UnityEngine;
using XInputDotNetPure;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    string prefabName;
    private bool ready = false;
    Sprite skinSprite;
    Color color;
    private PlayerIndex playerIndex;
    private GamePadState state;
    private GamePadState prevState;

    public bool Ready { get => ready; }
    public Controls Controls { get => controls; set => controls = value; }
    public GameObject Avatar { get => avatar; set => avatar = value; }
    public byte Id { get => id; set => id = value; }
    public string PrefabName { get => prefabName; set => prefabName = value; }
    public Sprite SkinSprite { get => skinSprite; set => skinSprite = value; }
    public Color Color { get => color; set => color = value; }
    public PlayerIndex PlayerIndex { private get => playerIndex; set => playerIndex = value; }

    public delegate void ReadyDelegate(byte id);
    public static ReadyDelegate OnReady;
    public static ReadyDelegate OnNotReady;

    public delegate void ButtonsDelegate(byte id);
    public static ButtonsDelegate OnBackButton;
    public static ButtonsDelegate OnAcceptButton;
    public static ButtonsDelegate OnStartButton;
    public static ButtonsDelegate OnLeftBumper;
    public static ButtonsDelegate OnRightBumper;

    private Controls controls;

    [SerializeField]
    private GameObject avatar;

    public static byte counter = 0;

    private void Awake()
    {
        ResetDel();
    }

    public static void ResetDel()
    {
        OnBackButton = null;
        OnAcceptButton = null;
        OnStartButton = null;
        OnLeftBumper = null;
        OnRightBumper = null;
    }

    private void Update()
    {
        prevState = state;
        state = GamePad.GetState(PlayerIndex);

        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed || prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            Debug.Log("Start/A");
            if (MenuCanvas.isMatchmaking)
            {
                counter++;
                if (!ready && counter >= 2)
                {
                    ready = true;
                    OnReady?.Invoke(Id);//MenuManager hears it
                }
            }
            OnStartButton?.Invoke(Id);
        }

        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Back");
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

        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            if (!(prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed))
                OnRightBumper?.Invoke(Id);//SkinSelector hears it.
        }
        else if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            if (!(prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed))
                OnLeftBumper?.Invoke(Id);//SkinSelector hears it.
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] SkinsDatabase skinsData = null;
    [SerializeField] int player = 0;
    int position = 0;

    Button[] buttons;
    Image[] images;

    public delegate void DelSkinSelector(int _player, int _position);
    public DelSkinSelector OnChangeSkin;

    public Button[] Buttons { get => buttons; set => buttons = value; }
    public Image[] Images { get => images; set => images = value; }
    public int Position { get => position; protected set => position = value; }

    private void Start()
    {
        PlayerMenu.OnLeftBumper += PreviousSkin;
        PlayerMenu.OnRightBumper += NextSkin;  
    }

    public void InitSkinSelector()
    {
        Position = player - 1;
        Buttons = GetComponentsInChildren<Button>(true);
        Images = GetComponentsInChildren<Image>(true);

        Buttons[0].onClick.AddListener(() => PreviousSkin());
        Buttons[1].onClick.AddListener(() => NextSkin());
    }

    #region With Triggers
    public void PreviousSkin(byte _playerId)
    {
        if (MenuCanvas.isMatchmaking)
        {
            if (!MenuManager.menu.Players[_playerId].Ready)
            {
                if (player - 1 == _playerId)
                {
                    skinsData.skins[Position].choosed = false;
                    int currentSkin = GetAvailableSkin(false);
                    OnChangeSkin?.Invoke(_playerId, currentSkin);//Skin manager
                } 
            } 
        }
    }

    public void NextSkin(byte _playerId)
    {
        if (MenuCanvas.isMatchmaking)
        {
            if (!MenuManager.menu.Players[_playerId].Ready)
            {
                if (player - 1 == _playerId)
                {
                    skinsData.skins[Position].choosed = false;
                    int currentSkin = GetAvailableSkin(true);
                    OnChangeSkin?.Invoke(_playerId, currentSkin);//Skin manager 
                }  
            }
        }
    }
    #endregion

    #region With Buttons
    public void NextSkin()
    {
        skinsData.skins[Position].choosed = false;
        int currentSkin = GetAvailableSkin(true);
        OnChangeSkin?.Invoke(player - 1, currentSkin);//Skin manager
    }

    public void PreviousSkin()
    {
        skinsData.skins[Position].choosed = false; 
        int currentSkin = GetAvailableSkin(false);
        OnChangeSkin?.Invoke(player - 1, currentSkin);//Skin manager
    }
    #endregion

    private int GetAvailableSkin(bool _Forward)
    {
        if (_Forward)
        {
            for (int i = Position + 1; i <= skinsData.skins.Count; i++)
            {
                if (i == skinsData.skins.Count)
                {
                    i = 0;
                }
                if (!skinsData.skins[i].choosed)
                {
                    return Position = i;
                }
            }
        }
        else
        {
            for (int i = Position - 1; i >= -1; i--)
            {
                if (i <= -1)
                {
                    i = skinsData.skins.Count - 1;
                }
                if (!skinsData.skins[i].choosed)
                {
                    return Position = i;
                }
            }
        }
        return Position;
    }
}

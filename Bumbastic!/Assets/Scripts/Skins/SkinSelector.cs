using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] SkinsDatabase skinsData = null;
    [SerializeField] int player = 0;
    int position = 0;

    private int iD = 0;

    public static System.Action<int, int> OnChangeSkin;

    private void Awake()
    {
        iD = player - 1;
    }

    private void Start()
    {
        PlayerMenu.OnLeftBumper += PreviousSkin;
        PlayerMenu.OnRightBumper += NextSkin;

        SkinManager.OnUpdateSkinSelectorsPosition += SetPosition;
        position = iD;
    }

    public void SetPosition(int _skinPosition, byte _playerID)
    {
        if (iD != _playerID) return;

        position = _skinPosition;
    }

    public void PreviousSkin(byte _playerId)
    {
        if (!MenuCanvas.isMatchmaking || iD != _playerId) return;

        if (!MenuManager.menu.Players[_playerId].Ready)
        {
            skinsData.skins[position].choosed = false;
            int currentSkin = GetAvailableSkin(false);
            OnChangeSkin?.Invoke(_playerId, currentSkin);
        } 
    }

    public void NextSkin(byte _playerId)
    {
        if (!MenuCanvas.isMatchmaking || iD != _playerId) return;

        if (!MenuManager.menu.Players[_playerId].Ready)
        {
            skinsData.skins[position].choosed = false;
            int currentSkin = GetAvailableSkin(true);
            OnChangeSkin?.Invoke(_playerId, currentSkin);
        }
    }

    private int GetAvailableSkin(bool _Forward)
    {
        if (_Forward)
        {
            for (int i = position + 1; i <= skinsData.skins.Count; i++)
            {
                if (i == skinsData.skins.Count)
                {
                    i = 0;
                }
                if (!skinsData.skins[i].choosed)
                {
                    return position = i;
                }
            }
        }
        else
        {
            for (int i = position - 1; i >= -1; i--)
            {
                if (i <= -1)
                {
                    i = skinsData.skins.Count - 1;
                }
                if (!skinsData.skins[i].choosed)
                {
                    return position = i;
                }
            }
        }
        return position;
    }
}

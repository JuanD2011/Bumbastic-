using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] Skins skinsData;
    [SerializeField] int player = 0;
    int position = 0;

    Button[] buttons;

    public delegate void DelSkinSelector(int _player, int _position);
    public static DelSkinSelector OnChangeSkin;

    private void Awake()
    {
        OnChangeSkin = null;
    }

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => PreviousSkin());
        buttons[1].onClick.AddListener(() => NextSkin());
        position = player - 1;
    }

    public void NextSkin()
    {
        Debug.Log("Next");
        //position = (position + 1) % skinsData.skins.Count;
        skinsData.skins[position].choosed = false;
        int currentSkin = GetAvailableSkin(true);
        OnChangeSkin?.Invoke(player - 1, currentSkin);//Skin manager
    }


    public void PreviousSkin()
    {
        Debug.Log("Previous");
        skinsData.skins[position].choosed = false; 
        int currentSkin = GetAvailableSkin(false);
        OnChangeSkin?.Invoke(player - 1, currentSkin);//Skin manager
    }

    private int GetAvailableSkin(bool _Forward)
    {
        if (_Forward)
        {
            for (int i = position; i < skinsData.skins.Count; i++)
            {
                if (!skinsData.skins[i].choosed)
                {
                    return position = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = position; i >= 0; i--)
            {
                if (!skinsData.skins[i].choosed)
                {
                    return position = i;
                    break;
                }
            }
        }
        return position;
    }
}

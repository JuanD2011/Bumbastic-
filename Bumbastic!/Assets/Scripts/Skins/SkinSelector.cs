﻿using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] Skins skinsData;
    [SerializeField] int player = 0;
    int position = 0;

    Button[] buttons;
    Image[] images;

    public delegate void DelSkinSelector(int _player, int _position);
    public DelSkinSelector OnChangeSkin;

    public Button[] Buttons { get => buttons; set => buttons = value; }
    public Image[] Images { get => images; set => images = value; }

    public void InitSkinSelector()
    {
        position = player - 1;
        Buttons = GetComponentsInChildren<Button>(true);
        Images = GetComponentsInChildren<Image>(true);

        Buttons[0].onClick.AddListener(() => PreviousSkin());
        Buttons[1].onClick.AddListener(() => NextSkin());
    }

    public void NextSkin()
    {
        Debug.Log("Next");
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
            for (int i = position + 1; i < skinsData.skins.Count; i++)
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
            for (int i = position - 1; i > -1; i--)
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

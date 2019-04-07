using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] Skins skinsData;
    [SerializeField] int player = 0;
    int position = 0;

    Button[] buttonSelection;

    public int Player { get => player; private set => player = value; }

    private void Awake()
    {
        buttonSelection = GetComponentsInChildren<Button>();
        buttonSelection[0].onClick.AddListener(() => PreviousSkin());
        buttonSelection[1].onClick.AddListener(() => NextSkin());
    }

    public void NextSkin()
    {
        Debug.Log("Next");
        position = (position + 1) % skinsData.skins.Count;
    }

    public void PreviousSkin()
    {
        Debug.Log("Previous");
        position = (position - 1) % skinsData.skins.Count;
    }
}

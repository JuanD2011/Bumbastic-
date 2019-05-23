﻿using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    string prefabName;
    private bool ready = false;
    Sprite skinSprite;
    Color color;

    public bool Ready { get => ready; }
    public Controls Controls { get => controls; set => controls = value; }
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

    private Controls controls;

    [SerializeField]
    private GameObject avatar;

    private void OnDisable()
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

    public void OnSubmit()
    {
        Debug.Log("Submit");
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

    public void OnStart()
    {
        Debug.Log("Start");
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

    public void OnLeftSkin()
    {
        Debug.Log("LB");
        OnLeftBumper?.Invoke(Id);//SkinSelector hears it. 
    }

    public void OnRightSkin()
    {
        Debug.Log("RB");
        OnRightBumper?.Invoke(Id);//SkinSelector hears it. 
    }
}

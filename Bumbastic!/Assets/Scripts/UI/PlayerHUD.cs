﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] byte iD = 0;
    [SerializeField] Image m_image = null;
    [SerializeField] TextMeshProUGUI m_ReadyText = null;
    [SerializeField] TextMeshProUGUI m_SkinName = null;
    [SerializeField] TextMeshProUGUI m_PlayerNumber = null;

    [SerializeField] Settings settings = null;

    private void Start()
    {
        SkinManager.OnSkinChanged += UpdateMyInfo;

        PlayerInputHandler.OnPlayerDeviceRegained += UpdateMyInfo;

        PlayerMenu.OnReady += SetReadyText;
        PlayerMenu.OnNotReady += SetReadyText;

        MenuManager.menu.Players[iD].Color = settings.playersColor[iD];
        m_image.color = settings.playersColor[iD];

        MenuCanvas.OnMatchmaking += MenuCanvas_OnMatchmaking;

        SpawnLine.OnActivePlayersSorted += UpdateMyNumberOfPlayer;

        SetReadyText(iD);
        UpdateMyInfo(iD);
    }

    private void MenuCanvas_OnMatchmaking(bool _isMatchmaking)
    {
        if (!_isMatchmaking) return;

        SetReadyText(iD);
    }

    private void UpdateMyInfo(int _playerID)
    {
        if (_playerID != iD) return;
        m_SkinName.SetText(MenuManager.menu.Players[iD].PrefabName);
    }

    private void UpdateMyInfo(byte _playerID)
    {
        if (_playerID != iD) return;
        m_SkinName.SetText(MenuManager.menu.Players[iD].PrefabName);
    }

    private void UpdateMyNumberOfPlayer(List<PlayerMenu> _activePlayers)
    {
        for (int i = 0; i < _activePlayers.Count; i++)
        {
            if (_activePlayers[i].Id == iD)
            {
                m_PlayerNumber.text = Translation.Fields[string.Format("P{0}", i + 1)];
            }
        }
    }

    private void SetReadyText(byte _id)
    {
        if (_id != iD) return;

        m_ReadyText.text = (MenuManager.menu.Players[_id].Ready) ? Translation.Fields["Ready"] : Translation.Fields["Start"];
    }
}

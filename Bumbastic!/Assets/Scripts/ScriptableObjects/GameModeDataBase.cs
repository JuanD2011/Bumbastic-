﻿using UnityEngine;

[CreateAssetMenu(fileName = "Game modes data", menuName = "Game modes data")]
public class GameModeDataBase : ScriptableObject
{
    public GameMode[] gameModes = new GameMode[1];
    public static GameMode currentGameMode;

    public static bool IsCurrentFreeForAll()
    {
        if (currentGameMode.gameModeType == GameModeType.FreeForAll)
        {
            return true;
        }
        return false;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "Game modes data", menuName = "Game modes data")]
public class GameModeDataBase : ScriptableObject
{
    public GameMode[] gameModes = new GameMode[1];
    public static GameMode currentGameMode;

    public static bool IsCurrentFreeForAll()
    {
        if (currentGameMode.GameModeType == GameModeType.FreeForAll)
        {
            return true;
        }
        return false;
    }
    public static bool IsCurrentHotPotato()
    {
        if (currentGameMode.GameModeType == GameModeType.HotPotato)
        {
            return true;
        }
        return false;
    }
    public static bool IsCurrentBasesGame()
    {
        if (currentGameMode.GameModeType == GameModeType.BasesGame)
        {
            return true;
        }
        return false;
    }
}
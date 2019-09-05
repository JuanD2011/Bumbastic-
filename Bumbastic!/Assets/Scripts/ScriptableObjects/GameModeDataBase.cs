using UnityEngine;

[CreateAssetMenu(fileName = "Game modes data", menuName = "Game modes data")]
public class GameModeDataBase : ScriptableObject
{
    public GameMode[] gameModes = new GameMode[1];
    public static GameMode currentGameMode = null;

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
    public static bool IsCurrentExplosiveRain()
    {
        if (currentGameMode.GameModeType == GameModeType.ExplosiveRain)
        {
            return true;
        }
        return false;
    }

    public void GetNextGameMode()
    {
        int random = Random.Range(0, gameModes.Length);

        if (currentGameMode == null)
        {
            currentGameMode = gameModes[random];
            return;
        }

        if (gameModes.Length > 1)
        {
            do
            {
                random = Random.Range(0, gameModes.Length);
            }
            while (currentGameMode.GameModeType == gameModes[random].GameModeType);

            currentGameMode = gameModes[random];
        }
        else
        {
            currentGameMode = gameModes[random];
        }
    }
}
[System.Serializable]
public class GameMode
{
    public string name;
    public string description;
    public GameModeType gameModeType;

    public GameMode(string _name, string _description, GameModeType _gameModeType)
    {
        name = _name;
        description = _description;
        gameModeType = _gameModeType;
    }
}

public enum GameModeType
{
    HotPotato,
    Deathmatch
};
using UnityEngine;

[System.Serializable]
public class GameMode
{
    public string name;
    public string description;
    public GameModeType gameModeType;
    public Sprite[] gameModeImages;

    public GameMode(string _name, string _description, GameModeType _gameModeType, Sprite[] _gameModeImages)
    {
        name = _name;
        description = _description;
        gameModeType = _gameModeType;
        gameModeImages = _gameModeImages;
    }
}

public enum GameModeType
{
    HotPotato,
    Deathmatch
};
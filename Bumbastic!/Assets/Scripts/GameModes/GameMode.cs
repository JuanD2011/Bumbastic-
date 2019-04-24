using UnityEngine;

[System.Serializable]
public class GameMode
{
    public string name;
    public string description;
    public GameModeType gameModeType;
    public Sprite[] gameModeBackgrounds;

    public GameMode(string _name, string _description, GameModeType _gameModeType, Sprite[] _gameModeBackgrounds)
    {
        name = _name;
        description = _description;
        gameModeType = _gameModeType;
        gameModeBackgrounds = _gameModeBackgrounds;
    }
}

public enum GameModeType
{
    HotPotato,
    FreeForAll,
    BasesGame
};
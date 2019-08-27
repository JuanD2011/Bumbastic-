using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class GameMode
{
    [SerializeField] string name = "";
    [SerializeField] string description = "";
    [SerializeField] GameModeType gameModeType = GameModeType.HotPotato;
    [SerializeField] Sprite[] gameModeBackgrounds = new Sprite[1];
    [SerializeField] VideoClip videoClip = null;

    public string Name { get => name; private set => name = value; }
    public string Description { get => description; private set => description = value; }
    public GameModeType GameModeType { get => gameModeType; private set => gameModeType = value; }
    public Sprite[] GameModeBackgrounds { get => gameModeBackgrounds; private set => gameModeBackgrounds = value; }
    public VideoClip VideoClip { get => videoClip; private set => videoClip = value; }

    public GameMode(string _name, string _description, GameModeType _gameModeType, Sprite[] _gameModeBackgrounds, VideoClip _videoClip)
    {
        Name = _name;
        Description = _description;
        GameModeType = _gameModeType;
        GameModeBackgrounds = _gameModeBackgrounds;
        VideoClip = _videoClip;
    }
}

public enum GameModeType
{
    HotPotato,
    FreeForAll,
    BasesGame,
    ExplosiveRain
};
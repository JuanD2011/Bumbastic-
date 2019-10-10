using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class GameMode
{
    [SerializeField] GameModeType gameModeType = GameModeType.HotPotato;
    [SerializeField] Sprite[] gameModeBackgrounds = new Sprite[1];
    [SerializeField] VideoClip videoClip = null;

    public GameModeType GameModeType { get => gameModeType; private set => gameModeType = value; }
    public Sprite[] GameModeBackgrounds { get => gameModeBackgrounds; private set => gameModeBackgrounds = value; }
    public VideoClip VideoClip { get => videoClip; private set => videoClip = value; }
}

public enum GameModeType
{
    HotPotato,
    FreeForAll,
    BasesGame,
    ExplosiveRain
};
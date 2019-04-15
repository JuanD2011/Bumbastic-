using UnityEngine;

[CreateAssetMenu(fileName = "Game modes data", menuName = "Game modes data")]
public class GameModes : ScriptableObject
{
    public GameMode[] gameModes = new GameMode[1];
}
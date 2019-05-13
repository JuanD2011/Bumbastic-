using UnityEngine;
using XInputDotNetPure;

public class PlayerSettings
{
    public string name;
    public GameObject avatar;
    public Controls controls;
    public Sprite skinSprite;
    public byte score;
    public Color color;
    public PlayerIndex playerIndex;

    public PlayerSettings(string _name, GameObject _avatar, Controls _controls, Sprite _skinSprite, Color _color, PlayerIndex _index)
    {
        name = _name;
        avatar = _avatar;
        controls = _controls;
        skinSprite = _skinSprite;
        score = 0;
        color = _color;
        playerIndex = _index;
    }
}

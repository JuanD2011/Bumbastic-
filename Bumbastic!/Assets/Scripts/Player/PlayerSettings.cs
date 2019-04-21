using UnityEngine;

public class PlayerSettings
{
    public string name;
    public GameObject avatar;
    public Controls controls;
    public Sprite skinSprite;
    public byte score;
    public Color color;                     

    public PlayerSettings(string _name, GameObject _avatar, Controls _controls, Sprite _skinSprite, Color _color)
    {
        name = _name;
        avatar = _avatar;
        controls = _controls;
        skinSprite = _skinSprite;
        score = 0;
        color = _color;
    }
}

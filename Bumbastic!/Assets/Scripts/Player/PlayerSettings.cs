using UnityEngine;

public struct PlayerSettings
{
    public string name;
    public GameObject avatar;
    public Controls controls;
    public Sprite skinSprite;

    public PlayerSettings(string _name, GameObject _avatar, Controls _controls, Sprite _skinSprite) : this()
    {
        name = _name;
        avatar = _avatar;
        controls = _controls;
        skinSprite = _skinSprite;
    }
}

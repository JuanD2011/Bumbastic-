using UnityEngine;

public struct PlayerSettings
{
    public string name;
    public GameObject avatar;
    public Controls controls;

    public PlayerSettings(string _name, GameObject _avatar, Controls _controls) : this()
    {
        name = _name;
        avatar = _avatar;
        controls = _controls;
    }
}

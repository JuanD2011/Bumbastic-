using UnityEngine;

public class PlayerSettings
{
    public int id;
    public string name;
    public GameObject avatar;
    public Sprite skinSprite;
    public byte score;
    public Color color;                     

    public PlayerSettings(int _id, string _name, GameObject _avatar, Sprite _skinSprite, Color _color)
    {
        id = _id;
        name = _name;
        avatar = _avatar;
        skinSprite = _skinSprite;
        score = 0;
        color = _color;
    }
}

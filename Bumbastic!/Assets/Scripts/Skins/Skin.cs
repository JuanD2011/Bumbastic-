using UnityEngine;

[System.Serializable]
public class Skin
{
    public string name;
    public string description;
    public int value;
    public GameObject prefab;
    public bool choosed;
    public Sprite skinSprite;

    public Skin(string _name, int _value, GameObject _prefab, bool _choosed, Sprite _skinSprite)
    {
        name = _name;
        value = _value;
        prefab = _prefab;
        choosed = _choosed;
        skinSprite = _skinSprite;
    }

    public Skin(string _name, string _description, int _value, GameObject _prefab, bool _choosed, Sprite _skinSprite)
    {
        name = _name;
        description = _description;
        value = _value;
        prefab = _prefab;
        choosed = _choosed;
        skinSprite = _skinSprite;
    }
}

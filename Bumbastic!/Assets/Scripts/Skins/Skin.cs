using System;
using UnityEngine;

[Serializable]
public class Skin
{
    public string name;
    public string description;
    public int value;
    public GameObject prefab;
    public bool choosed;

    public Skin(string _name, int _value, GameObject _prefab, bool _choosed)
    {
        name = _name;
        value = _value;
        prefab = _prefab;
        choosed = _choosed;
    }

    public Skin(string _name, string _description, int _value, GameObject _prefab, bool _choosed)
    {
        name = _name;
        description = _description;
        value = _value;
        prefab = _prefab;
        choosed = _choosed;
    }
}

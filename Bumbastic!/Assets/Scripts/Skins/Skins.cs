using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skins data", menuName = "Skins data")]
public class Skins : ScriptableObject
{
    public List<Skin> skins = new List<Skin>();
}

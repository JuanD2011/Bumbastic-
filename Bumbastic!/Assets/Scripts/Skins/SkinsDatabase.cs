using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skins data", menuName = "Skins data")]
public class SkinsDatabase : ScriptableObject
{
    public List<Skin> skins = new List<Skin>();
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InGame",menuName = "InGame")]
public class InGame : ScriptableObject
{
    public Transform posDropPU;

    public int maxPlayers;
    public List<PlayerSettings> playerSettings = new List<PlayerSettings>();
    public static List<Player> players = new List<Player>();

    Vector3 crowPos;
    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }
}

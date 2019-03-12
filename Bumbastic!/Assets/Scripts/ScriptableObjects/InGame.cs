using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

[CreateAssetMenu(fileName = "InGame",menuName = "InGame")]
public class InGame : ScriptableObject
{
    public int playersJoined = 1;
    public Transform posDropPU;
    public int maxPlayers;

    public List<PlayerInput> players = new List<PlayerInput>();

    Vector3 crowPos;
    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }

    public void ReturnMenu()
    {

    }
}

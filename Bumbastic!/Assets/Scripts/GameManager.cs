using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private List<Player> players;

    public List<Player> Players { get => players; set => players = value; }


    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {

    }
}

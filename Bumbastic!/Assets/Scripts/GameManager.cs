using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private List<Player> players;

    public List<Player> Players { get => players; set => players = value; }

    private void OnEnable()
    {

    }

    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this);
    }

    private void Start()
    {
        
    }


}

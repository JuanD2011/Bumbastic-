using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private List<PlayerInput> playersInput = new List<PlayerInput>();
    private List<Player> players = new List<Player>();

    public List<PlayerInput> Players { get => playersInput; set => playersInput = value; }

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this);
    }

    private void Start()
    {
        playersInput = inGame.players;
        foreach (PlayerInput item in playersInput)
        {
            item.ActivateInput();
            players.Add(item.gameObject.GetComponent<Player>());
        }
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        foreach (Player player in players)
        {
            player.enabled = true;
            player.SpawnPoint = GetSpawnPoint();
            player.transform.position = player.SpawnPoint;
            GameObject avatar = Instantiate(player.Avatar, player.transform.localPosition, player.transform.rotation);
            avatar.transform.SetParent(player.gameObject.transform);
            player.Initialize();
            player.CanMove = true;
        }
    }

    public Vector3 GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPos = spawnPoints[random].position;
        spawnPoints.RemoveAt(random);
        return spawnPos;
    }
}

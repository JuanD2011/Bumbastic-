﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private List<Player> players;
    private List<Player> bummies = new List<Player>();

    public Player BombHolder { get => bombHolder; set => bombHolder = value; }
    public Bomb Bomb { get => bomb; }
    public PlayableDirector Director { get => director; set => director = value; }
    public List<Player> Players { get => players; set => players = value; }

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    private PlayableDirector director;

    [SerializeField]
    private float minTime, maxTime;

    [SerializeField] private GameObject confettiBomb, playerPrefab;
    public GameObject floor;

    private Player bombHolder;

    [SerializeField]
    private Bomb bomb;

    public PowerUp powerUp;
    public GameObject magnetParticleSystem;

    public delegate void GameStateDelegate();
    public event GameStateDelegate OnGameOver;

    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this);

        players = new List<Player>();
        Director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        SpawnPlayers();

        Bomb.OnExplode += StartNewRound;
    }

    private void StartNewRound(Player _player)
    {
        Players.Remove(_player);
        _player.gameObject.SetActive(false);

        foreach (Player player in Players)
        {
            player.transform.position = player.SpawnPoint;
            player.CanMove = false;
        }

        GiveBombs();
    }

    private void SpawnPlayers()
    {
		for (int i = 0; i < inGame.playerSettings.Count; i++)
		{
			Player player = Instantiate(playerPrefab).GetComponent<Player>();
			Players.Add(player);
			player.Controls = inGame.playerSettings[i].controls;
			player.Avatar = inGame.playerSettings[i].avatar;
			player.Id = (byte)i;
			player.SpawnPoint = GetSpawnPoint();
			player.transform.position = player.SpawnPoint;
			player.Initialize();
		}
        GiveBombs();
    }

    public Vector3 GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPos = spawnPoints[random].position;
        spawnPoints.RemoveAt(random);
        return spawnPos;
    }

    public void GiveBombs()
    {
        if (Players.Count > 1)
        {
            bummies = RandomizeBummieList();

            int[] _bummies = new int[bummies.Count];

            Director.Play();

            for (int i = 0; i < bummies.Count; i++)
            {
                Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                bummies.RemoveAt(i);
            }
            bomb.transform.position = bummies[0].transform.position + new Vector3(0, 4, 0);
            bomb.Timer = Random.Range(minTime, maxTime);
            bomb.Exploded = false;
            bomb.gameObject.SetActive(true);
        }
        else if (Players.Count == 1)
        {
            GameOver();
        }
    }

    private List<Player> RandomizeBummieList()
    {
        List<Player> bummies = new List<Player>(Players);
        List<Player> randomBummies = new List<Player>();

        while (bummies.Count > 0)
        {
            int rand = Random.Range(0, bummies.Count);
            randomBummies.Add(bummies[rand]);
            bummies.RemoveAt(rand);
        }

        return randomBummies;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        Debug.Log("Game Over");
    }
}

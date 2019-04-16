using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class GameManager : MonoBehaviour
{
    protected List<Player> players;

    [SerializeField]
    private List<Transform> spawnPoints;

    public PlayableDirector Director { get => director; protected set => director = value; }
    public List<Player> Players { get => players; protected set => players = value; }

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    GameModeDataBase gameMode;

    [SerializeField]
    private GameObject playerPrefab;

    protected PlayableDirector director;

    public PowerUp powerUp;
    public GameObject magnetParticleSystem;
    public GameObject speedUpParticleSystem;

    public delegate void GameStateDelegate();
    public event GameStateDelegate OnGameOver;

    private void Awake()
    {
        players = new List<Player>();
        Director = GetComponent<PlayableDirector>();
        SpawnPlayers();
    }

    protected void SpawnPlayers()
    {
		for (int i = 0; i < inGame.playerSettings.Count; i++)
		{
			Player player = Instantiate(playerPrefab).GetComponent<Player>();
			Players.Add(player);
			player.Controls = inGame.playerSettings[i].controls;
			player.Avatar = inGame.playerSettings[i].avatar;
            player.PrefabName = inGame.playerSettings[i].name;
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
        GetNextGameMode();
        OnGameOver?.Invoke();//InGameCanvas
        Debug.Log("Game Over");
    }

    private void GetNextGameMode()
    {
        int random = Random.Range(0, gameMode.gameModes.Length);

        if (gameMode.gameModes.Length > 1)
        {
            if (GameModeDataBase.currentGameMode.gameModeType != gameMode.gameModes[random].gameModeType)
            {
                GameModeDataBase.currentGameMode = gameMode.gameModes[random];
            }
            else
            {
                GetNextGameMode();
            }
        }
        else
        {
            GameModeDataBase.currentGameMode = gameMode.gameModes[random];
        }
    }

    
}

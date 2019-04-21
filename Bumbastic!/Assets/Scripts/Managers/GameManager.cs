using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    protected List<Player> players;

    [SerializeField]
    private List<Transform> spawnPoints;

    public PlayableDirector Director { get => director; protected set => director = value; }
    public List<Player> Players { get => players; protected set => players = value; }

    [SerializeField]
    GameModeDataBase gameMode;

    [SerializeField]
    private GameObject playerPrefab;

    protected PlayableDirector director;

    public PowerUp powerUp;
    public GameObject magnetParticleSystem;
    public GameObject speedUpParticleSystem;
    public GameObject floor;

    public delegate void GameStateDelegate();
    public event GameStateDelegate OnGameModeOver;
    public event GameStateDelegate OnGameOver;

    protected virtual void Awake()
    {
        if (Manager == null) Manager = this;
        else Destroy(this);

        Players = new List<Player>();
        Director = GetComponent<PlayableDirector>();
        PlayerMenu.ResetDel();
        SpawnPlayers();
    }

    protected void SpawnPlayers()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            Player player = Instantiate(playerPrefab).GetComponent<Player>();
            Players.Add(player);
            player.Controls = InGame.playerSettings[i].controls;
            player.Avatar = InGame.playerSettings[i].avatar;
            player.PrefabName = InGame.playerSettings[i].name;
            player.Id = (byte)i;
            player.SpawnPoint = GetSpawnPoint();
            player.transform.position = player.SpawnPoint;
            player.Initialize();
        }

        GiveBombs();
    }

    protected abstract void GiveBombs();

    public Vector3 GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPos = spawnPoints[random].position;
        spawnPoints.RemoveAt(random);
        return spawnPos;
    }  

    protected List<Player> RandomizeBummieList()
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

    protected void GameOver()
    {
        Debug.Log("Game Over");
        OnGameModeOver?.Invoke();//InGameCanvas

        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            if (InGame.playerSettings[i].score == InGame.maxScore)
            {
                OnGameOver?.Invoke();//InGameCanvas hears it.
                return;
                break;
            }
        }

        GetNextGameMode();
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

    public abstract void PassBomb();

    public abstract void PassBomb(Player _receiver);

    public abstract void PassBomb(Player _receiver, Player _transmitter);
}

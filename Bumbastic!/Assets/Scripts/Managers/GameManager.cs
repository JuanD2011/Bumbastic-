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
    protected List<Transform> SpawnPoints { get => spawnPoints; private set => spawnPoints = value; }
    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }
    public EnumEnviroment Enviroment { get => enviroment; private set => enviroment = value; }

    [SerializeField]
    GameModeDataBase gameMode;

    private EnumEnviroment enviroment = EnumEnviroment.Desert;

    [SerializeField]
    GameObject playerPrefab;

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

        Enviroment = GetRandomEnviroment();

        Players = new List<Player>();
        Director = GetComponent<PlayableDirector>();
        PlayerMenu.ResetDel();
        SpawnPlayers();
    }

    protected virtual void Start()
    {
        InitAudio();
    }

    private void InitAudio()
    {
        switch (Enviroment)
        {
            case EnumEnviroment.Desert:
                AudioManager.instance.PlayAmbient(AudioManager.instance.audioClips.desertAmbient, 0.3f, 0.6f, 1f);
                break;
            case EnumEnviroment.Winter:
                AudioManager.instance.PlayAmbient(AudioManager.instance.audioClips.winterAmbient, 0.3f, 0.6f, 1f);
                break;
            default:
                AudioManager.instance.PlayAmbient(AudioManager.instance.audioClips.desertAmbient, 0.3f, 0.6f, 1f);
                break;
        }
    }

    private EnumEnviroment GetRandomEnviroment()
    {
        EnumEnviroment result = EnumEnviroment.Desert;
        int rand = Random.Range(0, 2);

        switch (rand)
        {
            case 0:
                result = EnumEnviroment.Desert;
                break;
            case 1:
                result = EnumEnviroment.Winter;
                break;
            default:
                result = EnumEnviroment.Desert;
                break;
        }
        return result;
    }

    protected virtual void SpawnPlayers()
    {
        for (int i = 0; i < InputManager.playerSettings.Count; i++)
        {
            Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
            Players.Add(player);
            player.Controls = InputManager.playerSettings[i].controls;
            player.Avatar = InputManager.playerSettings[i].avatar;
            player.PrefabName = InputManager.playerSettings[i].name;
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
        int random = Random.Range(0, SpawnPoints.Count);
        Vector3 spawnPos = SpawnPoints[random].position;
        SpawnPoints.RemoveAt(random);
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

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crowdCheer);

        foreach (Player player in Players)
        {
            player.CanMove = false;
        }

        OnGameModeOver?.Invoke();//InputManagerCanvas

        for (int i = 0; i < InputManager.playerSettings.Count; i++)
        {
            if (InputManager.playerSettings[i].score == InGame.maxScore)
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
            do
            {
                random = Random.Range(0, gameMode.gameModes.Length);
            }
            while (GameModeDataBase.currentGameMode.gameModeType == gameMode.gameModes[random].gameModeType);

            GameModeDataBase.currentGameMode = gameMode.gameModes[random];
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

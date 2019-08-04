using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    protected List<Player> players = new List<Player>();

    [SerializeField]
    private List<Transform> spawnPoints;

    public const byte maximunDashLevel = 3;

    [SerializeField]
    GameModeDataBase gameMode = null;

    private EnumEnviroment enviroment = EnumEnviroment.Desert;

    [SerializeField]
    GameObject playerPrefab;

    protected PlayableDirector director;

    public GameObject floor;

    public event System.Action OnGameModeOver = null, OnGameOver = null;
    public System.Action<Player> OnCorrectPassBomb = null;

    public PlayableDirector Director { get => director; protected set => director = value; }
    public List<Player> Players { get => players; protected set => players = value; }
    protected List<Transform> SpawnPoints { get => spawnPoints; private set => spawnPoints = value; }
    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }
    public EnumEnviroment Enviroment { get => enviroment; private set => enviroment = value; }

    protected virtual void Awake()
    {
        if (Manager == null) Manager = this;
        else Destroy(this);

        Enviroment = GetRandomEnviroment();
        Director = GetComponent<PlayableDirector>();
        Director.Play();

        SpawnPlayers();
    }

    protected virtual void Start()
    {
        InitAudio();
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips.inGameMusic, 0.6f, 0.6f, 0.6f);
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
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
            Players.Add(player);
            player.Avatar = InGame.playerSettings[i].avatar;
            player.PrefabName = InGame.playerSettings[i].name;
            player.Id = (byte)i;
            player.SpawnPoint = GetSpawnPoint();
            player.transform.position = player.SpawnPoint;
            player.Initialize();
        }
    }

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
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crowdCheer, 0.6f);

        foreach (Player player in Players)
        {
            player.CanMove = false;
        }

        OnGameModeOver?.Invoke();

        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            if (InGame.playerSettings[i].score == InGame.maxScore)
            {
                OnGameOver?.Invoke();
                return;
            }
        }

        gameMode.GetNextGameMode();
    }

    protected virtual void OnDisable()
    {
        PlayerMenu.ResetDel();
        Bomb.OnExplode = null;
    }
}

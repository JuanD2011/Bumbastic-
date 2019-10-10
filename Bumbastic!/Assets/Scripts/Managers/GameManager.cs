using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    protected List<Player> players = new List<Player>();

    [SerializeField]
    private List<Transform> spawnPoints;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    private PowerUp powerUp = null;

    protected PlayableDirector director;

    public GameObject floor;

    public event System.Action OnGameModeOver = null, OnGameOver = null;
    public System.Action<Player> OnCorrectPassBomb = null;

    public PlayableDirector Director { get => director; protected set => director = value; }
    public List<Player> Players { get => players; protected set => players = value; }
    protected List<Transform> SpawnPoints { get => spawnPoints; private set => spawnPoints = value; }
    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }
    public EnumEnviroment Enviroment { get; private set; } = EnumEnviroment.Desert;
    public PowerUp PowerUp { get => powerUp; set => powerUp = value; }

    protected virtual void Awake()
    {
        if (Manager == null) Manager = this;
        else Destroy(this);

        PlayerInputHandler.ResetMyEvents();

        Enviroment = GetRandomEnviroment();
        Director = GetComponent<PlayableDirector>();
        Director.Play();
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
                AudioManager.instance.PlayAmbient(AudioManager.instance.audioClips.desertAmbient, 0.55f, 0.6f, 1f);
                break;
            case EnumEnviroment.Winter:
                AudioManager.instance.PlayAmbient(AudioManager.instance.audioClips.winterAmbient, 0.55f, 0.6f, 1f);
                break;
            default:
                AudioManager.instance.PlayAmbient(AudioManager.instance.audioClips.desertAmbient, 0.55f, 0.6f, 1f);
                break;
        }
    }

    private EnumEnviroment GetRandomEnviroment()
    {
        EnumEnviroment result = EnumEnviroment.Desert;
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                result = EnumEnviroment.Desert;
                break;
            case 1:
                result = EnumEnviroment.Winter;
                break;
            case 2:
                result = EnumEnviroment.Beach;
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
            player.transform.rotation = new Quaternion(0f, 180f, 0f, 1f);
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
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crowdCheer, 0.3f);

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
    }

    protected virtual void OnDisable()
    {
        PlayerMenu.ResetDel();
        ThrowerPlayer.ResetEvents();
        Bomb.ResetEvents();
        BasesBomb.ResetEvents();
    }
}

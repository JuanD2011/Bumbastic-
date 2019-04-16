using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private List<Player> players;
    private List<Player> bummies = new List<Player>();

    public Player BombHolder { get => bombHolder; set => bombHolder = value; }
    public Bomb Bomb { get => bomb; }
    public PlayableDirector Director { get => director; private set => director = value; }
    public List<Player> Players { get => players; private set => players = value; }

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    GameModeDataBase gameMode;

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
    public GameObject speedUpParticleSystem;

    public delegate void GameStateDelegate();
    public event GameStateDelegate OnGameOver;

    private bool cooldown;
    private float time = 0;

    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this);

        players = new List<Player>();
        Director = GetComponent<PlayableDirector>();
        SpawnPlayers();
    }

    private void Start()
    {
        Bomb.OnExplode += StartNewRound;
    }

    private void StartNewRound()
    {
        Players.Remove(BombHolder);
        BombHolder.gameObject.SetActive(false);

        foreach (Player player in Players)
        {
            player.transform.position = player.SpawnPoint;
            player.CanMove = false;
        }

        cooldown = true;
    }

    private void Update()
    {
        if (cooldown)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                cooldown = false;
                time = 0;
                GiveBombs();
            }
        }
    }

    private void SpawnPlayers()
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

    public void GiveBombs()
    {
        if (Players.Count > 1)
        {
            bummies = RandomizeBummieList();

            int[] _bummies = new int[bummies.Count];

            Director.Play();

            for (int i = 0; i < bummies.Count; i++)
            {
                Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 6, 0), Quaternion.identity);
                bummies.RemoveAt(i);
            }
            bomb.transform.position = bummies[0].transform.position + new Vector3(0, 6, 0);
            bomb.Timer = Random.Range(minTime -= 3f, maxTime -= 3f);
            bomb.Exploded = false;
            if (bomb.RigidBody != null)
            {
                bomb.RigidBody.velocity = Vector3.zero; 
            }
            bomb.transform.rotation = Quaternion.identity;
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

    /// <summary>
    /// Pass bomb to the player that the bomb touch
    /// </summary>
    /// <param name="_receiver"></param>
    public void PassBomb(Player _receiver)
    {
        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true;
        }
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;
        BombHolder = _receiver;
        this.Bomb.RigidBody.isKinematic = true;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult.transform);
    }

    /// <summary>
    /// Pass bomb between players when one touch another
    /// </summary>
    /// <param name="_receiver"></param>
    /// <param name="_transmitter"></param>
    public void PassBomb(Player _receiver, Player _transmitter)
    {
        _transmitter.HasBomb = false;
        _transmitter.Collider.enabled = true;
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;
        BombHolder = _receiver;
        this.Bomb.RigidBody.isKinematic = true;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult);
    }

    public void PassBomb()
    {
        BombHolder.HasBomb = true;
        this.Bomb.RigidBody.isKinematic = true;
        Bomb.transform.position = BombHolder.Catapult.position;
        Bomb.transform.SetParent(BombHolder.Catapult);
    }
}

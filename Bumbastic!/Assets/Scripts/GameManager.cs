using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private List<Player> players = new List<Player>();
    private List<Player> bummies = new List<Player>();

    public Player BombHolder { get => bombHolder; set => bombHolder = value; }
    public Bomb Bomb { get => bomb; }
    public PlayableDirector Director { get => director; set => director = value; }

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    PlayableDirector director;

    [SerializeField]
    private float minTime, maxTime;

    [SerializeField]
    private GameObject confettiBomb;

    private Player bombHolder;
    [SerializeField]
    private Bomb bomb;

    public PowerUp powerUp;

    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this);
    }

    private void Start()
    {
        Director = GetComponent<PlayableDirector>();

        SpawnPlayers();

        Invoke("GiveBombs", 1);
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

    public void GiveBombs()
    {
        if (players.Count > 1)
        {
            bummies = RandomizeBummieList();

            int[] _bummies = new int[bummies.Count];

            Director.Play();

            for (int i = 0; i < bummies.Count; i++)
            {
                Debug.Log("Toma bomba");
                Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                bummies.RemoveAt(i);
            }
            bomb.transform.position = bummies[0].transform.position + new Vector3(0, 4, 0);
            bomb.Timer = Random.Range(minTime, maxTime);
            bomb.gameObject.SetActive(true);
        }
        else if (players.Count == 1)
        {
            GameOver();
        }
    }

    private List<Player> RandomizeBummieList()
    {
        List<Player> bummies = players;
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
        Debug.Log("Game Over");
    }
}

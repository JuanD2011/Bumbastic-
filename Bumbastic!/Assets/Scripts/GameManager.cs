using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    List<Bummie> playersInGame = new List<Bummie>();
    public Bummie bombHolder;
    public Bomb bomb;
    [SerializeField] private ConfettiBomb confettiBomb;
    public PowerUp powerUp;
    private int playersSpawned = 0;

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }
    public PlayableDirector Director { get => director; private set => director = value; }

    public List<Transform> spawnPoints;

    private List<Bummie> bummies = new List<Bummie>();
    PlayableDirector director;//My timeline

    private float minTime = 20f, maxTime = 28f;

    public delegate void DelGameManager();
    public DelGameManager OnCanvasEnd;

    private void Start()
    {

        Director = GetComponent<PlayableDirector>();

        Invoke("AssignSpawnPoints", 1f);
    }

    private void AssignSpawnPoints()
    {

    }

    public void GiveBombs()
    {

    }

    private void RPC_BombSpawn()
    {
        
    }

    private List<Bummie> RandomizeBummieList()
    {
        List<Bummie> bummies = PlayersInGame;
        List<Bummie> randomBummies = new List<Bummie>();

        while (bummies.Count > 0)
        {
            int rand = Random.Range(0, bummies.Count);
            randomBummies.Add(bummies[rand]);
            bummies.RemoveAt(rand);
        }

        return randomBummies;
    }

    void GameOver(int IDwinner)
    {

    }

    public Vector3 GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPos = spawnPoints[random].position;
        spawnPoints.RemoveAt(random);
        return spawnPos;
    }
}

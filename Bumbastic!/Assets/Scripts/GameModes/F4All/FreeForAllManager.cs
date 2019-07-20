using UnityEngine;

public class FreeForAllManager : HotPotatoManager
{
    public static FreeForAllManager FreeForAll;

    [SerializeField]
    private byte maxKills = 3;

    byte[] killsCounter;
    byte winnerID = 0;

    private Player lastPlayerGiven;

    private byte timesBombPlayed = 0;

    public byte[] KillsCounter { get => killsCounter; private set => killsCounter = value; }
    public byte WinnerID { get => winnerID; private set => winnerID = value; }

    public delegate void DelFreeForAll(byte _killerID);
    public event DelFreeForAll OnPlayerKilled;

    protected override void Awake()
    {
        base.Awake();

        if (FreeForAll == null) FreeForAll = this;
        else Destroy(this);
    }

    protected override void Start()
    {
        base.Start();
        KillsCounter = new byte[Players.Count];
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnBombExplode()
    {
        lastPlayerGiven = BombHolder;
        Bomb.RigidBody.isKinematic = true;
        BombHolder.transform.position = BombHolder.SpawnPoint;

        foreach (Player player in Players)
        {
            StartCoroutine(player.Rumble(0.8f, 0.8f, 1f));
        }

        if (transmitter != null)
        {
            if (timesBombPlayed > 1 && BombHolder.Id != transmitter.Id)
            {
                KillsCounter[transmitter.Id] += 1;
                OnPlayerKilled?.Invoke(transmitter.Id);

                if (killsCounter[transmitter.Id] == maxKills)
                {
                    WinnerID = transmitter.Id;
                    InGame.playerSettings[WinnerID].score += 1;
                    GameOver();
                    return;
                }

                transmitter = null;
            }
        }

        BombHolder = null;

        timesBombPlayed = 0;

        foreach (Player player in Players)
        {
            player.Collider.enabled = true;
        }
        
        cooldown = true;
    }

    protected override void GiveBombs()
    {
        int random = Random.Range(0, Players.Count);

        while (Players[random] == lastPlayerGiven)
        {
            random = Random.Range(0, Players.Count);
        }

        Bomb.gameObject.SetActive(true);
        Bomb.Collider.enabled = true;
        Bomb.transform.position = Players[random].transform.position + new Vector3(0, 2, 0);
        Bomb.Timer = Random.Range(minTime, maxTime);
        Bomb.Exploded = false;
        Bomb.RigidBody.velocity = Vector3.zero;
        Bomb.transform.rotation = Quaternion.identity;
        Bomb.SetAnimationKeys();
    }

    public override void PassBomb(Player _receiver, Bomb _Bomb)
    {
        base.PassBomb(_receiver, _Bomb);
        if (!Bomb.Exploded) timesBombPlayed++;
    }

    public override void PassBomb(Player _receiver, Player _transmitter, Bomb _Bomb)
    {
        base.PassBomb(_receiver, _transmitter, _Bomb);
        if (!Bomb.Exploded) timesBombPlayed++;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}

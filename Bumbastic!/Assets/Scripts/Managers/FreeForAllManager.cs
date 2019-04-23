using UnityEngine;

public class FreeForAllManager : HotPotatoManager
{
    public static FreeForAllManager FreeForAll;

    private bool gameOver;

    [SerializeField]
    private byte maxKills = 3;

    byte[] killsCounter;

    public byte[] KillsCounter { get => killsCounter; private set => killsCounter = value; }

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
        KillsCounter = new byte[players.Count];
    }

    protected override void Update()
    {
        if (cooldown)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                cooldown = false;
                time = 0;
                GiveBomb();
            }
        }
    }

    protected override void OnBombExplode()
    {
        transmitter.transform.position = transmitter.SpawnPoint;
        cooldown = true;

        Debug.Log(string.Format("{0}/{1}", transmitter.Id, BombHolder.Id));
        if (transmitter.Id != BombHolder.Id)
        {
            KillsCounter[transmitter.Id] += 1;
            OnPlayerKilled?.Invoke(transmitter.Id);//HUDFreeForAll hears it.
        }
    }

    protected void GiveBomb()
    {
        for (int i = 0; i < killsCounter.Length; i++)
        {
            if (KillsCounter[i] == maxKills)
            {
                InGame.playerSettings[i].score += 1;
                gameOver = true;
                break;
            }
        }

        if (!gameOver)
        {
            int random = Random.Range(0, Players.Count);

            Bomb.transform.position = Players[random].transform.position + new Vector3(0, 2, 0);
            Bomb.Timer = Random.Range(minTime, maxTime);
            Bomb.Exploded = false;
            if (Bomb.RigidBody != null)
            {
                Bomb.RigidBody.velocity = Vector3.zero;
            }
            Bomb.transform.rotation = Quaternion.identity;
            Bomb.gameObject.SetActive(true);
        }
        else
        {
            GameOver();
        }
    }
}

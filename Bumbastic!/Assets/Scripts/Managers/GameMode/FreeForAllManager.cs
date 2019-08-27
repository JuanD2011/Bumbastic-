using UnityEngine;

public class FreeForAllManager : HotPotatoManager
{
    public static FreeForAllManager FreeForAll;

    readonly byte maxLifePoints = 3;

    bool canGiveConfetti = true;

    public byte[] LifePoints { get; private set; }

    public event System.Action<byte> OnPlayerKilled;

    protected override void Awake()
    {
        base.Awake();

        if (FreeForAll == null) FreeForAll = this;
        else Destroy(this);
    }

    protected override void Start()
    {
        base.Start();

        LifePoints = new byte[Players.Count];

        for (int i = 0; i < Players.Count; i++)
        {
            LifePoints[i] = maxLifePoints;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnBombExplode()
    {
        Bomb.RigidBody.isKinematic = true;
        BombHolder.transform.position = BombHolder.SpawnPoint;

        foreach (Player player in Players)
        {
            StartCoroutine(player.Rumble(0.8f, 0.8f, 1f));
        }

        if (LifePoints[BombHolder.Id] > 1)
        {
            LifePoints[BombHolder.Id] -= 1;
            OnPlayerKilled?.Invoke(BombHolder.Id);
        }
        else
        {
            Players.Remove(BombHolder);
            BombHolder.gameObject.SetActive(false);
        }

        if (Players.Count == 1)
        {
            InGame.playerSettings[Players[0].Id].score += 1;
            InGame.lastWinner = InGame.playerSettings[Players[0].Id];
            GameOver();
            return;
        }

        foreach (Player player in Players)
        {
            player.Collider.enabled = true;
        }
        
        BombHolder = null;
        cooldown = true;
    }

    protected override void GiveBombs()
    {
        bummies = RandomizeBummieList();

        if (canGiveConfetti)
        {
            for (int i = 0; i < bummies.Count; i++)
            {
                Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
                bummies.RemoveAt(i);
            }
            canGiveConfetti = false;
        }

        Bomb.gameObject.SetActive(true);
        Bomb.Collider.enabled = true;
        Bomb.transform.position = bummies[0].transform.position + new Vector3(0, 2.5f, 0);
        Bomb.Timer = Random.Range(minTime, maxTime);
        Bomb.Exploded = false;
        Bomb.RigidBody.velocity = Vector3.zero;
        Bomb.transform.rotation = Quaternion.identity;
        Bomb.SetAnimationKeys();
    }

    protected override void BombHolderChange(ThrowerPlayer _player, Bomb _bomb)
    {
        base.BombHolderChange(_player, _bomb);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}

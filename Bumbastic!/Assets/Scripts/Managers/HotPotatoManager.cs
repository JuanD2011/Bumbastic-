using System.Collections.Generic;
using UnityEngine;

public class HotPotatoManager : GameManager
{
    public static HotPotatoManager HotPotato;

    protected Player bombHolder;
    protected Player transmitter;

    [SerializeField]
    private Bomb bomb;

    [SerializeField]
    protected GameObject confettiBomb;

    [SerializeField]
    protected float minTime, maxTime;

    protected bool cooldown;
    protected float time = 0;

    protected List<Player> bummies = new List<Player>();

    public Player BombHolder { get => bombHolder; protected set => bombHolder = value; }
    public Bomb Bomb { get => bomb; private set => bomb = value; }

    protected override void Awake()
    {
        if (HotPotato == null) HotPotato = this;
        else Destroy(this);

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Bomb.OnExplode += OnBombExplode;
    }

    protected virtual void Update()
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

    protected virtual void OnBombExplode()
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

    protected override void GiveBombs()
    {
        if (Players.Count > 1)
        {
            bummies = RandomizeBummieList();

            int[] _bummies = new int[bummies.Count];

            Director.Play();

            for (int i = 0; i < bummies.Count; i++)
            {
                Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                bummies.RemoveAt(i);
            }
            Bomb.gameObject.SetActive(true);
            Bomb.transform.position = bummies[0].transform.position + new Vector3(0, 1, 0);
            Bomb.Timer = Random.Range(minTime -= 3f, maxTime -= 3f);
            Bomb.Exploded = false;
            if (Bomb.RigidBody != null)
            {
                Bomb.RigidBody.velocity = Vector3.zero;
            }
            Bomb.transform.rotation = Quaternion.identity;
        }
        else if (Players.Count == 1)
        {
            InGame.playerSettings[Players[0].Id].score += 1;
            GameOver();
        }
    }

    /// <summary>
    /// Pass bomb to the player that the bomb touch
    /// </summary>
    /// <param name="_receiver"></param>
    public override void PassBomb(Player _receiver)
    {
        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true;
            transmitter = BombHolder;
        }
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;
        BombHolder = _receiver;
        Bomb.RigidBody.isKinematic = true;
        Bomb.Collider.enabled = false;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult.transform);

        float probTosound = Random.Range(0f, 1f);

        if (probTosound < 0.33f)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cTransmitter, 1f);
        }
    }

    /// <summary>
    /// Pass bomb between players when one touch another
    /// </summary>
    /// <param name="_receiver"></param>
    /// <param name="_transmitter"></param>
    public override void PassBomb(Player _receiver, Player _transmitter)
    {
        transmitter = _transmitter;
        _transmitter.HasBomb = false;
        _transmitter.Collider.enabled = true;
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;
        BombHolder = _receiver;
        Bomb.RigidBody.isKinematic = true;
        Bomb.Collider.enabled = false;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult);
        StartCoroutine(_receiver.Stun(false, 1f));

        float probTosound = Random.Range(0f, 1f);

        if (probTosound < 0.33f)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cTransmitter, 1f);
        }
    }

    public override void PassBomb()
    {
        BombHolder.HasBomb = true;
        BombHolder.SetOverrideAnimator(true);
        Bomb.RigidBody.isKinematic = true;
        Bomb.Collider.enabled = false;
        Bomb.transform.position = BombHolder.Catapult.position;
        Bomb.transform.SetParent(BombHolder.Catapult);
    }
}

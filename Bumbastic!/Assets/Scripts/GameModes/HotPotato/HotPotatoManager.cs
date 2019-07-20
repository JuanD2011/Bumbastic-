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

    [SerializeField] 
    PowerUp powerUp = null;

    [SerializeField] GameObject magnetParticleSystem;
    [SerializeField] GameObject speedUpParticleSystem;

    protected bool cooldown = true;
    protected float time = 0f;

    protected List<Player> bummies = new List<Player>();

    public event System.Action<Player> OnBombHolderChanged = null;

    public Player BombHolder { get => bombHolder; protected set => bombHolder = value; }
    public Bomb Bomb { get => bomb; private set => bomb = value; }
    public PowerUp PowerUp { get => powerUp; set => powerUp = value; }
    public GameObject MagnetParticleSystem { get => magnetParticleSystem; set => magnetParticleSystem = value; }
    public GameObject SpeedUpParticleSystem { get => speedUpParticleSystem; set => speedUpParticleSystem = value; }

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
            if (time > 1f)
            {
                cooldown = false;
                time = 0f;
                GiveBombs();
            }
            time += Time.deltaTime;
        }
    }

    protected virtual void OnBombExplode()
    {
        foreach (Player player in Players)
        {
            StartCoroutine(player.Rumble(0.8f, 0.8f, 1f));
        }

        Players.Remove(BombHolder);
        BombHolder.gameObject.SetActive(false);

        foreach (Player player in Players)
        {
            player.transform.position = player.SpawnPoint;
            player.Animator.SetFloat("speed", 0f);
            player.CanMove = false;
        }

        if (Players.Count == 1)
        {
            InGame.playerSettings[Players[0].Id].score += 1;
            GameOver();
            return;
        }

        cooldown = true;
    }

    protected virtual void GiveBombs()
    {
        bummies = RandomizeBummieList();

        for (int i = 0; i < bummies.Count; i++)
        {
            Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            bummies.RemoveAt(i);
        }
        Bomb.gameObject.SetActive(true);
        Bomb.Collider.enabled = true;
        Bomb.transform.position = bummies[0].transform.position + new Vector3(0, 1, 0);
        Bomb.Timer = Random.Range(minTime -= 3f, maxTime -= 3f);
        Bomb.Exploded = false;
        Bomb.RigidBody.velocity = Vector3.zero;
        Bomb.transform.rotation = Quaternion.identity;
        Bomb.SetAnimationKeys();
        Director.Play();
    }

    /// <summary>
    /// Pass bomb to the player that the bomb touch
    /// </summary>
    /// <param name="_receiver"></param>
    public override void PassBomb(Player _receiver, Bomb _Bomb)
    {
        OnCorrectPassBomb?.Invoke(BombHolder);
        OnBombHolderChanged?.Invoke(_receiver);

        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true;
            transmitter = BombHolder;
        }

        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;

        BombHolder = _receiver;
        Bomb.RigidBody.velocity = Vector2.zero;
        Bomb.RigidBody.isKinematic = true;
        Bomb.Collider.enabled = false;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult.transform);
        StartCoroutine(_receiver.Rumble(0.2f, 0.2f, 0.2f));

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
    public override void PassBomb(Player _receiver, Player _transmitter, Bomb _Bomb)
    {
        OnCorrectPassBomb?.Invoke(_transmitter);
        OnBombHolderChanged?.Invoke(_receiver);

        transmitter = _transmitter;

        _transmitter.HasBomb = false;
        _transmitter.Collider.enabled = true;

        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;

        BombHolder = _receiver;
        Bomb.RigidBody.velocity = Vector2.zero;
        Bomb.RigidBody.isKinematic = true;
        Bomb.Collider.enabled = false;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult);
        StartCoroutine(_receiver.Stun(false, 1f));
        StartCoroutine(_receiver.Rumble(0.2f, 0.2f, 0.2f));

        float probTosound = Random.Range(0f, 1f);

        if (probTosound < 0.33f)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cTransmitter, 1f);
        }
    }

    public virtual void PassBomb()
    {
        BombHolder.HasBomb = true;
        BombHolder.Collider.enabled = false;
        BombHolder.SetOverrideAnimator(true);
        Bomb.RigidBody.velocity = Vector2.zero;
        Bomb.RigidBody.isKinematic = true;
        Bomb.Collider.enabled = false;
        Bomb.transform.position = BombHolder.Catapult.position;
        Bomb.transform.SetParent(BombHolder.Catapult);
        StartCoroutine(BombHolder.Rumble(0.2f, 0.2f, 0.2f));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}

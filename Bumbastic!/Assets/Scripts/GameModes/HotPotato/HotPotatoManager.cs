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

    protected bool cooldown;
    protected float time = 0f;

    protected List<Player> bummies = new List<Player>();

    private System.Action onBombArmed = null;

    public Player BombHolder { get => bombHolder; protected set => bombHolder = value; }
    public Bomb Bomb { get => bomb; private set => bomb = value; }
    public System.Action OnBombArmed { get => onBombArmed; set => onBombArmed = value; }
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
        Bomb.onExplode += OnBombExplode;
        Player.OnCatchBomb += BombHolderChange;
        Bomb.OnFloorCollision += ReturnBomb;
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

        cooldown = true;
    }

    protected override void GiveBombs()
    {
        if (Players.Count > 1)
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
            if (Bomb.RigidBody != null)
            {
                Bomb.RigidBody.velocity = Vector3.zero;
            }
            Bomb.transform.rotation = Quaternion.identity;
            Director.Play();
            OnBombArmed?.Invoke();//Bomb hears it.
        }
        else if (Players.Count == 1)
        {
            InGame.playerSettings[Players[0].Id].score += 1;
            GameOver();
        }
    }

    protected virtual void BombHolderChange(Player _player, Bomb _bomb)
    {
        OnCorrectPassBomb?.Invoke(BombHolder);

        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true; 
        }

        BombHolder = _player;
    }

    protected override void ReturnBomb(Bomb _bomb)
    {
        BombHolder.HasBomb = true;
        BombHolder.Collider.enabled = false;
        BombHolder.SetOverrideAnimator(true);
        _bomb.RigidBody.velocity = Vector3.zero;
        _bomb.RigidBody.isKinematic = true;
        _bomb.Collider.enabled = false;
        _bomb.transform.position = BombHolder.Catapult.position;
        _bomb.transform.SetParent(BombHolder.Catapult);
        StartCoroutine(BombHolder.Rumble(0.2f, 0.2f, 0.2f));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Player.OnCatchBomb -= BombHolderChange;
        Bomb.OnFloorCollision -= ReturnBomb;
    }
}

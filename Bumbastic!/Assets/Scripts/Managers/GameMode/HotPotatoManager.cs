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
        Bomb.OnFloorCollision += ReturnBomb;

        Player.OnCatchBomb += BombHolderChange;
    }

    protected virtual void Update()
    {
        if (cooldown)
        {
            if (time >= 0.5f)
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
            Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
            bummies.RemoveAt(i);
        }

        Bomb.gameObject.SetActive(true);
        Bomb.Collider.enabled = true;
        Bomb.transform.position = bummies[0].transform.position + new Vector3(0, 2.5f, 0);
        Bomb.Timer = Random.Range(minTime -= 3f, maxTime -= 3f);
        Bomb.Exploded = false;
        Bomb.RigidBody.velocity = Vector3.zero;
        Bomb.transform.rotation = Quaternion.identity;
        Bomb.SetAnimationKeys();

        Director.Play();
    }

    protected virtual void BombHolderChange(Player _player, Bomb _bomb)
    {
        if (BombHolder != null)
        {
            if (_player != BombHolder) OnCorrectPassBomb?.Invoke(BombHolder);
            //Debug.Log(string.Format("Bombholder {0}, receptor {1}", BombHolder.Avatar.name, _player.Avatar.name)); 
        }

        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true;
        }

        float probTosound = Random.Range(0f, 1f);

        if (probTosound < 0.2f) AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cTransmitter, 1f);

        BombHolder = _player;
        OnBombHolderChanged?.Invoke(BombHolder);
    }

    protected override void ReturnBomb(Bomb _bomb)
    {
        BombHolder.CatchBomb(_bomb);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Player.OnCatchBomb -= BombHolderChange;
        Bomb.OnFloorCollision -= ReturnBomb;
    }
}

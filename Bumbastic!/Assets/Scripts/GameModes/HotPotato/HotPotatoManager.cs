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
            Bomb.transform.position = bummies[0].transform.position + new Vector3(0, 1, 0);
            Bomb.Timer = Random.Range(minTime -= 3f, maxTime -= 3f);
            Bomb.Exploded = false;
            if (Bomb.RigidBody != null)
            {
                Bomb.RigidBody.velocity = Vector3.zero;
            }
            Bomb.transform.rotation = Quaternion.identity;
            OnBombArmed?.Invoke();//Bomb hears it.
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
    public override void PassBomb(Player _receiver, Bomb _Bomb)
    {
        OnCorrectPassBomb?.Invoke(BombHolder);  

        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true;
            transmitter = BombHolder;

            foreach (Renderer renderer in transmitter.AvatarSkinnedMeshRenderers)
            {
                renderer.material.shader = DefaultShader;
            }
        }

        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;

        foreach (Renderer renderer in _receiver.AvatarSkinnedMeshRenderers)
        {
            renderer.material.shader = bombHolderShader;
        }

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

        transmitter = _transmitter;
        _transmitter.HasBomb = false;
        _transmitter.Collider.enabled = true;

        foreach (Renderer renderer in _transmitter.AvatarSkinnedMeshRenderers)
        {
            renderer.material.shader = DefaultShader;
        }

        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;

        foreach (Renderer renderer in _receiver.AvatarSkinnedMeshRenderers)
        {
            renderer.material.shader = bombHolderShader;
        }

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

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasesBomb : Bomb
{
    [SerializeField] float minTime = 10f, maxTime = 18f;

    public ThrowerPlayer ThrowerPlayer { get; private set; }

    public static event System.Action<ThrowerPlayer> OnBasesBombExplode = null;

    protected override void Awake()
    {
        base.Awake();
        CanCount = true;
        Timer = Random.Range(minTime, maxTime);
    }

    private void OnEnable()
    {
        Collider.enabled = true;
        Exploded = false;
    }

    private void Start()
    {
        cParticleModification.OnComplete += () => gameObject.SetActive(false);
        SetAnimationKeys();

        ThrowerPlayer.OnCatchBomb += AssignPlayer;
        ThrowerPlayer.OnBombThrew += CheckIfIThrewBomb;
    }

    private void CheckIfIThrewBomb(Bomb _bomb)
    {
        if (_bomb as BasesBomb == this)
        {
            ThrowerPlayer = null;
        }
    }

    public void SetThrowerPlayer(ThrowerPlayer _ThrowerPlayer)
    {
        if (_ThrowerPlayer != null)
        {
            ThrowerPlayer = _ThrowerPlayer;
        }
        else
        {
            ThrowerPlayer = null;
        }
    }

    private void AssignPlayer(ThrowerPlayer _throwerPlayer, Bomb _bomb)
    {
        if (_bomb as BasesBomb == this)
        {
            ThrowerPlayer = _throwerPlayer;
        }
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(elapsedTime) * speed;

        if (!Exploded)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime > Timer && !Exploded)
        {
            Explode();
        }
    }

    public override void Explode()
    {
        transform.SetParent(null);
        elapsedTime = 0;
        Exploded = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bomb, 0.7f);
        CameraShake.instance.OnShakeDuration?.Invoke(0.4f, 6f, 1.2f);
        RigidBody.isKinematic = false;
        Collider.enabled = false;
        cParticleModification.Execute();
        OnBasesBombExplode?.Invoke(ThrowerPlayer);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor") && !Exploded && ThrowerPlayer != null)
        {
            SetThrowerPlayer(null);
        }
    }

    public new static void ResetEvents()
    {
        OnBasesBombExplode = null;
    }

    
}

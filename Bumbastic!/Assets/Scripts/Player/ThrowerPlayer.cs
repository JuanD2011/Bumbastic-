using System;
using System.Collections;
using UnityEngine;

public class ThrowerPlayer : Player
{
    [SerializeField]
    private float throwForce = 30f, dashForce = 15f, penaltyOnPassBomb = 1f;

    public event Action<ThrowerPlayer> OnDashExecuted = null;
    public static event Action<ThrowerPlayer, Bomb> OnCatchBomb;

    private bool throwing;
    public bool HasBomb { get; set; }
    public Transform Catapult { get; private set; }
    public byte DashCount { get; private set; }
    public Bomb Bomb { get; set; } = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Manager.OnCorrectPassBomb += IncreaseDashCounter;
        Bomb.OnExplode += ResetPlayer;
        Bomb.OnAboutToExplode += BombIsAboutToExplode;
    }

    protected override void Update()
    {
        base.Update();

        if (CanMove)
        {
            if (InputDirection != Vector2.zero && !throwing)
            {
                TargetRotation = Mathf.Atan2(InputDirection.x, InputDirection.y) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetRotation, ref turnSmoothVel, TurnSmooth);
            }
        }
    }

    private void IncreaseDashCounter(Player _Transmitter)
    {
        if (_Transmitter == this)
        {
            DashCount = (DashCount <= GameManager.maximunDashLevel) ? DashCount += 1 : DashCount = GameManager.maximunDashLevel;
        }
    }
    private void ResetPlayer()
    {
        Collider.enabled = true;
        HasBomb = false;
        throwing = false;
        Animator.runtimeAnimatorController = animatorWNoBomb;
    }

    private void BombIsAboutToExplode(Bomb _bomb)
    {
        if (HasBomb && _bomb == Bomb) StartCoroutine(Rumble(0.3f, 0.3f, 0.2f));
    }

    public override void Initialize()
    {
        base.Initialize();

        Bummie cBummie = GetComponentInChildren<Bummie>();

        if (cBummie != null)
        {
            Catapult = cBummie.Catapult;
        }
        else
        {
            Debug.LogError("Bummie component was not found");
        }

        HasBomb = false;
    }

    public void OnThrow()
    {
        Throw();
    }

    public void OnDash()
    {
        Dash();
    }

    public void Throw()
    {
        if (HasBomb && CanMove && !throwing)
        {
            SetOverrideAnimator(false);
            StopCoroutine("SyncThrowAnim");
            if (Bomb != null) StartCoroutine(SyncThrowAnim());
        }
    }

    private void Dash()
    {
        if (DashCount == 0) return;

        if (CanMove)
        {
            switch (DashCount)
            {
                case 1:
                    Rigidbody.AddForce(transform.forward * dashForce * 0.6f, ForceMode.Impulse);
                    break;
                case 2:
                    Rigidbody.AddForce(transform.forward * dashForce * 0.8f, ForceMode.Impulse);
                    break;
                case 3:
                    Rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
                    break;
                default:
                    break;
            }
            DashCount = 0;
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.dash, 1f);
            OnDashExecuted?.Invoke(this);
        }
    }

    IEnumerator SyncThrowAnim()
    {
        Animator.SetTrigger("Throw");
        throwing = true;

        float elapsedTime = 0f;

        Quaternion initialRotation = transform.rotation;

        Vector3 aiming = new Vector3(InputAiming.x, 0, InputAiming.y);


        while (elapsedTime < 0.15f)
        {
            if (InputAiming != Vector2.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aiming);
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / 0.15f);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|BombLaunch"));
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f);

        if (InputAiming != Vector2.zero)
        {
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * aiming;
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|BombLaunch"))
            {
                Bomb.RigidBody.isKinematic = false;
                Bomb.transform.SetParent(null);
                Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
            }
        }
        else
        {
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * transform.forward;
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|BombLaunch"))
            {
                Bomb.RigidBody.isKinematic = false;
                Bomb.transform.SetParent(null);
                Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
            }
        }

        Bomb.Collider.enabled = true;

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bombThrow, 0.7f);

        float prob = UnityEngine.Random.Range(0f, 1f);
        if (prob < 0.33f) AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cThrow, 0.7f);

        HasBomb = false;
        throwing = false;
        Bomb = null;
        Collider.enabled = true;
    }

    public void CatchBomb(Bomb _bomb)
    {
        OnCatchBomb?.Invoke(this, _bomb);
        
        HasBomb = true;
        Collider.enabled = false;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bombReception, 0.6f);
        Animator.SetTrigger("Reception");
        SetOverrideAnimator(true);

        Bomb = _bomb;
        _bomb.RigidBody.velocity = Vector3.zero;
        _bomb.RigidBody.isKinematic = true;
        _bomb.Collider.enabled = false;
        _bomb.transform.position = Catapult.position;
        _bomb.transform.SetParent(Catapult);

        StartCoroutine(Rumble(0.2f, 0.2f, 0.2f));
    }

    private void OnDisable()
    {
        Bomb.OnAboutToExplode -= BombIsAboutToExplode;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Bomb bomb = other.GetComponent<Bomb>();
        if (bomb != null) CatchBomb(bomb);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        ThrowerPlayer player = collision.gameObject.GetComponentInParent<ThrowerPlayer>();

        if (player != null)
        {
            if (player.HasBomb && player.CanMove)
            {
                player.HasBomb = false;
                player.SetOverrideAnimator(false);
                CatchBomb(player.Bomb);
                StartCoroutine(Stun(false, penaltyOnPassBomb));
            }
        }
    }
}

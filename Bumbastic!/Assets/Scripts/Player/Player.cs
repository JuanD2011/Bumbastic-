using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Plugins.PlayerInput;

public class Player : MonoBehaviour
{
    #region Movement
    private float targetRotation;
    protected float speedSmooothTime = 0.075f, animationSpeedPercent;
    [SerializeField]
    private float moveSpeed = 0f, turnSmooth = 0.15f, powerUpSpeed = 0f;
    private float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    private Vector2 inputDirection;
    #endregion

    public bool speedPU;

    [SerializeField] float penaltyOnPassBomb = 1f;

    [SerializeField] float dashForce = 7f;
    public event Action<Player> OnDashExecuted = null;
    public event Action<bool> OnStuned = null;

    [SerializeField]
    private float throwForce = 0f;

    private GameObject player;

    private AnimatorOverrideController animatorWNoBomb;
    [SerializeField] AnimatorOverrideController animatorWBomb = null;

    private bool throwing;

    private Gamepad gamepad;

    public bool CanMove { get; set; } = false;
    public Vector3 SpawnPoint { get; set; }
    public bool HasBomb { get; set; }
    public Vector2 InputAiming { get; private set; }
    public GameObject Avatar { get; set; }
    public byte Id { get; set; }
    public Animator Animator { get; set; }
    public string PrefabName { get; set; }
    public SphereCollider Collider { get; private set; }
    public Transform Catapult { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public byte DashCount { get; private set; }
    public float TurnSmooth { get => turnSmooth; private set => turnSmooth = value; }
    public Bomb Bomb { get; set; } = null;

    public static event Action<Player, Bomb> OnCatchBomb;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        gamepad = (Gamepad)GetComponent<PlayerInput>().devices[0];
    }

    private void Start()
    {
        if (animatorWNoBomb == null) animatorWNoBomb = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        GameManager.Manager.Director.stopped += (PlayableDirector _playableDirector) => CanMove = true;
        GameManager.Manager.OnCorrectPassBomb += IncreaseDashCounter;
        Bomb.OnExplode += ResetPlayer;
        Bomb.OnAboutToExplode += BombIsAboutToExplode;
    }

    public void Initialize()
    {
        player = Instantiate(Avatar, transform.position, transform.rotation);
        player.transform.SetParent(transform);

        Bummie cBummie = GetComponentInChildren<Bummie>();

        if (cBummie != null)
        {
            Animator = cBummie.Animator;
            Catapult = cBummie.Catapult;
        }
        else
        {
            Debug.LogError("Bummie component was not found");
        }

        foreach (SphereCollider sphere in cBummie.SphereColliders)
        {
            if (sphere.isTrigger)
            {
                Collider = sphere;
            }
        }

        HasBomb = false;
    }

    private void IncreaseDashCounter(Player _Transmitter)
    {
        if (_Transmitter == this)
        {
            DashCount = (DashCount <= GameManager.maximunDashLevel) ? DashCount += 1 : DashCount = GameManager.maximunDashLevel; 
        }
    }

    public void OnMove(InputValue context)
    {
        inputDirection = context.Get<Vector2>();
    }

    public void OnAim(InputValue context)
    {
        InputAiming = context.Get<Vector2>();
        InputAiming.Normalize();
    }

    public void OnThrow()
    {
        Throw();
    }

    public void OnDash()
    {
        Dash();
    }

    public void OnStart()
    {
        PlayerMenu.OnStartButton?.Invoke(Id);
    }

    public void PodiumAnimation(int _podiumState)
    {
        Animator.SetInteger("PodiumState", _podiumState);
        switch (_podiumState)
        {
            case 0:
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cWin, 1f);
                break;
            case 1:
            case 2:
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cLose, 1f);
                break;
            default:
                break;
        }
    }

    private void ResetPlayer()
    {
        HasBomb = false;
        throwing = false;
        Animator.runtimeAnimatorController = animatorWNoBomb;
    }

    public void SetOverrideAnimator(bool _hasBomb)
    {
        if (!_hasBomb)
        {
            Animator.runtimeAnimatorController = animatorWNoBomb;
        }
        else
        {
            if (Animator.runtimeAnimatorController != animatorWBomb)
            {
                Animator.runtimeAnimatorController = animatorWBomb;
            }
        }
    }

    void Update()
    {
        if (CanMove)
        {
            inputDirection.Normalize();

            if (inputDirection != Vector2.zero && !throwing)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, TurnSmooth);
            }

            targetSpeed = ((speedPU) ? powerUpSpeed : moveSpeed) * inputDirection.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmooothTime);
            animationSpeedPercent = ((speedPU) ? 1 : 0.5f) * inputDirection.magnitude;
            Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            Rigidbody.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
        }
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        PowerUp powerUpCollisioned = collision.gameObject.GetComponent<PowerUp>();
        Player player = collision.gameObject.GetComponentInParent<Player>();

        if (powerUpCollisioned != null)
        {
            if (powerUpCollisioned.transform.GetComponent<Player>() == null)
            {
                IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
                powerUp.PickPowerUp(this);
            }
        }

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

    private void OnTriggerEnter(Collider other)
    {
        Bomb bomb = other.GetComponent<Bomb>();

        if (bomb != null) CatchBomb(bomb);
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

    public void Stun(bool _stun)
    {
        if (_stun) Animator.SetTrigger("Stun");

        Animator.SetBool("CanMove", !_stun);

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 0.6f, _stun);
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cStun, 1f);

        CanMove = !_stun;
    }


    /// <summary>
    /// This will be used on Pass bomb and Wagon hit.
    /// </summary>
    /// <param name="_animStun">Only if stun animation is needed</param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    public IEnumerator Stun(bool _animStun, float _duration)
    {
        StartCoroutine(Rumble(0.4f, 0.4f, 0.2f));
        Animator.SetBool("CanMove", false);
        inputDirection = Vector2.zero;
        CanMove = false;

        if (_animStun)
        {
            Animator.SetTrigger("Stun");
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 0.6f, true);
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cStun, 1f);
            OnStuned?.Invoke(true);
        }

        yield return new WaitForSeconds(_duration);

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 0.6f, false);

        Animator.SetBool("CanMove", true);
        CanMove = true;
        OnStuned?.Invoke(false);
    }

    private void BombIsAboutToExplode(Bomb _bomb)
    {
        if (HasBomb && _bomb == Bomb) StartCoroutine(Rumble(0.3f, 0.3f, 0.2f));
    }

    public IEnumerator Rumble(float _leftSpeed, float _rightSpeed, float _duration)
    {
        gamepad.SetMotorSpeeds(_leftSpeed, _rightSpeed);
        yield return new WaitForSeconds(_duration);
        gamepad.SetMotorSpeeds(0f, 0f);
    }

    private void OnDisable()
    {
        Bomb.OnAboutToExplode -= BombIsAboutToExplode;
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.PlayerInput;

public class Player : MonoBehaviour
{
    #region Movement
    protected float targetRotation;
    protected float speedSmooothTime = 0.075f, animationSpeedPercent;
    [SerializeField]
    protected float moveSpeed = 0f, turnSmooth = 0.15f, powerUpSpeed = 0f;
    protected float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    protected Vector2 inputDirection;
    #endregion

    public bool speedPU;

    public event Action<bool> OnStuned = null;

    private GameObject player;

    protected AnimatorOverrideController animatorWNoBomb;
    [SerializeField] protected AnimatorOverrideController animatorWBomb = null;


    private Gamepad gamepad;

    public bool CanMove { get; set; } = false;
    public Vector3 SpawnPoint { get; set; }
    public Vector2 InputAiming { get; private set; }
    public GameObject Avatar { get; set; }
    public byte Id { get; set; }
    public Animator Animator { get; set; }
    public string PrefabName { get; set; }
    public SphereCollider Collider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public float TurnSmooth { get => turnSmooth; private set => turnSmooth = value; }

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        gamepad = (Gamepad)GetComponent<PlayerInput>().devices[0];
    }

    protected virtual void Start()
    {
        if (animatorWNoBomb == null) animatorWNoBomb = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        GameManager.Manager.Director.stopped += (PlayableDirector _playableDirector) => CanMove = true;
    }

    public virtual void Initialize()
    {
        player = Instantiate(Avatar, transform.position, transform.rotation);
        player.transform.SetParent(transform);

        Bummie cBummie = GetComponentInChildren<Bummie>();

        if (cBummie != null)
        {
            Animator = cBummie.Animator;
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

    protected virtual void Update()
    {
        if (CanMove)
        {
            inputDirection.Normalize();

            if (inputDirection != Vector2.zero)
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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        PowerUp powerUpCollisioned = collision.gameObject.GetComponent<PowerUp>();

        IBounce iBounce = collision.gameObject.GetComponent<IBounce>();

        if (iBounce != null) iBounce.Bounce(gameObject, collision);

        if (powerUpCollisioned != null)
        {
            if (powerUpCollisioned.transform.GetComponent<Player>() == null)
            {
                IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
                powerUp.PickPowerUp(GetComponentInChildren<ThrowerPlayer>());
            }
        }
    }    

    protected virtual void OnTriggerEnter(Collider other)
    {
        IBounce iBounce = other.gameObject.GetComponent<IBounce>();

        if (iBounce != null) iBounce.Bounce(gameObject, null);     
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

    public IEnumerator Rumble(float _leftSpeed, float _rightSpeed, float _duration)
    {
        gamepad.SetMotorSpeeds(_leftSpeed, _rightSpeed);
        yield return new WaitForSeconds(_duration);
        gamepad.SetMotorSpeeds(0f, 0f);
    }
}
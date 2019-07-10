using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class Player : MonoBehaviour
{
    #region Movement
    private float targetRotation;
    protected float speedSmooothTime = 0.075f, animationSpeedPercent;
    [SerializeField] float moveSpeed = 0f, turnSmooth = 0f, powerUpSpeed = 0f;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    private Vector2 inputDirection;
    private Vector2 inputAiming;
    #endregion

    public bool speedPU;
    private bool canMove = false;

    byte dashCount = 0;
    bool canDash = false;
    [SerializeField] float dashForce = 7f;
    public event System.Action<Player> OnDashExecuted = null;

    private byte id;
    string prefabName;

    [SerializeField]
    private GameObject avatar;

    [SerializeField]
    private float throwForce = 0f;

    private GameObject player;
    private new SphereCollider collider;
    private Rigidbody m_Rigidbody;
    private Vector3 spawnPoint;

    private Animator m_Animator;
    private AnimatorOverrideController animatorWNoBomb;
    [SerializeField] AnimatorOverrideController animatorWBomb = null;

    private bool hasBomb;

    private Transform catapult;
    private SkinnedMeshRenderer[] avatarSkinnedMeshRenderers = new SkinnedMeshRenderer[0];

    private bool throwing;
    Bomb m_Bomb = null;

    private Gamepad gamepad;

    public bool CanMove { get => canMove; set => canMove = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public Vector2 InputAiming { get => inputAiming; private set => inputAiming = value; }
    public GameObject Avatar { get => avatar; set => avatar = value; }
    public float TurnSmooth { get => turnSmooth; private set => turnSmooth = value; }
    public byte Id { get => id; set => id = value; }
    public Animator Animator { get => m_Animator; set => m_Animator = value; }
    public string PrefabName { get => prefabName; set => prefabName = value; }
    public SphereCollider Collider { get => collider; private set => collider = value; }
    public Transform Catapult { get => catapult; private set => catapult = value; }
    public Rigidbody Rigidbody { get => m_Rigidbody; private set => m_Rigidbody = value; }
    public SkinnedMeshRenderer[] AvatarSkinnedMeshRenderers { get => avatarSkinnedMeshRenderers; set => avatarSkinnedMeshRenderers = value; }
    public byte DashCount { get => dashCount; private set => dashCount = value; }

    private void Start()
    {
        if (animatorWNoBomb == null)
        {
            animatorWNoBomb = new AnimatorOverrideController(m_Animator.runtimeAnimatorController);
        }

        Rigidbody = GetComponent<Rigidbody>();
        GameManager.Manager.Director.stopped += LetMove;
        GameManager.Manager.OnCorrectPassBomb += IncreaseDashCounter;
        Bomb.onExplode += ResetPlayer;
        gamepad = (Gamepad)GetComponent<PlayerInput>().devices[0];
    }

    private void IncreaseDashCounter(Player _player)
    {
        if (_player != this) return;

        DashCount = (DashCount <= GameManager.numberToReachDash) ? DashCount += 1 : DashCount = GameManager.numberToReachDash;

        if (DashCount == GameManager.numberToReachDash) canDash = true;
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
        m_Animator.SetInteger("PodiumState", _podiumState);
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
        StopAllCoroutines();
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in AvatarSkinnedMeshRenderers)
        {
            skinnedMeshRenderer.material.shader = GameManager.Manager.DefaultShader;
        }
        m_Animator.runtimeAnimatorController = animatorWNoBomb;
    }

    public void SetOverrideAnimator(bool _hasBomb)
    {
        if (!_hasBomb)
        {
            m_Animator.runtimeAnimatorController = animatorWNoBomb;
        }
        else
        {
            if (m_Animator.runtimeAnimatorController != animatorWBomb)
            {
                m_Animator.runtimeAnimatorController = animatorWBomb;
            }
        }
    }

    private void LetMove(PlayableDirector _obj) => canMove = true;

    void Update()
    {
        if (canMove)
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
        if (canMove)
        {
            Rigidbody.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
        }
    }

    public void Initialize()
    {
        player = Instantiate(avatar, transform.position, transform.rotation);
        player.transform.SetParent(transform);

        Bummie cBummie = GetComponentInChildren<Bummie>();

        if (cBummie != null)
        {
            Animator = cBummie.Animator;
            Catapult = cBummie.Catapult;
            AvatarSkinnedMeshRenderers = cBummie.SkinnedMeshRenderers;
        }
        else
        {
            Debug.LogError("Bummie component was not found");
        }

        foreach (SphereCollider sphere in cBummie.SphereColliders)
        {
            if (sphere.isTrigger)
            {
                collider = sphere;
            }
        }

        HasBomb = false;
    }

    public void Throw()
    {
        m_Bomb = Catapult.GetComponentInChildren<Bomb>();

        if (HasBomb && CanMove && !throwing)
        {
            SetOverrideAnimator(false);
            StopCoroutine(SyncThrowAnim());
            if (m_Bomb != null) StartCoroutine(SyncThrowAnim());
        }
    }

    private void Dash()
    {
        if (canDash && CanMove)
        {
            Rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
            canDash = false;
            dashCount = 0;
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

        m_Bomb.RigidBody.isKinematic = false;
        m_Bomb.transform.SetParent(null);

        if (InputAiming != Vector2.zero)
        {
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * aiming;
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|BombLaunch"))
               m_Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse); 
        }
        else
        {
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * transform.forward;
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|BombLaunch"))
                m_Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse); 
        }

        m_Bomb.Collider.enabled = true;

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bombThrow, 0.7f);

        float prob = Random.Range(0f, 1f);
        if (prob < 0.33f) AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cThrow, 0.7f);

        HasBomb = false;
        throwing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PowerUp powerUpCollisioned = collision.gameObject.GetComponent<PowerUp>();
        Player otherPlayer = collision.gameObject.GetComponentInParent<Player>();

        if (otherPlayer == null)
        {
            if (powerUpCollisioned != null && GetComponent<PowerUp>() == null)
            {
                if (!HasBomb)
                {
                    IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
                    powerUp.PickPowerUp(this);
                }
                else
                {
                    gameObject.AddComponent<Velocity>();
                }
                collision.gameObject.SetActive(false);
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player collisionedPlayer = other.GetComponentInParent<Player>();
        Bomb collisionedBomb = other.GetComponent<Bomb>();

        if (collisionedBomb != null && collisionedBomb.transform.parent == null)
        {
            GameManager.Manager.PassBomb(this, collisionedBomb);
            Animator.SetTrigger("Reception");
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bombReception, 0.6f);
            SetOverrideAnimator(true);
        }
        else if (collisionedPlayer != null)
        {
            if (collisionedPlayer.HasBomb && collisionedPlayer.CanMove)
            {
                GameManager.Manager.PassBomb(this, collisionedPlayer, collisionedPlayer.Catapult.GetComponentInChildren<Bomb>());
                Animator.SetTrigger("Reception");
                collisionedPlayer.SetOverrideAnimator(false);
                SetOverrideAnimator(true);
            }
        }
    }

    public void Stun(bool _stun)
    {
        Animator.SetBool("CanMove", !_stun);
        if (_stun)
        {
            Animator.SetTrigger("Stun");
        }
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 0.6f, _stun);
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cStun, 1f);
        canMove = !_stun;
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
        canMove = false;

        if (_animStun)
        {
            Animator.SetTrigger("Stun"); 
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 0.6f, true);
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cStun, 1f);
        }

        yield return new WaitForSeconds(_duration);
        Animator.SetBool("CanMove", true);

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 0.6f, false); 
        canMove = true;
    }

    public IEnumerator Rumble(float _leftSpeed, float _rightSpeed, float _duration)
    {
        gamepad.SetMotorSpeeds(_leftSpeed, _rightSpeed);
        yield return new WaitForSeconds(_duration);
        gamepad.SetMotorSpeeds(0f, 0f);
    }
}
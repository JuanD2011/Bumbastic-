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
    [SerializeField] float moveSpeed, turnSmooth, powerUpSpeed;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    private Vector2 inputDirection;
    private Vector2 inputAiming;
    #endregion

    public bool speedPU;
    private bool canMove = false;

    private byte id;
    string prefabName;

    [SerializeField]
    private GameObject avatar;

    [SerializeField]
    private float throwForce;

    private GameObject player;
    private SphereCollider collider;
    private Rigidbody m_Rigidbody;
    private Vector3 spawnPoint;
    private Controls controls;

    //[SerializeField]

    private Animator m_Animator;
    private AnimatorOverrideController animatorWNoBomb;
    [SerializeField] AnimatorOverrideController animatorWBomb;

    private bool hasBomb;

    private Transform catapult;

    private bool throwing;

    public bool CanMove { get => canMove; set => canMove = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public Vector2 InputAiming { get => inputAiming; private set => inputAiming = value; }
    public Controls Controls { private get => controls; set => controls = value; }
    public GameObject Avatar { get => avatar; set => avatar = value; }
    public float TurnSmooth { get => turnSmooth; private set => turnSmooth = value; }
    public byte Id { get => id; set => id = value; }
    public Animator Animator { get => m_Animator; set => m_Animator = value; }
    public string PrefabName { get => prefabName; set => prefabName = value; }
    public SphereCollider Collider { get => collider; private set => collider = value; }
    public Transform Catapult { get => catapult; private set => catapult = value; }
    public Rigidbody Rigidbody { get => m_Rigidbody; private set => m_Rigidbody = value; }

    private void Start()
    {
        if (animatorWNoBomb == null)
        {
            animatorWNoBomb = new AnimatorOverrideController(m_Animator.runtimeAnimatorController);
        }

        Rigidbody = GetComponent<Rigidbody>();
        GameManager.Manager.Director.stopped += LetMove;
        Bomb.OnExplode += ResetPlayer;
    }

    public void OnMove(InputValue context)
    {
        inputDirection = context.Get<Vector2>();
    }

    public void OnAim(InputValue context)
    {
        inputAiming = context.Get<Vector2>();
    }

    public void OnThrow()
    {
        Throw();
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
        m_Animator.runtimeAnimatorController = animatorWNoBomb;
        hasBomb = false;
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
        Move();

        //if (canMove)
        //{
        //    inputDirection = new Vector2(Input.GetAxis(Controls.ljoystickHorizontal), Input.GetAxis(Controls.ljoystickVertical));
        //    inputAiming = new Vector2(Input.GetAxis(Controls.rjoystickHorizontal), Input.GetAxis(Controls.rjoystickVertical));
        //    Move(); 
        //}
        //if (controls.rightButtonTrigger != KeyCode.None)
        //{
        //    if (Input.GetKeyDown(controls.rightButtonTrigger) && !throwing)
        //    {
        //        Throw();
        //    }
        //}
        //else if (controls.rightAxisTrigger != "" && !throwing)
        //{
        //    if (Input.GetAxis(controls.rightAxisTrigger) == 1f)
        //    {
        //        Throw();
        //    }
        //}

		//if (Input.GetKeyDown(Controls.startButton))
  //      {
  //          PlayerMenu.OnStartButton?.Invoke(Id);
  //      }
    }


    private void Move()
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
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
            animationSpeedPercent = ((speedPU) ? 1 : 0.5f) * inputDirection.magnitude;
            Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime); 
        }
    }

    public void Initialize()
    {
        player = Instantiate(avatar, transform.position, transform.rotation);
        player.transform.SetParent(transform);
        Animator = GetComponentInChildren<Animator>();
        HasBomb = false;
        Catapult = GetComponentInChildren<Bummie>().Catapult;
        SphereCollider[] colliders = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider sphere in colliders)
        {
            if (sphere.isTrigger)
            {
                collider = sphere;
            }
        }
    }

    public void Throw()
    {
        if (HasBomb && CanMove && !throwing)
        {
            Animator.SetTrigger("Throw");
            SetOverrideAnimator(false);
            StartCoroutine(SyncThrowAnim(InputAiming));
        }
    }

    IEnumerator SyncThrowAnim(Vector2 _InputAiming)
    {
        throwing = true;
        
        float elapsedTime = 0f;

        Quaternion initialRotation = transform.rotation;
        Vector3 aiming = new Vector3(InputAiming.normalized.x, 0, InputAiming.normalized.y);
    
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

        HotPotatoManager.HotPotato.Bomb.transform.parent = null;
        HotPotatoManager.HotPotato.Bomb.RigidBody.isKinematic = false;
        HotPotatoManager.HotPotato.Bomb.Collider.enabled = true;

        if (_InputAiming != Vector2.zero)
        {         
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * aiming;
            HotPotatoManager.HotPotato.Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        else
        {
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * transform.forward;
            HotPotatoManager.HotPotato.Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bombThrow, 0.7f);
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cThrow, 0.7f);

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
            GameManager.Manager.PassBomb(this);
            Animator.SetTrigger("Reception");
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bombReception, 0.6f);
            SetOverrideAnimator(true);
        }
        else if (collisionedPlayer != null)
        {
            if (collisionedPlayer.HasBomb && collisionedPlayer.CanMove)
            {
                GameManager.Manager.PassBomb(this, collisionedPlayer);
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
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, _stun, 0.6f);
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
        Animator.SetBool("CanMove", false);
        inputDirection = Vector2.zero;
        canMove = false;

        if (_animStun)
        {
            Animator.SetTrigger("Stun"); 
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, true, 0.6f);
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cStun, 1f);
        }

        yield return new WaitForSeconds(_duration);
        Animator.SetBool("CanMove", true);

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, false, 0.6f); 
        canMove = true;
    }
}
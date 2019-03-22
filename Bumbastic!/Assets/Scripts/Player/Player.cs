using System;
using UnityEngine;
using UnityEngine.Playables;

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

    private bool speedPU;
    private bool canMove = false;

    private byte id;

    [SerializeField]
    private GameObject avatar;

    [SerializeField]
    private float throwForce;

    private GameObject player;
    private Animator m_Animator;
    private Vector3 spawnPoint;
    private Controls controls;

    private bool hasBomb;

    public bool SpeedPU { get => speedPU; set => speedPU = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public Vector2 InputAiming { get => inputAiming; set => inputAiming = value; }
    public Controls Controls { get => controls; set => controls = value; }
    public GameObject Avatar { set => avatar = value; }
    public float TurnSmooth { get => turnSmooth; private set => turnSmooth = value; }
    public byte Id { get => id; set => id = value; }

    private void Start() => GameManager.manager.Director.stopped += LetMove;

    private void LetMove(PlayableDirector obj) => canMove = true;

    public static PlayerMenu.ButtonsDelegate OnStartInGame;

    void Update()
    {
        if (canMove)
        {
            inputDirection = new Vector2(Input.GetAxis(controls.ljoystickHorizontal), Input.GetAxis(controls.ljoystickVertical));
            inputAiming = new Vector2(Input.GetAxis(controls.rjoystickHorizontal), Input.GetAxis(controls.rjoystickVertical));
            Move(); 
        }
        if (controls.rightTrigger != null)
        {
            if (Input.GetAxis(controls.rightTrigger) == 1f)
            {
				Debug.Log("Throw");
                Throw();
            } 
        }

        if (Input.GetKeyDown(controls.startButton))
        {
            OnStartInGame?.Invoke();
        }

    }

    public void Throw()
    {
        if (HasBomb)
        {
            GameManager.manager.Bomb.RigidBody.AddForce(transform.forward * throwForce); 
        }
    }

    private void Move()
    {
        if (inputDirection != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, TurnSmooth);
        }

        targetSpeed = ((SpeedPU) ? powerUpSpeed : moveSpeed) * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmooothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        animationSpeedPercent = ((SpeedPU) ? 1 : 0.5f) * inputDirection.magnitude;
        m_Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime);
    }

    public void Initialize()
    {
        player = Instantiate(avatar, transform.position, transform.rotation);
        player.transform.SetParent(transform);
        m_Animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PowerUp>() != null && !HasBomb && GetComponent<PowerUp>() == null)
        {
            IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
            powerUp.PickPowerUp(GetComponent<Player>());
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Bomb bomb = other.gameObject.GetComponent<Bomb>();
        //This is when they throw the bomb
        if (bomb != null && !HasBomb)
        {
			HasBomb = false;
            PassBomb();
        }
        //When a player touches another player
        else if (bomb != null && !HasBomb)
        {
            other.gameObject.GetComponent<Player>().HasBomb = false;
            PassBomb();
        }
    }

    private void PassBomb()
    {
        //m_Animator.SetTrigger("Reception");
        GameManager.manager.BombHolder = this;
        GameManager.manager.BombHolder.HasBomb = true;
        GameManager.manager.Bomb.transform.parent = null;
        GameManager.manager.Bomb.transform.SetParent(GameManager.manager.BombHolder.transform);
        GameManager.manager.Bomb.transform.position = GameManager.manager.BombHolder.transform.GetChild(1).transform.position;
        GameManager.manager.Bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}

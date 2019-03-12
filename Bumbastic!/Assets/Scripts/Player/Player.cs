using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool speedPU;
    private bool canMove;

    private PlayerInput playerInput;
    private byte id;

    [SerializeField]
    private InputManager controls;

    [SerializeField]
    private GameObject avatar;
    private Animator m_Animator;
    private Vector3 spawnPoint;

    private bool hasBomb;

    public bool SpeedPU { get => speedPU; set => speedPU = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public GameObject Avatar { get => avatar; set => avatar = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public Vector2 InputAiming { get => inputAiming; set => inputAiming = value; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (canMove)
        {
            Move(); 
        }
    }

    public void Throw()
    {
        Debug.Log("Throw");
    }

    private void Move()
    {
        if (inputDirection != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmooth);
        }

        targetSpeed = ((SpeedPU) ? powerUpSpeed : moveSpeed) * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmooothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        animationSpeedPercent = ((SpeedPU) ? 1 : 0.5f) * inputDirection.magnitude;
        m_Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime);
    }

    public void OnThrowing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (HasBomb)
            {
                Throw();
            }
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputDirection = context.ReadValue<Vector2>().normalized;
        }
        else if (context.cancelled)
        {
            inputDirection = Vector2.zero;
        }
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputAiming = context.ReadValue<Vector2>().normalized;
        }
        else if (context.cancelled)
        {
            InputAiming = Vector2.zero;
        }
    }

    public void OnDeviceLost()
    {
        Destroy(this.gameObject);
    }

    public void Initialize()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        id = (byte)playerInput.playerIndex;
        playerInput.SwitchActions("Player");
        m_Animator = GetComponentInChildren<Animator>();
        canMove = false;
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
        //This is when they throw the bomb
        if (other.gameObject.GetComponent<Bomb>() != null && !HasBomb)
        {
            PassBomb();
        }
        //When a player touches another player
        else if (other.gameObject.GetComponentInChildren<Bomb>() != null && !HasBomb)
        {
            other.gameObject.GetComponent<Player>().HasBomb = false;
            PassBomb();
        }
    }

    private void PassBomb()
    {
        m_Animator.SetTrigger("Reception");
        GameManager.manager.BombHolder = this;
        GameManager.manager.BombHolder.HasBomb = true;
        GameManager.manager.Bomb.transform.parent = null;
        GameManager.manager.Bomb.transform.SetParent(GameManager.manager.BombHolder.transform);
        GameManager.manager.Bomb.transform.position = GameManager.manager.BombHolder.transform.GetChild(1).transform.position;
        GameManager.manager.Bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}

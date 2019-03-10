﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bummie : MonoBehaviour
{
    #region Movement
    private float targetRotation;
    protected float speedSmooothTime = 0.075f, animationSpeedPercent;
    [SerializeField] float moveSpeed, turnSmooth, powerUpSpeed;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    private Vector2 input, inputDirection, inputAiming, inputAim;
    Vector3 movement;

    LineRenderer m_AimPath;
    #endregion

    #region Move or bum
    float timeBeforeBum = 5f;
    float elapsedTime = 0f;
    bool exploded = false;
    #endregion

    #region Bomb
    [SerializeField] private bool hasBomb = false;
    float throwForce = 20f;
    #endregion

    [SerializeField] private bool speedPU;

    bool canMove;
    Animator m_Animator;
    private Vector3 spawnPoint;

    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public bool SpeedPU { get => speedPU; set => speedPU = value; }
    public bool CanMove { private get => canMove; set => canMove = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

    public delegate void DelBummie();
    public DelBummie OnDisableJoystick;


    private void Start()
    {
        canMove = false;

        m_Animator = GetComponentInChildren<Animator>();

        //GameManager.instance.Director.stopped += Director_stopped;

        m_AimPath = transform.GetChild(2).GetComponent<LineRenderer>();
        m_AimPath.SetPosition(1, new Vector3(0, 0, throwForce/1.8f));
    }

    private void Director_stopped(UnityEngine.Playables.PlayableDirector obj)
    {
        canMove = true;
    }

    private void ResetTime()
    {
        elapsedTime = 0f;
    }

    private void SetPath(bool _show)
    {
        m_AimPath.gameObject.SetActive(_show);
    }

    private void Update()
    {

        //Move Or Bum
        //if (!joystickMovement.IsMoving && !exploded)
        //{
        //    elapsedTime += Time.deltaTime;
        //    if (elapsedTime > timeBeforeBum)
        //    {
        //        exploded = true;
        //        EZCameraShake.CameraShaker.Instance.ShakeOnce(4f, 2.5f, 0.1f, 1f);
        //        gameObject.SetActive(false);
        //        GameManager.instance.PlayersInGame.Remove(this);
        //        if (hasBomb)
        //            GameManager.instance.GiveBomb();
        //    }
        //}
        //else if (joystickMovement.Direction.magnitude >= 0.2f && !isMoving){
        //    elapsedTime = 0f;
        //    isMoving = true;
        //    print(elapsedTime);
        //}
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        inputDirection = input.normalized;

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

    void RPC_ThrowBomb()
    {
        //GameManager.instance.bomb.transform.parent = null;
        //GameManager.instance.bomb.RigidBody.constraints = RigidbodyConstraints.None;
        //GameManager.instance.bomb.RigidBody.velocity = GameManager.instance.bombHolder.transform.forward * throwForce;
        //GameManager.instance.bomb.RigidBody.position += GameManager.instance.bomb.RigidBody.velocity;
        //hasBomb = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PowerUp>() != null && !hasBomb && GetComponent<PowerUp>() == null)
        { 
            IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
            powerUp.PickPowerUp(GetComponent<Bummie>());
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //This is when they throw the bomb
        if (other.gameObject.GetComponent<Bomb>() != null && !hasBomb)
        {
            PassBomb();
        }
        //When a player touches another player
        else if (other.gameObject.GetComponentInChildren<Bomb>() != null && !hasBomb)
        {
            other.gameObject.GetComponent<Bummie>().HasBomb = false;
            PassBomb();
        }
    }

    private void PassBomb()
    {
        m_Animator.SetTrigger("Reception");
        //GameManager.instance.bombHolder = this;
    }

    public IEnumerator CantMove(float _time)
    {
        WaitForSeconds wait = new WaitForSeconds(_time);
        CanMove = false;
        yield return wait;
        CanMove = true;
    }

    void RPC_SyncBomb()
    {
        //foreach (Bummie bummie in GameManager.instance.PlayersInGame)
        //{
        //    bummie.HasBomb = false;
        //}
        //GameManager.instance.bombHolder.HasBomb = true;
        //GameManager.instance.bomb.transform.parent = null;
        //GameManager.instance.bomb.transform.SetParent(GameManager.instance.bombHolder.transform);
        //GameManager.instance.bomb.transform.position = GameManager.instance.bombHolder.transform.GetChild(1).transform.position;
        //GameManager.instance.bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void NewRound()
    {
        transform.position = spawnPoint;
        canMove = false;   
    }
}
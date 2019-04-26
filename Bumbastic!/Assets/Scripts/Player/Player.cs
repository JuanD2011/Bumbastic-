﻿using System;
using System.Collections;
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
    string prefabName;

    [SerializeField]
    private GameObject avatar;

    [SerializeField]
    private float throwForce;

    private GameObject player;
    private SphereCollider collider;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private Vector3 spawnPoint;
    private Controls controls;

    private bool hasBomb;

    private Transform catapult;

    private bool throwing;

    public bool SpeedPU { private get => speedPU; set => speedPU = value; }
    public bool CanMove { private get => canMove; set => canMove = value; }
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
        Rigidbody = GetComponent<Rigidbody>();
        GameManager.Manager.Director.stopped += LetMove;
    }

    private void LetMove(PlayableDirector _obj) => canMove = true;

    void Update()
    {
        if (canMove)
        {
            inputDirection = new Vector2(Input.GetAxis(Controls.ljoystickHorizontal), Input.GetAxis(Controls.ljoystickVertical));
            inputAiming = new Vector2(Input.GetAxis(Controls.rjoystickHorizontal), Input.GetAxis(Controls.rjoystickVertical));
            Move(); 
        }
        if (controls.rightButtonTrigger != KeyCode.None)
        {
            if (Input.GetKeyDown(controls.rightButtonTrigger) && !throwing)
            {
                Throw();
            }
        }
        else if (controls.rightAxisTrigger != "" && !throwing)
        {
            if (Input.GetAxis(controls.rightAxisTrigger) == 1f)
            {
                Throw();
            }
        }

		if (Input.GetKeyDown(Controls.startButton))
        {
            PlayerMenu.OnStartButton?.Invoke(Id);
        }
    }


    private void Move()
    {
        if (inputDirection != Vector2.zero && !throwing)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, TurnSmooth);
        }

        targetSpeed = ((SpeedPU) ? powerUpSpeed : moveSpeed) * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmooothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        animationSpeedPercent = ((SpeedPU) ? 1 : 0.5f) * inputDirection.magnitude;
        Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime);
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
        if (HasBomb)
        {
            Animator.SetTrigger("Throw");

            StartCoroutine(SyncThrowAnim(InputAiming));
        }
    }

	private IEnumerator RotateWhileThrowing(AnimatorStateInfo stateInfo)
	{
		float elapsedTime = 0;

		while (elapsedTime < 0.5f)
		{
			elapsedTime = stateInfo.normalizedTime;

			Vector3 aiming = new Vector3(-InputAiming.normalized.y, 0, InputAiming.normalized.x);
			Vector3 currentRotation = aiming - transform.eulerAngles;
			Quaternion newRotation = Quaternion.LookRotation(currentRotation);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, elapsedTime / 1);
			yield return null;
		}

		targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
		transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, TurnSmooth);

		throwing = false;
	}

    IEnumerator SyncThrowAnim(Vector2 _InputAiming)
    {
        throwing = true;

        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|BombLaunch"));


        float elapsedTime = 0f;

        AnimatorStateInfo animatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
		StartCoroutine(RotateWhileThrowing(animatorStateInfo));

        while (elapsedTime < 0.1f)
        {
            elapsedTime = animatorStateInfo.normalizedTime;
            yield return null;
        }

        HotPotatoManager.HotPotato.Bomb.transform.parent = null;
        HotPotatoManager.HotPotato.Bomb.RigidBody.isKinematic = false;

        if (_InputAiming != Vector2.zero)
        {
            Vector3 aiming = new Vector3(-InputAiming.normalized.y, 0, InputAiming.normalized.x);
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * aiming;
            HotPotatoManager.HotPotato.Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        else
        {
            Vector3 direction = Quaternion.AngleAxis(10, transform.right) * transform.forward;
            HotPotatoManager.HotPotato.Bomb.RigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        HasBomb = false;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        PowerUp powerUpCollisioned = collision.gameObject.GetComponent<PowerUp>();
        Player otherPlayer = collision.gameObject.GetComponent<Player>();

        if (otherPlayer == null)
        {
            if (powerUpCollisioned != null && GetComponent<PowerUp>() == null)
            {
                if (!HasBomb)
                {
                    IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
                    powerUp.PickPowerUp(GetComponent<Player>());
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
        if (other.GetComponent<Player>() != null)
        {
            if (other.GetComponent<Player>().HasBomb)
            {
                GameManager.Manager.PassBomb(this, other.GetComponent<Player>());
                Animator.SetTrigger("Reception");
            }
        }
        else if (other.GetComponent<Bomb>() != null)
        {
            GameManager.Manager.PassBomb(this);
            Animator.SetTrigger("Reception");
        }
    }
}
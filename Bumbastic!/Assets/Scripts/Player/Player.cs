using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class Player : MonoBehaviour
{
    private byte id;

    [SerializeField]
    private InputManager controls;

    #region Movement
    private float targetRotation;
    protected float speedSmooothTime = 0.075f, animationSpeedPercent;
    [SerializeField] float moveSpeed, turnSmooth, powerUpSpeed;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    private Vector2 inputDirection, inputAiming, inputAim;
    #endregion

    private bool speedPU;

    public bool SpeedPU { get => speedPU; set => speedPU = value; }
    public byte Id { get => id; set => id = value; }
    public InputDevice Device { get => device; set => device = value; }

    private Animator m_Animator;

    InputDevice device;

    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Debug.Log(inputDirection);
        Move();
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
            Throw();
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
}

﻿using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;

    private bool exploded = false;
    bool canCount = false;

    private float timer;
    private Rigidbody m_rigidBody;
    private Collider m_Collider;

    protected Animator m_Animator;
    protected AnimationCurve animationCurve = new AnimationCurve();

    private float gravity = 16f;
    protected float elapsedTime = 0f;

    protected ParticleModication cParticleModification = null;

    public static Action OnExplode;

    public static Action<float> OnArmed;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }
    public bool Exploded { get => exploded; set => exploded = value; }
    public bool CanCount { get => canCount; protected set => canCount = value; }
    public Collider Collider { get => m_Collider; private set => m_Collider = value; }

    protected virtual void Awake()
    {
        cParticleModification = GetComponentInChildren<ParticleModication>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
    }

    public void SetAnimationKeys()
    {
        animationCurve.AddKey(0, 0f);
        animationCurve.AddKey(timer, 1f);
        OnArmed?.Invoke(Timer);
    }

    protected virtual void FixedUpdate()
    {
        RigidBody.AddForce(-Vector3.up * gravity);
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(elapsedTime) * speed;

        if (!Exploded)
        {
            if (transform.parent != null)
            {
                if (!canCount)
                {
                    canCount = true;
                }
                elapsedTime += Time.deltaTime;
            }
            else
            {
                if (canCount)
                {
                    canCount = false; 
                }
            }
        }

        if (elapsedTime > Timer && !Exploded)
        {
            Explode();
        }
    }

    protected void Explode()
    {
        OnExplode?.Invoke();
        transform.SetParent(null);
        elapsedTime = 0;
        Exploded = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bomb, 0.7f);
        CameraShake.instance.OnShakeDuration?.Invoke(0.4f, 6f, 1.2f);
        RigidBody.isKinematic = false;
        Collider.enabled = false;
        cParticleModification.Execute();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameModeDataBase.IsCurrentBasesGame()) return;

        if (collision.transform.CompareTag("Floor") && !Exploded && transform.parent == null)
        {
            HotPotatoManager.HotPotato.PassBomb(); 
        } 
    }
}

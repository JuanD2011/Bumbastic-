﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;

    protected Animator m_Animator;
    protected AnimationCurve animationCurve = new AnimationCurve();

    private float gravity = 16f;
    protected float elapsedTime = 0f;
    private bool aboutToExplode = false;

    protected ParticleModication cParticleModification = null;

    public float Timer { get; set; } = 0f;
    public Rigidbody RigidBody { get; set; } = null;
    public bool Exploded { get; set; }
    public bool CanCount { get; protected set; }
    public Collider Collider { get; private set; }

    public static Action OnExplode;
    public static Action<float> OnArmed;
    public static event Action<Bomb> OnFloorCollision;
    public static event Action<Bomb> OnAboutToExplode;

    protected virtual void Awake()
    {
        cParticleModification = GetComponentInChildren<ParticleModication>();
        RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
    }

    public void SetAnimationKeys()
    {
        animationCurve.AddKey(0, 0f);
        animationCurve.AddKey(Timer, 1f);
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
                if (!CanCount)
                {
                    CanCount = true;
                }
                elapsedTime += Time.deltaTime;
            }
            else
            {
                if (CanCount)
                {
                    CanCount = false;
                }
            }
        }

        if (!Exploded && !aboutToExplode)
        {
            if (elapsedTime > Timer - 5f)
            {
                aboutToExplode = true;
                StartCoroutine(AboutToExplode());
            }
        }

        if (elapsedTime > Timer && !Exploded)
        {
            Explode();
        }
    }

    protected virtual void Explode()
    {
        transform.SetParent(null);
        elapsedTime = 0;
        Exploded = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bomb, 0.7f);
        CameraShake.instance.OnShakeDuration?.Invoke(0.4f, 6f, 1.2f);
        RigidBody.isKinematic = false;
        Collider.enabled = false;
        cParticleModification.Execute();
        OnExplode?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameModeDataBase.IsCurrentBasesGame()) return;

        if (collision.transform.CompareTag("Floor") && !Exploded && transform.parent == null)
        {
            OnFloorCollision?.Invoke(this);
        }
    }

    private IEnumerator AboutToExplode()
    {
        byte counter = 0;
        while (!Exploded || counter < 7)
        {
            if (counter < 2) yield return new WaitForSeconds(1f);
            else if (counter < 4) yield return new WaitForSeconds(0.5f);
            else yield return new WaitForSeconds(0.25f);
            OnAboutToExplode?.Invoke(this);
            counter++;
        }
        aboutToExplode = false;
    }
}

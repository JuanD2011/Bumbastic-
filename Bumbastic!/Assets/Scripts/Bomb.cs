﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Bomb : MonoBehaviour
{
    private float t = 0f;
    private bool exploded = false;

    private float timer;
    private Rigidbody m_rigidBody;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }

    private Animator m_Animator;

    private AnimationCurve animationCurve = new AnimationCurve();
    private float speed = 4f;

    public delegate void BombDelegate();
    public static BombDelegate OnExplode;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetKeys();
    }

    private void SetKeys()
    {
        animationCurve.AddKey(0, 0f);
        animationCurve.AddKey(timer, 1f);
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(t) * speed;

        if (!exploded && transform.parent != null)
        {
            t += Time.deltaTime;
        }

        if (t > Timer && !exploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;
        CameraShake.instance.OnShake(0.4f, 6f, 1.2f);
        RigidBody.constraints = RigidbodyConstraints.None;
        GameObject parent = transform.parent.gameObject;
        transform.SetParent(null);
        parent.SetActive(false);
        gameObject.SetActive(false);
        OnExplode?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}

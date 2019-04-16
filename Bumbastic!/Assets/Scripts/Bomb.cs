using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Bomb : MonoBehaviour
{
    private float t = 0f;
    private bool exploded = false;

    private float timer;
    private Rigidbody m_rigidBody;

    public float Timer { private get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }
    public bool Exploded { private get => exploded; set => exploded = value; }

    private Animator m_Animator;

    private AnimationCurve animationCurve = new AnimationCurve();
    private float speed = 4f;

    public delegate void BombDelegate();
    public static BombDelegate OnExplode;
    [SerializeField] private float gravity = 12f;

    private void Awake()
    {
        OnExplode = null;
    }

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetAnimationKeys();
    }

    private void SetAnimationKeys()
    {
        animationCurve.AddKey(0, 0f);
        animationCurve.AddKey(timer, 1f);
    }

    private void Update()
    {
        RigidBody.AddForce(-Vector3.up * gravity);

        m_Animator.speed = animationCurve.Evaluate(t) * speed;

        if (!Exploded && transform.parent != null)
        {
            t += Time.deltaTime;
        }

        if (t > Timer && !Exploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        t = 0;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.audioClips.bomb, AudioType.SFx);
        }
        transform.SetParent(null);
        CameraShake.instance.OnShake?.Invoke(0.4f, 6f, 1.2f);
        Exploded = true;
        RigidBody.isKinematic = false;
        OnExplode?.Invoke();//ParticleModification, GameManager, FloorManager hears it
        //gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor") && !Exploded)
        {
            GameManager.Manager.PassBomb();
        }
    }
}

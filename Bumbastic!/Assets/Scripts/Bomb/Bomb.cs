using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    private float t = 0f;
    private bool exploded = false;
    bool canCount = false;

    private float timer;
    private Rigidbody m_rigidBody;
    private Collider m_Collider;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }
    public bool Exploded { get => exploded; set => exploded = value; }
    public bool CanCount { get => canCount; private set => canCount = value; }
    public Collider Collider { get => m_Collider; private set => m_Collider = value; }

    private Animator m_Animator;

    private AnimationCurve animationCurve = new AnimationCurve();
    [SerializeField] private float speed = 4f;

    public static Action onExplode;
    private float gravity = 16f;

    private void Awake()
    {
        onExplode = null;
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();

        HotPotatoManager.HotPotato.OnBombArmed += SetAnimationKeys;
    }

    private void SetAnimationKeys()
    {
        animationCurve.AddKey(0, 0f);
        animationCurve.AddKey(timer, 1f);
    }

    private void FixedUpdate()
    {
        RigidBody.AddForce(-Vector3.up * gravity);
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(t) * speed;

        if (!Exploded)
        {
            if (transform.parent != null)
            {
                if (!canCount)
                {
                    canCount = true;
                }
                t += Time.deltaTime;
            }
            else
            {
                if (canCount)
                {
                    canCount = false; 
                }
            }
        }

        if (t > Timer && !Exploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        t = 0;
        Exploded = true;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bomb, 0.7f);
        }
        transform.SetParent(null);
        CameraShake.instance.OnShakeDuration?.Invoke(0.4f, 6f, 1.2f);
        RigidBody.isKinematic = false;
        Collider.enabled = true;
        onExplode?.Invoke();//ParticleModification, GameManager, FloorManager hears it
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor") && !Exploded && transform.parent == null)
        {
            GameManager.Manager.PassBomb();
        }
    }
}

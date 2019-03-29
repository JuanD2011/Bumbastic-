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

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }
    public bool Exploded { get => exploded; set => exploded = value; }

    private Animator m_Animator;

    private AnimationCurve animationCurve = new AnimationCurve();
    private float speed = 4f;

    public delegate void BombDelegate(Player _player);
    public static BombDelegate OnExplode;

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
        GameObject parent = transform.parent.gameObject;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.audioClips.bomb, AudioType.SFx);
        }

        CameraShake.instance.OnShake?.Invoke(0.4f, 6f, 1.2f);

        Exploded = true;
        RigidBody.constraints = RigidbodyConstraints.None;
        OnExplode?.Invoke(parent.GetComponent<Player>());//ParticleModification hears it
        transform.SetParent(null);
        //gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            GameManager.manager.Bomb.transform.SetParent(GameManager.manager.BombHolder.transform);
            GameManager.manager.Bomb.transform.position = GameManager.manager.BombHolder.transform.GetChild(1).transform.position;
            RigidBody.constraints = RigidbodyConstraints.FreezeAll;
            GameManager.manager.BombHolder.HasBomb = true;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasesBomb : Bomb
{
    [SerializeField] float minTime = 10f, maxTime = 18f;

    public static event System.Action<Player> OnBasesBombExplode;

    protected override void Awake()
    {
        base.Awake();
        CanCount = true;
        Timer = Random.Range(minTime, maxTime);
    }

    private void OnEnable()
    {
        Collider.enabled = true;
        Exploded = false;
    }

    private void Start()
    {
        cParticleModification.OnComplete += () => gameObject.SetActive(false);
        SetAnimationKeys();
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(elapsedTime) * speed;

        if (!Exploded)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime > Timer && !Exploded)
        {
            Explode();
        }
    }

    protected override void Explode()
    {
        OnBasesBombExplode?.Invoke(GetComponentInParent<Player>());
        base.Explode();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

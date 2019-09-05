using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveRainBomb : Bomb
{
    [SerializeField]
    private float minExplosionTime = 0f, maxExplosionTime = 0f, explosionRadius = 0f, explosionForce = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        cParticleModification.OnComplete += () => gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Exploded = false;
        CanCount = true;

        Timer = Random.Range(minExplosionTime, maxExplosionTime);
        SetAnimationKeys();
        elapsedTime = 0f;
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(elapsedTime) * speed;

        if (CanCount)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= Timer && !Exploded)
        {
            CanCount = false;
            Explode();
        }
    }

    private new void Explode()
    {
        Exploded = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bomb, 0.7f);
        CameraShake.instance.OnShakeDuration?.Invoke(0.4f, 6f, 1.2f);
        cParticleModification.Execute();
        KillNearbyPlayers();
    }

    private void KillNearbyPlayers()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        if (hitColliders.Length != 0)
        {
            foreach (Collider item in hitColliders)
            {
                if (item.GetComponentInParent<Player>() != null)
                {
                    item.GetComponentInParent<Rigidbody>().AddExplosionForce(explosionForce * 1.5f, transform.position, explosionRadius);
                    item.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * explosionForce * 0.1f, ForceMode.Impulse);
                    StartCoroutine(item.GetComponentInParent<Player>().Rumble(0.8f, 0.8f, 0.8f));
                }
                if (item.GetComponentInParent<Bomb>() != null)
                {
                    item.GetComponentInChildren<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

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
        SetAnimationKeys();
        Timer = Random.Range(minExplosionTime, maxExplosionTime);
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(elapsedTime) * speed;

        if (CanCount)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= Timer)
        {
            CanCount = false;
            Explode();
        }
    }

    private new void Explode()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bomb, 0.7f);
        CameraShake.instance.OnShakeDuration?.Invoke(0.4f, 6f, 1.2f);
        cParticleModification.Execute();
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
                    item.GetComponent<Rigidbody>().AddExplosionForce(explosionForce * 1.5f, transform.position, explosionRadius);
                }
                if (item.GetComponentInParent<Bomb>() != null)
                {
                    item.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
    }
}

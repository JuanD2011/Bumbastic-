using UnityEngine;

public class Explosion : MonoBehaviour
{
    Rigidbody[] c_Rigidbodies = null;
    [SerializeField] float minForce = 100f, maxForce = 750f, radius = 10f;

    public event System.Action OnBoxExplode;

    private void Awake()
    {
        c_Rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    void Start()
    {
        foreach (Rigidbody rigidbody in c_Rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void Explode()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxExplosion, 1f);
        foreach (Rigidbody rigidbody in c_Rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(Random.Range(minForce, maxForce), rigidbody.transform.position, radius);
        }
        OnBoxExplode?.Invoke();
    }
}

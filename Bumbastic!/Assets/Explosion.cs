using UnityEngine;

public class Explosion : MonoBehaviour
{
    Rigidbody[] c_Rigidbodies = null;
    [SerializeField] float minForce = 100f, maxForce = 750f, radius = 10f;

    public event System.Action OnBoxExplode;

    [SerializeField] GameObject[] smokes = new GameObject[5];

    private void Awake()
    {
        c_Rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    void OnEnable()
    {
        foreach (GameObject gameObject in smokes)
        {
            gameObject.SetActive(false);
        }

        foreach (Rigidbody rigidbody in c_Rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.transform.localPosition = Vector3.zero;
            rigidbody.transform.localEulerAngles = Vector3.zero;
        }
    }

    public void Explode()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxExplosion, 1f);

        foreach (GameObject gameObject in smokes)
        {
            gameObject.SetActive(true);
        }

        foreach (Rigidbody rigidbody in c_Rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(Random.Range(minForce, maxForce), rigidbody.transform.position, radius);
        }
        OnBoxExplode?.Invoke();
    }
}

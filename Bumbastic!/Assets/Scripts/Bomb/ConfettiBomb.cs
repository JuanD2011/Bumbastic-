using UnityEngine;

public class ConfettiBomb : MonoBehaviour
{
    private ParticleSystem m_Particle;
    Rigidbody m_Rigidbody = null;
    private Renderer m_Renderer;
    [SerializeField] GameObject wick = null;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Particle = GetComponentInChildren<ParticleSystem>();
        m_Renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        Invoke("Bum", 2f);
    }

    private void Bum()
    {
        wick.SetActive(false);
        m_Renderer.enabled = false;
        GetComponent<Collider>().enabled = false;
        m_Particle.Play();
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.confettiBomb, 0.4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null)
        {
            if (transform.parent == null)
            {
                player.Animator.SetTrigger("Reception");
                transform.position = player.Catapult.position;
                transform.SetParent(player.Catapult);
                m_Rigidbody.isKinematic = true;
            }
        }
    }
}

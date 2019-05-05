using UnityEngine;

public class ConfettiBomb : MonoBehaviour
{
    private ParticleSystem m_Particle;
    private Renderer m_Renderer;

    void Start()
    {
        m_Particle = GetComponentInChildren<ParticleSystem>();
        m_Renderer = GetComponent<Renderer>();
        Invoke("Bum", 2f);
    }

    private void Bum()
    {
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
                transform.position = player.Catapult.position;
                transform.SetParent(player.Catapult);  
            }
        }
    }
}

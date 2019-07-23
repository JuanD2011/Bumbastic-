using UnityEngine;

public class Confetti : MonoBehaviour
{
    ParticleSystem m_ParticleSystem;

    Vector3 initpos;

    private void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        initpos = transform.position;
        InvokeRepeating("Explode", Random.Range(0.3f, 2f), Random.Range(2f, 5f));
    }

    private void Explode()
    {
        if (!m_ParticleSystem.isPlaying)
        {
            m_ParticleSystem.Play();
            transform.position = initpos + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.confettiBomb, 0.7f); 
        }
    }
}

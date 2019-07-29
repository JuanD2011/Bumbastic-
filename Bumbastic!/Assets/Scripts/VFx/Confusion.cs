using UnityEngine;

public class Confusion : MonoBehaviour
{
    Player player = null;
    ParticleSystem m_ParticleSystem = null;

    private void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        player.OnStuned += ShowUpParticles;
    }

    private void ShowUpParticles(bool _canShowParticles)
    {
        m_ParticleSystem.Stop();

        if (_canShowParticles) m_ParticleSystem.Play();
    }
}

using UnityEngine;

public class Dash : MonoBehaviour
{
    ThrowerPlayer p_Player = null;

    ParticleSystem m_ParticleSystem = null;
    ParticleSystem.ShapeModule shapeModule;

    private void Awake()
    {
        p_Player = GetComponentInParent<ThrowerPlayer>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        shapeModule = m_ParticleSystem.shape;
    }

    private void Start()
    {
        p_Player.OnDashExecuted += ActiveParticles;
    }

    private void ActiveParticles(Player _player)
    {
        if (_player != p_Player) return;

        m_ParticleSystem.Play();
    }
}

using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    SpeedUpParticle speedUpParticle;

    private void Awake()
    {
        GetPlayer();
        speedUpParticle = GetComponentInChildren<SpeedUpParticle>();
    }

    private void Start()
    {
        Duration = 5f;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cSpeedUP, 1f);
        StartCoroutine(InitSpeedUP());
        speedUpParticle.OnComplete += OnComplete;
    }

    IEnumerator InitSpeedUP()
    {
        m_player.speedPU = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpSpeedUP, 0.7f);
        yield return new WaitForSeconds(Duration);
        m_player.speedPU = false;
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}

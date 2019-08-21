using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    GameObject speedUpParticle;

    private void Awake()
    {
        GetPlayer();
    }

    private void Start()
    {
        Duration = 5f;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cSpeedUP, 1f);
        StartCoroutine(InitSpeedUP());
        speedUpParticle.GetComponent<SpeedUpParticle>().OnComplete += OnComplete;
    }

    IEnumerator InitSpeedUP()
    {
        speedUpParticle = Instantiate(HotPotatoManager.HotPotato.SpeedUpParticleSystem, transform.position, Quaternion.identity, m_player.transform);
        m_player.speedPU = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpSpeedUP, 0.7f);
        yield return new WaitForSeconds(Duration);
        m_player.speedPU = false;
    }

    private void OnComplete()
    {
        Destroy(speedUpParticle.gameObject);
        Destroy(this);
    }
}

using System.Collections;
using UnityEngine;

public class Magnet : PowerUp
{
    [SerializeField, HideInInspector] float lerpDuration = 2f;

    public float LerpDuration { get => lerpDuration; set => lerpDuration = value; }

    private void Awake()
    {
        GetPlayer();
    }

    private void Start()
    {
        StartCoroutine(LerpBomb());
        m_player.Collider.enabled = true;
        transform.GetChild(0).position = transform.position + new Vector3(0f, 0.8f, 0f);
    }

    IEnumerator LerpBomb()
    {
        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        m_player.Stun(true);
        float elapsedTime = 0f;

        Vector3 initBombPos = HotPotatoManager.HotPotato.Bomb.transform.position;

        float normalizedTime = 0f;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpMagnet, 0.7f, true);

        while (normalizedTime < 0.9)
        {
            HotPotatoManager.HotPotato.Bomb.transform.position = Vector3.Lerp(initBombPos, m_player.transform.position, normalizedTime);
            normalizedTime = elapsedTime / LerpDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpMagnet, 0.7f, false);

        m_player.CatchBomb(HotPotatoManager.HotPotato.Bomb);
        m_player.Stun(false);
        Destroy(gameObject);
    }
}

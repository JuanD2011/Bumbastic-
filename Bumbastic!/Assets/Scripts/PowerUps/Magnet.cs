using System.Collections;
using UnityEngine;

public class Magnet : PowerUp
{
    [SerializeField] float lerpDuration = 2f;

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
        ThrowerPlayer thrower = m_player as ThrowerPlayer;
        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        thrower.Stun(true);
        float elapsedTime = 0f;

        Vector3 initBombPos = HotPotatoManager.HotPotato.Bomb.transform.position;

        float normalizedTime = 0f;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpMagnet, 0.7f, true);

        while (normalizedTime < 0.9)
        {
            HotPotatoManager.HotPotato.Bomb.transform.position = Vector3.Lerp(initBombPos, thrower.transform.position, normalizedTime);
            normalizedTime = elapsedTime / lerpDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpMagnet, 0.7f, false);

        thrower.CatchBomb(HotPotatoManager.HotPotato.Bomb);
        thrower.Stun(false);
        Destroy(gameObject);
    }
}

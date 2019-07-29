using System.Collections;
using UnityEngine;

public class Magnet : PowerUp
{
    GameObject magnetManager;

    [SerializeField] float lerpDuration = 2f;

    private void Awake()
    {
        GetPlayer();
    }

    private void Start()
    {
        magnetManager = Instantiate(HotPotatoManager.HotPotato.MagnetParticleSystem, transform.position + new Vector3(0f, 0.8f, 0f), Quaternion.identity, m_player.transform);

        StartCoroutine(LerpBomb());
        m_player.Collider.enabled = true;
    }

    IEnumerator LerpBomb()
    {
        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        m_player.Stun(true);
        float elapsedTime = 0f;

        Vector3 initBombPos = HotPotatoManager.HotPotato.Bomb.transform.position;

        float normalizedTime = 0f;

        while (normalizedTime < 0.9)
        {
            HotPotatoManager.HotPotato.Bomb.transform.position = Vector3.Lerp(initBombPos, m_player.transform.position, normalizedTime);
            normalizedTime = elapsedTime / lerpDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_player.CatchBomb(HotPotatoManager.HotPotato.Bomb);
        m_player.Stun(false);
        Destroy(magnetManager);
        Destroy(this);
    }
}

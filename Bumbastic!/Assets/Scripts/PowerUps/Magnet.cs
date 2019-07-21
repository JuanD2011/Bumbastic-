using System.Collections;
using UnityEngine;

public class Magnet : PowerUp
{
    GameObject magnetManager;

    [SerializeField] float lerpDuration = 2f;

    protected override void Start()
    {
        base.Start();

        magnetManager = Instantiate(HotPotatoManager.HotPotato.MagnetParticleSystem, transform.position, Quaternion.identity, player.transform);

        StartCoroutine(LerpBomb());
    }

    IEnumerator LerpBomb()
    {
        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        player.Stun(true);
        float elapsedTime = 0f;

        Vector3 initBombPos = HotPotatoManager.HotPotato.Bomb.transform.position;

        float normalizedTime = 0f;

        while (normalizedTime < 0.9)
        {
            HotPotatoManager.HotPotato.Bomb.transform.position = Vector3.Lerp(initBombPos, player.transform.position, normalizedTime);
            normalizedTime = elapsedTime / lerpDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.CatchBomb(HotPotatoManager.HotPotato.Bomb);
        player.Stun(false);
        Destroy(magnetManager);
        Destroy(this);
    }
}

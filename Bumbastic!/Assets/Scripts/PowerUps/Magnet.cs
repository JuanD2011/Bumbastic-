using System.Collections;
using UnityEngine;

public class Magnet : PowerUp
{
    GameObject magnetManager;

    [SerializeField] float lerpDuration = 2f;

    protected override void Start()
    {
        base.Start();

        magnetManager = Instantiate(GameManager.Manager.magnetParticleSystem, transform.position, Quaternion.identity, player.transform);

        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        StartCoroutine(LerpBomb());
    }

    IEnumerator LerpBomb()
    {
        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        player.Stun(true);
        float elapsedTime = 0f;

        Vector3 initBombPos = HotPotatoManager.HotPotato.Bomb.transform.position;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            HotPotatoManager.HotPotato.Bomb.transform.position = Vector3.Lerp(initBombPos, player.Catapult.position, elapsedTime / lerpDuration);
            yield return null;
        }
        Destroy(magnetManager);
        player.Stun(false);
        Destroy(this);
    }
}

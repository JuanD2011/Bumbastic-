﻿using System.Collections;
using UnityEngine;

public class Magnet : PowerUp
{
    GameObject magnetManager;

    [SerializeField] float lerpDuration = 2f;

    protected override void Start()
    {
        base.Start();

        magnetManager = Instantiate(GameManager.Manager.magnetParticleSystem, transform.position, Quaternion.identity, player.transform);

        StartCoroutine(LerpBomb());
    }

    IEnumerator LerpBomb()
    {
        HotPotatoManager.HotPotato.Bomb.transform.SetParent(null);
        HotPotatoManager.HotPotato.Bomb.Collider.enabled = true;
        player.Stun(true);
        float elapsedTime = 0f;

        Vector3 initBombPos = HotPotatoManager.HotPotato.Bomb.transform.position;

        while (HotPotatoManager.HotPotato.Bomb.transform.parent == null)
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

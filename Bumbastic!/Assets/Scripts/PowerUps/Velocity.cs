using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    GameObject speedUpParticle;

    protected override void Start()
    {
        base.Start();
        Duration = 5f;
        StartCoroutine(InitSpeedUP());
        speedUpParticle.GetComponent<SpeedUpParticle>().OnComplete += OnComplete;
    }

    IEnumerator InitSpeedUP()
    {
        speedUpParticle = Instantiate(GameManager.Manager.speedUpParticleSystem, transform.position, Quaternion.identity, player.transform);
        player.speedPU = true;
        yield return new WaitForSeconds(Duration);
        player.speedPU = false;
    }

    private void OnComplete()
    {
        Destroy(speedUpParticle.gameObject);
        Destroy(this);
    }
}

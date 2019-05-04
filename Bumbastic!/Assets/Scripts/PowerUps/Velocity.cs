using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    GameObject speedUpParticle;

    public void Execute()
    {
        StartCoroutine(InitSpeedUP());

        speedUpParticle.GetComponent<SpeedUpParticle>().OnComplete += OnComplete;
        Duration = 5f;
    }

    private void OnComplete()
    {
        Destroy(speedUpParticle.gameObject);
        Destroy(this);
    }

    IEnumerator InitSpeedUP()
    {
        WaitForSeconds wait = new WaitForSeconds(Duration);

        speedUpParticle = Instantiate(GameManager.Manager.speedUpParticleSystem, transform.position, Quaternion.identity, player.transform);
        player.SpeedPU = true;
        yield return wait;
        player.SpeedPU = false;
    }
}

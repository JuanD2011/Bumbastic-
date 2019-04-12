using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    GameObject speedUpParticle;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Execute());
        speedUpParticle.GetComponent<SpeedUpParticle>().OnComplete += OnComplete;
        Duration = 5f;
    }

    private void OnComplete()
    {
        Destroy(speedUpParticle.gameObject);
        Destroy(this);
    }

    IEnumerator Execute()
    {
        WaitForSeconds wait = new WaitForSeconds(Duration);

        speedUpParticle = Instantiate(GameManager.manager.speedUpParticleSystem, transform.position, Quaternion.identity, GetComponentInChildren<Bummie>().transform);
        player.SpeedPU = true;
        yield return wait;
        player.SpeedPU = false;
    }
}

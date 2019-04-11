using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    GameObject speedUpParticle;

    private void Awake()
    {
        StartCoroutine(Execute());
    }

    protected override void Start()
    {
        base.Start();
        Duration = 5f;
        speedUpParticle.GetComponent<SpeedUpParticle>().OnComplete += OnComplete;
    }

    private void OnComplete()
    {
        Destroy(speedUpParticle.gameObject);
        Destroy(this);
    }

    IEnumerator Execute()
    {
        WaitForSeconds wait = new WaitForSeconds(Duration);

        speedUpParticle = Instantiate(GameManager.manager.speedUpParticleSystem, Vector3.zero, Quaternion.identity, transform);
        player.SpeedPU = true;
        yield return wait;
        player.SpeedPU = false;
    }
}

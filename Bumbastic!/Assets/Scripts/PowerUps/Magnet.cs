using UnityEngine;

public class Magnet : PowerUp
{
    MagnetManager magnetManager;

    protected override void Start()
    {
        base.Start();
        Execute();
        magnetManager = GetComponentInChildren<MagnetManager>();
        magnetManager.OnLerpComplete += OnComplete;
    }

    private void OnComplete()
    {
        Destroy(magnetManager.gameObject);
        Destroy(this);
    }

    void Execute()
    {
        Instantiate(GameManager.manager.magnetParticleSystem, Vector3.zero, Quaternion.identity, gameObject.transform);
    }
}

using UnityEngine;

public class Magnet : PowerUp
{
    GameObject magnetManager;

    private void Awake()
    {
        Execute();
    }

    protected override void Start()
    {
        base.Start();
        magnetManager.GetComponentInChildren<MagnetManager>().OnLerpComplete += OnComplete;
    }

    private void OnComplete()
    {
        Destroy(magnetManager.gameObject);
        Destroy(this);
    }

    void Execute()
    {
        magnetManager = Instantiate(GameManager.manager.magnetParticleSystem, Vector3.zero, Quaternion.identity, gameObject.transform);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class SpeedUpParticle : ParticleModication
{
    PowerUp powerUp;
    [SerializeField] private float angularVel = 0.5f;

    public Action OnComplete;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        lightIntensity = 2f;
        powerUp = GetComponentInParent<PowerUp>();

        if (powerUp != null)
        {
            duration = powerUp.Duration;
        }
        Execute();
    }

    protected override void Execute()
    {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        float elapsedTime = 0f;

        Light.enabled = true;

        foreach (ParticleSystem item in ParticleSystems)
        {
            item.Play();
            yield return null;
        }

        while (elapsedTime < RealTime)
        {
            Light.intensity = lightIntensity * Mathf.Sin(angularVel * elapsedTime) + 4f;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (Light.intensity > 0)
        {
            Mathf.Lerp(Light.intensity, 0, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Light.enabled = false;
        OnComplete?.Invoke();
    }
}
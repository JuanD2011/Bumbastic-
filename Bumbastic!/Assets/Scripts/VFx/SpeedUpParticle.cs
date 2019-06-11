using System;
using System.Collections;
using UnityEngine;

public class SpeedUpParticle : ParticleModication
{
    Velocity p_Velocity;
    [SerializeField] private float angularVel = 5f;

    public Action OnComplete;

    protected override void Start()
    {
        p_Velocity = GetComponentInParent<Velocity>();

        if (p_Velocity != null)
        {
            duration = p_Velocity.Duration;
        }

        base.Start();
        Execute();
    }

    public override void Execute()
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

        while (elapsedTime < duration)
        {
            Light.intensity = lightIntensity * Mathf.Sin(angularVel * elapsedTime) + 4f;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < startLifetime)
        {
            MainModules[0].startColor = Color.Lerp(MainModules[0].startColor.colorMax, Color.clear, elapsedTime / startLifetime);
            MainModules[1].startColor = Color.Lerp(MainModules[1].startColor.colorMax, Color.clear, elapsedTime / startLifetime);
            Light.intensity = Mathf.Lerp(Light.intensity, 0, elapsedTime / startLifetime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Light.enabled = false;
        OnComplete?.Invoke();
    }
}
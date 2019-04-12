using System.Collections;
using UnityEngine;

public class ExplosionParticle : ParticleModication
{
    AudioSource m_audioSource;

    protected override void Start()
    {
        base.Start();
        Bomb.OnExplode += Execute;
    }

    protected override void Execute()
    {
        m_audioSource = AudioManager.instance.CurrentAudioSource;

        if (m_audioSource != null)
        {
            if (Light != null)
            {
                StartCoroutine(ExplosionParticles());
            }
            else Debug.LogError("There's no light attached");
        }
        else Debug.LogError("There's no Audio Source available");
    }

    IEnumerator ExplosionParticles()
    {
        float elapsedTime = 0f;

        float volume = m_audioSource.volume;
        float pitch = m_audioSource.pitch;

        Light.enabled = true;

        foreach (ParticleSystem item in ParticleSystems)
        {
            item.Play();
            yield return null;
        }

        while (elapsedTime < RealTime)
        {
            float realValue = curve.Evaluate((elapsedTime * velocity) / RealTime);
            float value = curve.Evaluate(elapsedTime / duration);

            Light.intensity = realValue * lightIntensity;
            Light.color = gradient.Evaluate(elapsedTime / duration);

            m_audioSource.pitch = realValue;
            m_audioSource.volume = realValue;

            if (modifySize)
            {
                MainModules[0].startSize = value * size;
                MainModules[1].startSize = value * size;
            }

            if (modifyRadius)
            {
                ShapeModules[0].radius = curve.Evaluate(elapsedTime / duration) * radius;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }

        m_audioSource.pitch = pitch;
        m_audioSource.volume = volume;

        Light.enabled = false;
    }
}
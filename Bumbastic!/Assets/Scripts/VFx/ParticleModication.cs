using System.Collections;
using UnityEngine;

public class ParticleModication : MonoBehaviour {

    [SerializeField] Gradient gradient;
    [SerializeField] float duration, radius, velocity, lightIntensity, size;
    [SerializeField] AnimationCurve curve;

    ParticleSystem[] particleSystems;
    ParticleSystem.ShapeModule[] shapeModules;
    ParticleSystem.MainModule[] mainModules;

    [SerializeField] bool modifySize;
    [SerializeField] bool modifyRadius;

    new Light light;
    AudioSource m_audioSource;

    float realTime = 0f;

    void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        mainModules = new ParticleSystem.MainModule[particleSystems.Length];
        shapeModules = new ParticleSystem.ShapeModule[particleSystems.Length];

        for (int i = 0; i < particleSystems.Length; i++)
        {
            mainModules[i] = particleSystems[i].main;
            shapeModules[i] = particleSystems[i].shape;
            mainModules[i].simulationSpeed = velocity;
            mainModules[i].duration = duration;

            if (i != particleSystems.Length - 2)
            {
                mainModules[i].startColor = gradient;
            }
        }

        light = GetComponentInChildren<Light>();
        realTime = mainModules[0].duration + mainModules[0].startLifetime.Evaluate(1);

        Bomb.OnExplode += SetParticles;
	}

    private void SetParticles(Player _player)
    {
        m_audioSource = AudioManager.instance.CurrentAudioSource;

        if (m_audioSource != null)
        {
            if (light != null)
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

        light.enabled = true;

        foreach (ParticleSystem item in particleSystems)
        {
            item.Play();
            yield return null;
        }

        while (elapsedTime < realTime)
        {
            float realValue = curve.Evaluate((elapsedTime * velocity) / realTime);
            float value = curve.Evaluate(elapsedTime / duration);

            light.intensity = realValue * lightIntensity;
            light.color = gradient.Evaluate(elapsedTime / duration); 

            m_audioSource.pitch = realValue;
            m_audioSource.volume = realValue; 

            if (modifySize)
            {
                mainModules[0].startSize = value * size;
                mainModules[1].startSize = value * size;
            }

            if (modifyRadius)
            {
                shapeModules[0].radius = curve.Evaluate(elapsedTime / duration) * radius;
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

        light.enabled = false; 
    }
}

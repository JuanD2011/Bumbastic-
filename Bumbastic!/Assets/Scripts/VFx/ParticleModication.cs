using System.Collections;
using UnityEngine;

public class ParticleModication : MonoBehaviour {

    [SerializeField] Gradient gradient;
    [SerializeField] float duration, radius, velocity, lightIntensity, size;
    [SerializeField] AnimationCurve curve;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;

    ParticleSystem[] particleSystems;
    ParticleSystem.ShapeModule[] shapeModules;
    ParticleSystem.MainModule[] mainModules;

    [SerializeField] bool modifySize;
    [SerializeField] bool modifyRadius;

    new Light light;

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
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(StartParticle());
        }
	}

    IEnumerator StartParticle()
    {
        float elapsedTime = 0f;
        light.enabled = true;
        audioSource.PlayOneShot(audioClip);
        float volume = audioSource.volume;
        float pitch = audioSource.pitch;

        foreach (var item in particleSystems)
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
            audioSource.pitch = realValue;
            audioSource.volume = realValue;

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
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        light.enabled = false;
    }
}

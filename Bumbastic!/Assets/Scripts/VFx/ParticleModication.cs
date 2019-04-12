using UnityEngine;

public abstract class ParticleModication : MonoBehaviour
{
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected float duration, radius, velocity, lightIntensity, size;
    [SerializeField] protected AnimationCurve curve;

    ParticleSystem[] particleSystems;
    ParticleSystem.ShapeModule[] shapeModules;
    ParticleSystem.MainModule[] mainModules;

    [SerializeField] protected bool modifySize;
    [SerializeField] protected bool modifyRadius;
    [SerializeField] protected bool modifyStartColor = false;

    new Light light;

    float realTime = 0f;

    protected int particlesToSetColor = 2;

    protected ParticleSystem[] ParticleSystems { get => particleSystems; set => particleSystems = value; }
    protected ParticleSystem.ShapeModule[] ShapeModules { get => shapeModules; set => shapeModules = value; }
    protected ParticleSystem.MainModule[] MainModules { get => mainModules; set => mainModules = value; }
    protected float RealTime { get => realTime; private set => realTime = value; }
    protected Light Light { get => light; set => light = value; }

    protected virtual void Start()
    {
        ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        MainModules = new ParticleSystem.MainModule[ParticleSystems.Length];
        ShapeModules = new ParticleSystem.ShapeModule[ParticleSystems.Length];

        for (int i = 0; i < particleSystems.Length; i++)
        {
            MainModules[i] = ParticleSystems[i].main;
            ShapeModules[i] = ParticleSystems[i].shape;
            MainModules[i].simulationSpeed = velocity;
            MainModules[i].duration = duration;

            if (modifyStartColor)
            {
                if (i < particlesToSetColor)
                {
                    MainModules[i].startColor = gradient;
                } 
            }
        }

        Light = GetComponentInChildren<Light>(true);
        RealTime = MainModules[0].duration + MainModules[0].startLifetime.Evaluate(1);
	}

    protected abstract void Execute();
}

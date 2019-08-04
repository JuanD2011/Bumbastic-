using UnityEngine;

public abstract class ParticleModication : MonoBehaviour
{
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected float duration = 5f, radius = 1f, velocity = 1f, lightIntensity = 2f, size = 5f;
    [SerializeField] protected AnimationCurve curve;

    ParticleSystem[] particleSystems;
    ParticleSystem.ShapeModule[] shapeModules;
    ParticleSystem.MainModule[] mainModules;

    [SerializeField] protected bool modifySize = true;
    [SerializeField] protected bool modifyRadius = true;
    [SerializeField] protected bool modifyStartColor = false;

    new Light light;

    float realTime = 0f;

    [SerializeField] protected int particlesToSetColor = 2;
    protected float startLifetime;

    public System.Action OnComplete;

    protected ParticleSystem[] ParticleSystems { get => particleSystems; set => particleSystems = value; }
    protected ParticleSystem.ShapeModule[] ShapeModules { get => shapeModules; set => shapeModules = value; }
    protected ParticleSystem.MainModule[] MainModules { get => mainModules; set => mainModules = value; }
    protected float RealTime { get => realTime; private set => realTime = value; }
    protected Light Light { get => light; set => light = value; }

    protected virtual void Awake()
    {
        ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        MainModules = new ParticleSystem.MainModule[ParticleSystems.Length];
        ShapeModules = new ParticleSystem.ShapeModule[ParticleSystems.Length];
        Light = GetComponentInChildren<Light>(true);
    }

    protected virtual void Start()
    {
        for (int i = 0; i < ParticleSystems.Length; i++)
        {
            MainModules[i] = ParticleSystems[i].main;
            MainModules[i].simulationSpeed = velocity;
            MainModules[i].duration = duration;
            ShapeModules[i] = ParticleSystems[i].shape; 

            if (modifyStartColor)
            {
                if (i < particlesToSetColor)
                {
                    MainModules[i].startColor = gradient;
                } 
            }
        }
        startLifetime = MainModules[0].startLifetime.Evaluate(1);

        RealTime = MainModules[0].duration + startLifetime;
	}

    public abstract void Execute();
}

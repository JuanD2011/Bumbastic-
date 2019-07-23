using System.Collections;
using UnityEngine;

public class MagnetManager : ParticleModication
{
    LineRenderer wave;
    Vector3[] wavePositions = new Vector3[2];

    [SerializeField] float ringsFadeOut = 0.5f;
    [SerializeField] float thundersSpeed = 20f;

    ParticleSystem.Particle[] pathParticles;

    GameObject bomb;
    Player player;

    float t = 0f;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
        wave = GetComponentInChildren<LineRenderer>();
        pathParticles = new ParticleSystem.Particle[MainModules[0].maxParticles];
        bomb = HotPotatoManager.HotPotato.Bomb.gameObject;
    }

    protected override void Start()
    {
        base.Start();
        t = 0;
        Execute();
    }

    public override void Execute()
    {
        foreach (ParticleSystem particleSystem in ParticleSystems)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play(); 
            }
        }
        StartCoroutine(SetParticles());
    }

    void Update()
    {
        t += Time.deltaTime;
        float currentSpeed = curve.Evaluate(t / duration) * thundersSpeed;
        ShapeModules[0].radius = curve.Evaluate(t / duration) * radius;
        MainModules[0].startSize = curve.Evaluate(t / duration) * size;
        Light.intensity = curve.Evaluate(t / RealTime) * lightIntensity;
        Light.color = gradient.Evaluate(t / RealTime);

        int length = ParticleSystems[0].GetParticles(pathParticles);
        int i = 0;
        transform.LookAt(bomb.transform.position);

        while (i < length)
        {
            Vector3 direction = (bomb.transform.position - pathParticles[i].position).normalized;
            pathParticles[i].velocity = direction * currentSpeed;
            float distance = Vector3.Distance(bomb.transform.position, pathParticles[i].position);

            if (distance < 1f)
            {
                pathParticles[i].remainingLifetime = -0.1f;
            }
            i++;
        }

        ParticleSystems[0].SetParticles(pathParticles, length);
    }

    public IEnumerator SetParticles()
    {
        Vector3 initBombPos = bomb.transform.position;

        wavePositions[0] = player.transform.position;
        wavePositions[1] = bomb.transform.position;
        wave.SetPositions(wavePositions);

        float distance = Mathf.Abs(Vector3.Distance(player.Catapult.transform.position, initBombPos));

        while (distance >= 2f)
        {
            wavePositions[0] = player.transform.position;
            wavePositions[1] = bomb.transform.position;
            wave.SetPositions(wavePositions);
            distance = Mathf.Abs(Vector3.Distance(player.transform.position, bomb.transform.position));
            yield return null;
        }

        float t = 0;

        while(t <= ringsFadeOut)
        {
            MainModules[1].startColor = Color.Lerp(MainModules[1].startColor.colorMax, Color.clear, t / ringsFadeOut);//Refers to the Rings
            t += Time.deltaTime;
            yield return null;
        }
    }
}

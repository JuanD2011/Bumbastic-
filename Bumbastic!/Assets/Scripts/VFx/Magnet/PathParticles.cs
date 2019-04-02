using UnityEngine;

public class PathParticles : MonoBehaviour
{

    [SerializeField] AnimationCurve curve, sizeCurve, radiusCurve;
    [SerializeField] float particleSpeed = 40f, speedCurveDuration, radiusCurveDuration, radius;
  

    ParticleSystem mPartSystem;
    ParticleSystem.MainModule main;
    ParticleSystem.ShapeModule shape;
    ParticleSystem.Particle[] mParticles;

    [HideInInspector]
    public bool activateParticles;
    float t;

    Transform target;

    private void Awake()
    {
        activateParticles = false;
        mPartSystem = GetComponent<ParticleSystem>();
        main = mPartSystem.main;
        shape = mPartSystem.shape;

        target = GameManager.manager.Bomb.transform;
    }

    void Initialize()
    {
        if (mParticles == null || mParticles.Length < mPartSystem.main.maxParticles)
            mParticles = new ParticleSystem.Particle[mPartSystem.main.maxParticles];
    }

    private void OnDisable()
    {
        t = 0;
        activateParticles = false;
    }

    void Update()
    {
        t += Time.deltaTime;
        float currentSpeed = curve.Evaluate(t / speedCurveDuration) * particleSpeed;
        shape.radius = radiusCurve.Evaluate(t / radiusCurveDuration) * radius;
        main.startSize = sizeCurve.Evaluate(t / 1f) * 2f;

        Initialize();
        int length = mPartSystem.GetParticles(mParticles);
        int i = 0;
        transform.LookAt(target.position);
        
        while (i < length)
        {
            Vector3 direction = (target.position - mParticles[i].position).normalized;
            mParticles[i].velocity = direction * currentSpeed;
            float distance = Vector3.Distance(target.position, mParticles[i].position);

            if (distance < 1f)
            {
                if(!activateParticles)
                    activateParticles = true;
                mParticles[i].remainingLifetime = -0.1f;
            }
            i++;
        }
        
        mPartSystem.SetParticles(mParticles, length);       
    }
}

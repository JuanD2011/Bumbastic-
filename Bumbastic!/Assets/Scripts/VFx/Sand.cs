using System.Collections;
using UnityEngine;

public class Sand : MonoBehaviour
{
    ParticleSystem[] c_ParticleSystems = null;
    ParticleSystem.EmissionModule[] emissionModules;
    ParticleSystem.MainModule[] mainModules;

    BoxCollider m_BoxCollider = null;

    [SerializeField] float[] emissionRate = new float[2];
    [SerializeField] float timeToStop = 30f, timeToStartSand = 8f;

    [SerializeField] bool spawnInVolume = false;
    [SerializeField, HideInInspector] bool changeParticlesColor = false;

    [SerializeField, HideInInspector] private Color desertColor = Color.white;
    [SerializeField, HideInInspector] private Color winterColor = Color.white;

    public bool ChangeParticlesColor { get => changeParticlesColor; set => changeParticlesColor = value; }
    public Color DesertColor { get => desertColor; set => desertColor = value; }
    public Color WinterColor { get => winterColor; set => winterColor = value; }

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();

        c_ParticleSystems = GetComponentsInChildren<ParticleSystem>();

        mainModules = new ParticleSystem.MainModule[c_ParticleSystems.Length];
        emissionModules = new ParticleSystem.EmissionModule[c_ParticleSystems.Length];

        for (int i = 0; i < c_ParticleSystems.Length; i++)
        {
            mainModules[i] = c_ParticleSystems[i].main;
            emissionModules[i] = c_ParticleSystems[i].emission;
        }
    }

    private void Start()
    {
        StartCoroutine(ChangeState());

        if (ChangeParticlesColor && GameManager.Manager != null)
        {
            switch (GameManager.Manager.Enviroment)
            {
                case EnumEnviroment.Desert:
                    for (int i = 0; i < mainModules.Length; i++)
                    {
                        mainModules[i].startColor = DesertColor;
                    }
                    break;
                case EnumEnviroment.Winter:
                    for (int i = 0; i < mainModules.Length; i++)
                    {
                        mainModules[i].startColor = WinterColor;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator ChangeState()
    {
        for (int i = 0; i < emissionModules.Length; i++)
        {
            emissionModules[i].rateOverTime = Random.Range(emissionRate[0], emissionRate[1]);
            yield return null;
        }

        if (spawnInVolume) SetParticlesPosition();

        yield return new WaitForSeconds(timeToStop * Random.Range(0.1f, 1f));

        for (int i = 0; i < emissionModules.Length; i++)
        {
            emissionModules[i].rateOverTime = 0f;
            yield return null;
        }

        yield return new WaitForSeconds(timeToStartSand * Random.Range(0.4f, 1f));

        StartCoroutine(ChangeState());
    }

    private void SetParticlesPosition()
    {
        for (int i = 0; i < c_ParticleSystems.Length; i++)
        {
            c_ParticleSystems[i].transform.position = m_BoxCollider.GetRandomPointInVolume();
        }
    }
}

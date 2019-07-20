using UnityEngine;

public class BombHolderLight : MonoBehaviour
{
    [SerializeField] float maxVelocity = 6f;
    float elapsedTime = 0f, bombTime = 0f;
    bool canStart = false;

    Light m_Light = null;

    private void Awake()
    {
        m_Light = GetComponent<Light>();
    }

    void Start()
    {
        Bomb.OnArmed += Initialize;
        Bomb.OnExplode += () => { canStart = false; m_Light.intensity = 0f; };
    }

    private void Initialize(float _time)
    {
        bombTime = _time;
        elapsedTime = 0f;
        canStart = true;
    }

    private void Update()
    {
        if (!canStart) return;

        m_Light.intensity = 5f * Mathf.Sin(elapsedTime / bombTime  * maxVelocity) + 10f;
        elapsedTime += Time.deltaTime;
    }
}

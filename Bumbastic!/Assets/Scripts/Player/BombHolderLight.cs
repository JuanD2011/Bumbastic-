using UnityEngine;

public class BombHolderLight : MonoBehaviour
{
    [SerializeField] float maxVelocity = 11f;
    [SerializeField]private Vector3 offset = Vector3.zero;

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

        HotPotatoManager.HotPotato.OnBombHolderChanged += UpdateBombsHolderLight;
    }

    private void UpdateBombsHolderLight(Player _bombHolder)
    {
        transform.SetParent(null);
        transform.SetParent(_bombHolder.transform);
        transform.position = _bombHolder.transform.position + offset;
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

        m_Light.intensity = 7.5f * Mathf.Sin((elapsedTime / bombTime)  * maxVelocity * elapsedTime) + 12.5f;
        elapsedTime += Time.deltaTime;
    }

    private void OnDisable()
    {
        Bomb.OnArmed -= Initialize;
    }
}

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteEffect : MonoBehaviour
{
    [SerializeField] PostProcessProfile postProcessing = null;
    [SerializeField] Color exploding;
    [SerializeField] float maxVelocity = 6f;

    Vignette vignette;

    float elapsedTime = 0f;

    float bombTime = 0f;
    FloatParameter intensity = new FloatParameter();

    bool canCount = false;

    private void Awake()
    {
        vignette = postProcessing.GetSetting<Vignette>();
    }

    private void Start()
    {
        Bomb.OnArmed += Initialize;
        Bomb.OnExplode += () => canCount = false;
    }

    private void Initialize(float _time)
    {
        vignette.color.value = exploding;
        bombTime = _time;
        elapsedTime = 0f;
        canCount = true;
    }

    private void Update()
    {
        if (canCount)
        {
            intensity.value = 0.2f * Mathf.Sin(maxVelocity * (elapsedTime / bombTime) * elapsedTime) + 0.2f;
            vignette.intensity.value = intensity.value;
            elapsedTime += Time.deltaTime; 
        }
    }

    private void OnDisable()
    {
        vignette.intensity.value = 0.3f;
        vignette.color.value = Color.black;
        Bomb.OnArmed -= Initialize;
    }
}

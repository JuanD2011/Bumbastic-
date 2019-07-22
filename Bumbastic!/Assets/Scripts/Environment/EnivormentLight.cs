using UnityEngine;

public class EnivormentLight : MonoBehaviour
{
    new Light light;

    [SerializeField] Color[] colors = new Color[0];
    [SerializeField] float[] colorTemperature = new float[0];

    private void Start()
    {
        light = GetComponent<Light>();
        Initialize();
    }

    private void Initialize()
    {
        switch (GameManager.Manager.Enviroment)
        {
            case EnumEnviroment.Desert:
                light.color = colors[0];
                light.colorTemperature = colorTemperature[0];
                break;
            case EnumEnviroment.Winter:
                light.color = colors[1];
                light.colorTemperature = colorTemperature[1];
                break;
            default:
                break;
        }
    }
}

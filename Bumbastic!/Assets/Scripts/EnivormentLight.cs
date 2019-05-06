using UnityEngine;

public class EnivormentLight : MonoBehaviour
{
    new Light light;

    [SerializeField] Color[] colors;
    [SerializeField] float[] colorTemperature;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Initialize()
    {
        switch (GameManager.Manager.Enviroment)
        {
            case EnumEnviroment.Dessert:
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

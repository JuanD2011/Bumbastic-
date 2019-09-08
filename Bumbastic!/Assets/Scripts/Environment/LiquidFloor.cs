using UnityEngine;

public class LiquidFloor : MonoBehaviour
{
    [SerializeField] Material lava = null, water = null;

    Renderer m_Renderer = null;
    int random = 0;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        random = Random.Range(0, 2);

        switch (GameManager.Manager.Enviroment)
        {
            case EnumEnviroment.Desert:
                if (random == 0) m_Renderer.material = water;
                else m_Renderer.material = lava;
                break;
            case EnumEnviroment.Winter:
                if (random == 0) m_Renderer.material = water;
                else m_Renderer.material = lava;
                break;
            case EnumEnviroment.Beach:
                m_Renderer.material = water;
                break;
            default:
                break;
        }

    }
}

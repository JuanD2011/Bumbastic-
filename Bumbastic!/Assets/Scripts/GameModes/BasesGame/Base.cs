using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Base : MonoBehaviour
{
    [SerializeField] byte lifePoints = 3;
    [SerializeField] byte id = 0;
    Renderer m_Renderer;

    public byte LifePoints { get => lifePoints; private set => lifePoints = value; }
    public Renderer Renderer { get => m_Renderer; set => m_Renderer = value; }

    public delegate void DelBase(byte _baseID, byte _lifePoints);
    public static event DelBase OnBaseDamage;

    private void Awake()
    {
        OnBaseDamage = null;
        Renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null)
        {
            if (player.HasBomb)
            {
                if (LifePoints > 0)
                {
                    LifePoints--;
                    OnBaseDamage?.Invoke(id, LifePoints);//HUDBaseGame hears it.
                }
                else
                {
                    Debug.Log("Base Destroyed");
                }
            }
        }
    }
}

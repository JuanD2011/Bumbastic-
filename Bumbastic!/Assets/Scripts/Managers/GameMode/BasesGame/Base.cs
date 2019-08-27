using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] byte lifePoints = 3;
    [SerializeField] byte id = 0;
    Renderer m_Renderer;

    public byte LifePoints { get => lifePoints; private set => lifePoints = value; }
    public Renderer Renderer { get => m_Renderer; set => m_Renderer = value; }

    public delegate void DelBase(byte _baseID, byte _lifePoints);
    public static event DelBase OnBaseDamage;

    public static Action OnBaseDestoryed;

    private void Awake()
    {
        OnBaseDestoryed = null;
        OnBaseDamage = null;
        Renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ThrowerPlayer player = other.GetComponentInParent<ThrowerPlayer>();

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
                    OnBaseDestoryed?.Invoke();
                    Debug.Log("Base Destroyed");
                }
            }
        }
    }
}

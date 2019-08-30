using System;
using UnityEngine;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] Color teamColor = new Color();
    [SerializeField] Transform[] m_spawnPoints = new Transform[2];
    [SerializeField] byte lifePoints = 3;
    [SerializeField] byte id = 0;

    List<Player> members = new List<Player>();

    public byte LifePoints { get => lifePoints; private set => lifePoints = value; }
    public List<Player> Members { get => members; set => members = value; }
    public Transform[] SpawnPoints { get => m_spawnPoints; private set => m_spawnPoints = value; }
    public Color TeamColor { get => teamColor; private set => teamColor = value; }
    public byte Id { get => id; private set => id = value; }

    public delegate void DelBase(byte _baseID, byte _lifePoints);
    public static event DelBase OnBaseDamage;

    public static Action OnBaseDestoryed;

    private void Awake()
    {
        OnBaseDestoryed = null;
        OnBaseDamage = null;

        GetComponent<Renderer>().materials[1].color = teamColor;
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
                    OnBaseDamage?.Invoke(Id, LifePoints);
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

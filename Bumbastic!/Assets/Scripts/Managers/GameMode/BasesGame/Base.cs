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
    public static event DelBase OnBaseDamage, OnBaseDestroyed;

    private void Awake()
    {
        OnBaseDamage = null; OnBaseDestroyed = null;

        GetComponent<Renderer>().materials[1].color = teamColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        BasesBomb bomb = other.GetComponent<BasesBomb>();

        if (bomb != null)
        {
            if (LifePoints > 1)
            {
                LifePoints--;
                bomb.Explode();
                OnBaseDamage?.Invoke(Id, LifePoints);
            }
            else
            {
                LifePoints = 0;
                bomb.Explode();
                OnBaseDestroyed?.Invoke(Id, lifePoints);
                Debug.Log("Base Destroyed");
            }
        }
    }
}

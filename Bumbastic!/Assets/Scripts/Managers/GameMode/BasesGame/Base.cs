﻿using UnityEngine;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] Color teamColor = new Color();
    [SerializeField] Transform[] m_spawnPoints = new Transform[2];
    [SerializeField] byte lifePoints = 3;
    [SerializeField] byte id = 0;

    Light m_Light = null;

    public byte LifePoints { get => lifePoints; private set => lifePoints = value; }
    public List<Player> Members { get; set; } = new List<Player>();
    public Transform[] SpawnPoints { get => m_spawnPoints; private set => m_spawnPoints = value; }
    public Color TeamColor { get => teamColor; private set => teamColor = value; }
    public byte Id { get => id; private set => id = value; }

    public delegate void DelBase(byte _baseID, byte _lifePoints);
    public static event DelBase OnBaseDamage, OnBaseDestroyed;

    private void Awake()
    {
        OnBaseDamage = null; OnBaseDestroyed = null;

        GetComponent<Renderer>().materials[1].color = teamColor;
        m_Light = GetComponentInChildren<Light>();

        m_Light.color = teamColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        BasesBomb bomb = other.GetComponent<BasesBomb>();

        if (bomb != null)
        {
            if (bomb.ThrowerPlayer == null) return;

            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i] as ThrowerPlayer == bomb.ThrowerPlayer) return;
            }

            if (LifePoints > 1)
            {
                LifePoints--;
                bomb.SetThrowerPlayer(null);
                bomb.Explode();
                OnBaseDamage?.Invoke(Id, LifePoints);
            }
            else
            {
                LifePoints = 0;
                bomb.SetThrowerPlayer(null);
                bomb.Explode();
                OnBaseDestroyed?.Invoke(Id, lifePoints);
                Debug.Log("Base Destroyed");
            }
        }
    }
}

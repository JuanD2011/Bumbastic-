using System;
using System.Collections.Generic;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerPrefab;

    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }

    private void Awake()
    {
        InGame.playerSettings.Sort();
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
            player.Controls = InGame.playerSettings[i].controls;
            player.Avatar = InGame.playerSettings[i].avatar;
            player.PrefabName = InGame.playerSettings[i].name;
            player.Id = (byte)i;
            player.SpawnPoint = spawnPoints[i].position;
            player.transform.position = player.SpawnPoint;
            player.Initialize();
        }
    }
}

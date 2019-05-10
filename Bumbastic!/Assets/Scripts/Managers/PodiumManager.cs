using System.Linq;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerPrefab;

    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }

    private void Awake()
    {
        InGame.playerSettings = InGame.playerSettings.OrderBy(w => w.score).ToList();
        InGame.playerSettings.Reverse();
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            if (i < 3)
            {
                Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
                player.Controls = InGame.playerSettings[i].controls;
                player.Avatar = InGame.playerSettings[i].avatar;
                player.PrefabName = InGame.playerSettings[i].name;
                player.Id = (byte)i;
                player.SpawnPoint = spawnPoints[i].position;
                player.transform.position = player.SpawnPoint;
                player.transform.rotation = new Quaternion(0, 180, 0, 0);
                player.Initialize();
            }
            else break;
        }
    }
}

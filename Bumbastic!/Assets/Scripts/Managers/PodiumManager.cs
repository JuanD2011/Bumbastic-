using System.Linq;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerPrefab;

    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }

    private void Awake()
    {
        InputManager.playerSettings = InputManager.playerSettings.OrderBy(w => w.score).ToList();
        InputManager.playerSettings.Reverse();
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < InputManager.playerSettings.Count; i++)
        {
            if (i < 3)
            {
                Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
                player.Controls = InputManager.playerSettings[i].controls;
                player.Avatar = InputManager.playerSettings[i].avatar;
                player.PrefabName = InputManager.playerSettings[i].name;
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

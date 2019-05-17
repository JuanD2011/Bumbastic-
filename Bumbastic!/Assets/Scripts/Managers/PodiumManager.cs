using System.Linq;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerPrefab;

    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }

    private void Awake()
    {
        PlayerMenu.ResetDel();
        InGame.playerSettings = InGame.playerSettings.OrderBy(w => w.score).ToList();
        InGame.playerSettings.Reverse();
    }

    private void Start()
    {
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
            player.transform.rotation = new Quaternion(0, 180, 0, 0);
            player.Initialize();
            if (i == 0)
            {
                player.PodiumAnimation(true);
            }
            else
            {
                player.PodiumAnimation(false);
            }
            AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crowdCheer, 1f);
        }
    }
}

using System.Collections;
using System.Linq;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject rainParticle;
    [SerializeField] float timeToNextPlayer = 3f;

    protected GameObject PlayerPrefab { get => playerPrefab; private set => playerPrefab = value; }

    private void Awake()
    {
        PlayerMenu.ResetDel();
        InGame.playerSettings = InGame.playerSettings.OrderBy(w => w.score).ToList();
        InGame.playerSettings.Reverse();
    }

    private void Start()
    {
        StartCoroutine(SpawnPlayers());
    }

    private IEnumerator SpawnPlayers()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crowdCheer, 1f);
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
            else if (i == InGame.playerSettings.Count - 1)
            {
                rainParticle.SetActive(true);
            }
            else
            {
                player.PodiumAnimation(false);
            }
            yield return new WaitForSeconds(timeToNextPlayer);
        }
    }
}

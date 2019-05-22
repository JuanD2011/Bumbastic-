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
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.mrBumbasticIs, 0.7f);
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
                player.PodiumAnimation(0);
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cWin, 0.7f);
            }
            else if (i == 3)
            {
                rainParticle.SetActive(true);
                player.PodiumAnimation(2);
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cLose, 0.7f);
            }
            else
            {
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cLose, 0.7f);
                player.PodiumAnimation(1);
            }
            yield return new WaitForSeconds(timeToNextPlayer);
        }
    }
}

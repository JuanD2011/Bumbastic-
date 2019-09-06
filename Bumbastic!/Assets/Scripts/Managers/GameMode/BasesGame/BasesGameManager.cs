using System.Linq;
using UnityEngine;

public class BasesGameManager : GameManager
{
    public static BasesGameManager basesGame;

    PlayerSettings[] playerSettings;

    [SerializeField] Base[] teams = new Base[2];

    public Base[] Teams { get => teams; private set => teams = value; }
    public Base Winners { get; set; } = null;

    protected override void Awake()
    {
        if (basesGame == null) basesGame = this;
        else Destroy(this);

        base.Awake();

        playerSettings = InGame.playerSettings.ToArray();
        SpawnPlayers();
    }

    protected override void Start()
    {
        base.Start();
        BasesBomb.OnBasesBombExplode += OnBombExplode;
        Base.OnBaseDestroyed += CheckWinCondition;
    }

    private void CheckWinCondition(byte _baseID, byte _lifePoints)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            if (_baseID != teams[i].Id)
            {
                InGame.lastWinners.Clear();
                Winners = teams[i];
                for (int j = 0; j < Winners.Members.Count; j++)
                {
                    InGame.playerSettings[Winners.Members[j].Id].score += 1;
                    InGame.lastWinners.Enqueue(InGame.playerSettings[Winners.Members[j].Id]);
                }
                GameOver();
                return;
            }
        }
    }

    private void OnBombExplode(Player _PlayerExploded)
    {
        if (_PlayerExploded == null) return;

        _PlayerExploded.transform.position = _PlayerExploded.SpawnPoint;
    }

    protected override void SpawnPlayers()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            Player player = Instantiate(PlayerPrefab).GetComponent<Player>();
            Players.Add(player);
            player.Avatar = InGame.playerSettings[i].avatar;
            player.PrefabName = InGame.playerSettings[i].name;
            player.Id = (byte)i;
            player.Initialize();
        }

        SetTeamsAndSpawnPoint();
    }

    private void SetTeamsAndSpawnPoint()
    {
        switch (playerSettings.Length)
        {
            case 2:
                for (int i = 0; i < playerSettings.Length; i++)
                {
                    Teams[i].Members.Add(Players[i]);

                    if (i == 0)
                    {
                        Teams[i].Members[0].SpawnPoint = Teams[i].SpawnPoints[1].transform.position;
                        Teams[i].Members[0].transform.position = Teams[i].Members[0].SpawnPoint;
                    }
                    else
                    {
                        Teams[i].Members[0].SpawnPoint = Teams[i].SpawnPoints[0].transform.position;
                        Teams[i].Members[0].transform.position = Teams[i].Members[0].SpawnPoint;
                    }
                }
                break;
            case 3:
                playerSettings = playerSettings.OrderBy(w => w.score).ToArray();
                for (int i = 0; i < playerSettings.Length; i++)
                {
                    if (i == 0)
                    {
                        Teams[i].Members.Add(Players[playerSettings[i].id]);
                        Teams[i].Members[i].SpawnPoint = Teams[i].SpawnPoints[i].transform.position;
                        Teams[i].Members[i].transform.position = Teams[i].Members[i].SpawnPoint;
                    }
                    else
                    {
                        Teams[1].Members.Add(Players[playerSettings[i].id]);
                        Teams[1].Members[i - 1].SpawnPoint = Teams[1].SpawnPoints[i - 1].transform.position;
                        Teams[1].Members[i - 1].transform.position = Teams[1].Members[i - 1].SpawnPoint;
                    } 
                }
                break;
            case 4:
                playerSettings = playerSettings.OrderBy(w => w.score).ToArray();

                for (int i = 0; i < playerSettings.Length; i++)
                {
                    if (i == 0)
                    {
                        Teams[i].Members.Add(Players[playerSettings[i].id]);
                        Teams[i].Members[i].SpawnPoint = Teams[i].SpawnPoints[i].transform.position;
                        Teams[i].Members[i].transform.position = Teams[i].Members[i].SpawnPoint;
                    }
                    else if (i == playerSettings.Length - 1)
                    {
                        Teams[0].Members.Add(Players[playerSettings[i].id]);
                        Teams[0].Members[1].SpawnPoint = Teams[0].SpawnPoints[1].transform.position;
                        Teams[0].Members[1].transform.position = Teams[0].Members[1].SpawnPoint;
                    }
                    else
                    {
                        Teams[1].Members.Add(Players[playerSettings[i].id]);
                        Teams[1].Members[i - 1].SpawnPoint = Teams[1].SpawnPoints[i - 1].transform.position;
                        Teams[1].Members[i - 1].transform.position = Teams[1].Members[i - 1].SpawnPoint;
                    } 
                }
                break;
            default:
                break;
        }

    }
}

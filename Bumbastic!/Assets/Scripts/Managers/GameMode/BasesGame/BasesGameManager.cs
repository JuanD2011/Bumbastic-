public class BasesGameManager : GameManager
{
    public static BasesGameManager basesGame;

    public Base[] bases;

    protected override void Awake()
    {
        if (basesGame == null) basesGame = this;
        else Destroy(this);

        base.Awake();

        SetBasesColor();
    }

    protected override void Start()
    {
        base.Start();
        BasesBomb.OnBasesBombExplode += OnBombExplode;
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
            player.SpawnPoint = SpawnPoints[i].transform.position;
            player.transform.position = player.SpawnPoint;
            player.Initialize();
        }
    }

    private void SetBasesColor()
    {
        //TODO implement color to the "Hair" of the base.
    }
}

public class ExplosiveRainManager : GameManager
{
    protected override void Awake()
    {
        base.Awake();
        SpawnPlayers();
    }

    protected override void Start()
    {
        base.Start();
        ExplosiveRainBomb.OnPlayerKilled += PlayerKilled;
    }

    private void PlayerKilled(Player _player)
    {
        Players.Remove(_player);
        if (Players.Count == 1) GameOver();
    }
}

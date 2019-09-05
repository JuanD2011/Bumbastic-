public class ExplosiveRainManager : GameManager
{
    protected override void Awake()
    {
        base.Awake();
        SpawnPlayers();
        DeadZone.OnPlayerKilled -= PlayerKilled;
    }

    protected override void Start()
    {
        base.Start();
        DeadZone.OnPlayerKilled += PlayerKilled;
    }

    private void PlayerKilled(Player _killedPlayer)
    {
        Players.Remove(_killedPlayer);
        _killedPlayer.gameObject.SetActive(false);
        if (Players.Count == 1) GameOver();
    }
}

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
        DeadZone.OnPlayerKilled += PlayerKilled;
    }

    private void PlayerKilled(Player _killedPlayer)
    {
        Players.Remove(_killedPlayer);
        _killedPlayer.gameObject.SetActive(false);
        if (Players.Count == 1)
        {
            InGame.playerSettings[Players[0].Id].score += 1;
            InGame.lastWinner = InGame.playerSettings[Players[0].Id];
            Invoke("GameOver", 1f);
        }
    }
}

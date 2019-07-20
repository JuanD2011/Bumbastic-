using UnityEngine;

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

    protected override void GiveBombs()
    {
        return;
    }

    protected override void ReturnBomb(Bomb _bomb)
    {
        //TODO Return bomb to launcher
    }
}

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
            player.Controls = InGame.playerSettings[i].controls;
            player.Avatar = InGame.playerSettings[i].avatar;
            player.PrefabName = InGame.playerSettings[i].name;
            player.Id = (byte)i;
            player.SpawnPoint = SpawnPoints[i].transform.position;
            player.transform.position = player.SpawnPoint;
            player.Initialize();
        }

        GiveBombs();
    }

    private void SetBasesColor()
    {
        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].Renderer.material.SetColor("_Color", InGame.playerSettings[i].color);
            bases[i].Renderer.material.SetTexture("_MainTex", InGame.playerSettings[i].skinSprite.texture);
        }
    }

    public override void PassBomb()
    {
        return;
    }

    public override void PassBomb(Player _receiver)
    {
        return;
    }

    public override void PassBomb(Player _receiver, Player _transmitter)
    {
        return;
    }

    protected override void GiveBombs()
    {
        return;
    }
}

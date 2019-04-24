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
    }

    public override void PassBomb()
    {
        throw new System.NotImplementedException();
    }

    public override void PassBomb(Player _receiver)
    {
        throw new System.NotImplementedException();
    }

    public override void PassBomb(Player _receiver, Player _transmitter)
    {
        throw new System.NotImplementedException();
    }

    protected override void GiveBombs()
    {
        throw new System.NotImplementedException();
    }
}

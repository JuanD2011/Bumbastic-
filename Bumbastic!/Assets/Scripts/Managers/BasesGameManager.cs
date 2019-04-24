using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasesGameManager : GameManager
{
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

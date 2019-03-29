using UnityEngine;

public class Magnet : PowerUp
{
    protected override void Start()
    {
        base.Start();
        Execute();
    }

    void Execute()
    {
        //player.HasBomb = true;
        //GameManager.manager.BombHolder = player;
        //GameManager.manager.Bomb.transform.SetParent(player.transform);
        //GameManager.manager.Bomb.gameObject.transform.position = player.transform.GetChild(1).transform.position;
        //GameManager.manager.bomb.GetComponent<Bomb>().RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        //Destroy(this);
    }
}

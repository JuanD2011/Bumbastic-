using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    protected float duration;
    protected Player player;

    protected virtual void Start()
    {
        player = GetComponent<Player>();
    }

    public void PickPowerUp(Player _player)
    {
        if (!_player.HasBomb)
        {
            int randomPU = Random.Range(0, 3);
            switch (randomPU)
            {
                case 0:
                    _player.gameObject.AddComponent<Velocity>();
                    break;
                case 1:
                    _player.gameObject.AddComponent<Magnet>();
                    break;
                case 2:
                    _player.gameObject.AddComponent<Shield>();
                    break;
                case 3:
                    //GameManager.instance.bombHolder.gameObject.AddComponent<Velocity>();
                    break;
                default:
                    break;
            }
        }
    }
}

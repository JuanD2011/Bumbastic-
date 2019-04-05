using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PowerUp : MonoBehaviour, IPowerUp
{
    protected float duration;
    protected Player player;

    Collider m_Collider;
    public Collider Collider { get => m_Collider; set => m_Collider = value; }

    protected virtual void Start()
    {
        player = GetComponent<Player>();
        Collider = GetComponent<Collider>();
    }

    public void PickPowerUp(Player _player)
    {
        int randomPU = Random.Range(0, 3);
        switch (randomPU)
        {
            case 0:
                _player.gameObject.AddComponent<Velocity>();
                break;
            case 1:
                //_player.gameObject.AddComponent<Magnet>();
                _player.gameObject.AddComponent<Velocity>();
                break;
            case 2:
                //_player.gameObject.AddComponent<Shield>();
                _player.gameObject.AddComponent<Velocity>();
                break;
            case 3:
                //GameManager.instance.bombHolder.gameObject.AddComponent<Velocity>();
                break;
            default:
                break;
        }
    }
}

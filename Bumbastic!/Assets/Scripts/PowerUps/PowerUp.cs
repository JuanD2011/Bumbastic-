using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PowerUp : MonoBehaviour, IPowerUp
{
    private float duration;
    protected Player player;

    Collider m_Collider;
    public Collider Collider { get => m_Collider; set => m_Collider = value; }
    public float Duration { get => duration; protected set => duration = value; }

    protected virtual void Start()
    {
        Collider = GetComponent<Collider>();
    }

    public void PickPowerUp(Player _player)
    {
        player = _player;

        int randomPU = Random.Range(0, 3);

        switch (randomPU)
        {
            case 0:
                //player.gameObject.AddComponent<Velocity>();
                //player.GetComponent<Velocity>().Execute();
                break;
            case 1:
                //_player.gameObject.AddComponent<Magnet>();
                //player.gameObject.AddComponent<Velocity>();
                //player.GetComponent<Velocity>().Execute();
                break;
            case 2:
                //_player.gameObject.AddComponent<Shield>();
                //player.gameObject.AddComponent<Velocity>();
                //player.GetComponent<Velocity>().Execute();
                break;
            case 3:
                //GameManager.instance.bombHolder.gameObject.AddComponent<Velocity>();
                break;
            default:
                break;
        }
    }
}

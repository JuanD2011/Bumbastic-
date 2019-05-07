using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float duration = 0f;
    protected Player player;

    Collider m_Collider;
    public Collider Collider { get => m_Collider; set => m_Collider = value; }
    public float Duration { get => duration; protected set => duration = value; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        player = GetComponent<Player>();
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
                _player.gameObject.AddComponent<Magnet>();
                _player.Collider.enabled = true;
                //_player.gameObject.AddComponent<Velocity>();
                break;
            case 2:
                //_player.gameObject.AddComponent<Shield>();
                _player.gameObject.AddComponent<Magnet>();
                _player.Collider.enabled = true;
                //_player.gameObject.AddComponent<Velocity>();
                break;
            case 3:
                //GameManager.instance.bombHolder.gameObject.AddComponent<Velocity>();
                break;
            default:
                break;
        }
    }
}

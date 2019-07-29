using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float duration = 0f;
    protected Player m_player = null;

    public Collider Collider { get; set; }
    public float Duration { get => duration; protected set => duration = value; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    protected void GetPlayer()
    {
        m_player = GetComponent<Player>();
    }

    public virtual void PickPowerUp(Player _player)
    {
        int randomPU = Random.Range(0, 3);

        if (_player.HasBomb)
        {
            _player.gameObject.AddComponent<Velocity>();
            gameObject.SetActive(false);
            return;
        }

        switch (randomPU)
        {
            case 0:
                _player.gameObject.AddComponent<Velocity>();
                break;
            case 1:
                _player.gameObject.AddComponent<Magnet>();
                break;
            case 2:
                _player.gameObject.AddComponent<Magnet>();
                break;
            case 3:
                break;
            default:
                break;
        }
        gameObject.SetActive(false);
    }
}

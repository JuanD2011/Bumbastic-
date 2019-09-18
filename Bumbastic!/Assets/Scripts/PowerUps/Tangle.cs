using UnityEngine;

public class Tangle : PowerUp
{
    private Confusion confusion = null;

    public static event System.Action<bool, Player> OnTangle;

    private void Awake()
    {
        GetPlayer();
        Duration = 10f;
    }

    private void Start()
    {
        m_player.tangled = true;
        OnTangle?.Invoke(true, m_player);
        Invoke("UnTangle", Duration);
    }

    private void UnTangle()
    {
        m_player.tangled = false;
        OnTangle?.Invoke(false, m_player);
        Destroy(gameObject);
    }
}

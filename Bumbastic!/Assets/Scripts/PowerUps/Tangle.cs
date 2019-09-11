using UnityEngine;

public class Tangle : PowerUp
{
    private void Awake()
    {
        GetPlayer();
    }

    private void Start()
    {
        Duration = 10f;
        m_player.tangled = true;
        Invoke("UnTangle", Duration);
        Debug.Log("Tangled", gameObject);
    }

    private void UnTangle()
    {
        m_player.tangled = false;
        Destroy(gameObject);
    }
}

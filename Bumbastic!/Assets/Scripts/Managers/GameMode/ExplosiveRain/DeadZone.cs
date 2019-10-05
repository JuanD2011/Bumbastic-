using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private bool canKill = true;
    private Player playerKilled = null;
    public static event System.Action<Player> OnPlayerKilled = null;

    private void Awake()
    {
        OnPlayerKilled = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null && canKill)
        {
            playerKilled = player;
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player killed");
        OnPlayerKilled?.Invoke(playerKilled);
        if (GameManager.Manager.Players.Count == 1) canKill = false;
    }
}

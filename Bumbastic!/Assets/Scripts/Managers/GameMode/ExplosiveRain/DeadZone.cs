using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public static event System.Action<Player> OnPlayerKilled;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null)
        {
            Debug.Log("Player killed");
            OnPlayerKilled?.Invoke(player);
        }
    }
}

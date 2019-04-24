using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] int lifePoints = 3;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null)
        {
            if (player.HasBomb)
            {
                if (lifePoints > 0)
                {
                    lifePoints--;
                }
                else
                {
                    Debug.Log("Base Destroyed");
                }
            }
        }
    }
}

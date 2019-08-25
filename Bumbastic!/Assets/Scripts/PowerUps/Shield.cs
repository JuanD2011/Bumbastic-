using System.Collections;
using UnityEngine;

public class Shield : PowerUp, IBounce
{
    float bounceForce = 12f;
    float timeToLerpBomb = 0.7f;

    private void Awake()
    {
        GetPlayer();
    }

    private void Start()
    {
        m_player.Collider.enabled = false;
        Duration = 15f;
        Invoke("EndExecution", Duration);
    }

    private void EndExecution() { m_player.Collider.enabled = true; Destroy(gameObject); }

    public void Bounce(GameObject _bounceable, Collision _collision)
    {
        Bomb bomb = _bounceable.GetComponent<Bomb>();
        Player player = _bounceable.GetComponent<Player>();

        if (bomb != null)
        {
            StartCoroutine(LerpBomb(bomb.transform));
        }
        else if (player != null && m_player != player)
        {
            player.Rigidbody.AddForce(-player.transform.forward * bounceForce, ForceMode.Impulse);
        }
    }

    private Transform NearestPlayer()
    {
        Transform nearestPlayer = null;
        float minDist = Mathf.Infinity;

        foreach (Player player in HotPotatoManager.HotPotato.Players)
        {
            if (m_player != player)
            {
                Vector3 heading = transform.position - player.transform.position;
                float dist = heading.magnitude;

                if (dist < minDist)
                {
                    nearestPlayer = player.transform;
                    minDist = dist;
                } 
            }
        }
        return nearestPlayer;
    }

    IEnumerator LerpBomb(Transform _bomb)
    {
        float elapsedTime = 0f;
        Vector3 initPos = _bomb.position;
        Transform finalPos = NearestPlayer();

        while (_bomb.parent == null)
        {
            _bomb.position = Vector3.Lerp(initPos, finalPos.position, elapsedTime / timeToLerpBomb);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

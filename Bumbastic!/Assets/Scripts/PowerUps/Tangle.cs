using System.Collections;
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
        StartCoroutine(Tangled());
    }

    private IEnumerator Tangled()
    {
        m_player.InputDirection *= -1;
        yield return new WaitForSeconds(Duration);
        m_player.InputDirection *= -1;
    }
}

using System.Collections;
using UnityEngine;

public class ExplosiveCloudSpawner : MonoBehaviour
{
    [SerializeField]
    private float minSpawnRate = 0f, maxSpawnRate = 0f;

    private Collider spawnVolume = null;

    private BombPool pool = null;

    private void Awake()
    {
        spawnVolume = GetComponent<Collider>();
        pool = GetComponent<BombPool>();
    }

    private void Start()
    {
        StartCoroutine(RainStart());
        InvokeRepeating("ReduceSpawnTime", 5f, 5f);
    }

    private void SpawnBomb()
    {
        Bomb bomb = pool.GetAvailableBomb();
        bomb.gameObject.SetActive(true);
        bomb.transform.position = spawnVolume.GetRandomPointInVolume();
    }

    private IEnumerator RainStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
            SpawnBomb();
        }
    }

    private void ReduceSpawnTime()
    {
        minSpawnRate -= 0.15f;
        maxSpawnRate -= 0.15f;
    }
}

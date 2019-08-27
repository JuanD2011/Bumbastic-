using System.Collections;
using UnityEngine;

public class ExplosiveCloudSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab = null;

    [Header("Bomb spawn rate")]
    [SerializeField]
    private float minSpawnRate = 0f, maxSpawnRate = 0f;

    private Collider spawnVolume = null;

    private void Awake()
    {
        spawnVolume = GetComponent<Collider>();
    }

    private void Start()
    {
        StartCoroutine(RainStart());
    }

    private void SpawnBomb()
    {
        Instantiate(bombPrefab, spawnVolume.GetRandomPointInVolume(), Quaternion.identity);
    }

    private IEnumerator RainStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
            SpawnBomb();
        }
    }
}

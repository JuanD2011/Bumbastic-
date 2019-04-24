using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] float force = 1f;
    [SerializeField] float timeToSpawnBomb = 5f;
    [SerializeField] float xRandom = 25f, zRandom = 25f;

    Collider spawner;

    Bomb bombToSpawn;

    private void Start()
    {
        spawner = GetComponent<Collider>();

        InvokeRepeating("SpawnBomb", timeToSpawnBomb, timeToSpawnBomb);
    }

    private void SpawnBomb()
    {
        bombToSpawn = BombPool.instance.GetAvailableBomb();
        bombToSpawn.transform.position = spawner.GetPointInVolume();
        bombToSpawn.RigidBody.AddForce(GetRandomVector() * force, ForceMode.Impulse);
    }

    private Vector3 GetRandomVector()
    {
        Vector3 result = Vector3.up;

        result = new Vector3(Random.Range(-xRandom, xRandom), 1f, Random.Range(-zRandom, zRandom));

        return result;
    }
}

public static class SpawnerExtensions
{
    public static Vector3 GetPointInVolume(this Collider collider)
    {
        Vector3 result = Vector3.zero;
        result = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), collider.transform.position.y, 0F);

        return result;
    }
}
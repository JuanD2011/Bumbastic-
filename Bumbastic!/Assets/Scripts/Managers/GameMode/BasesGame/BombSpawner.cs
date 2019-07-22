using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] float force = 5f;
    [SerializeField] float timeToSpawnBomb = 5f;
    [SerializeField] float xRandom = 25f, zRandom = 25f;

    Collider spawner;

    Bomb bombToSpawn = null;

    BombPool m_bombPool = null;

    private void Start()
    {
        spawner = GetComponent<Collider>();
        m_bombPool = GetComponent<BombPool>();

        InvokeRepeating("SpawnBomb", timeToSpawnBomb, timeToSpawnBomb);
    }

    private void SpawnBomb()
    {
        bombToSpawn = m_bombPool.GetAvailableBomb();

        if (bombToSpawn != null)
        {
            bombToSpawn.gameObject.SetActive(true);
            bombToSpawn.transform.position = spawner.GetPointInVolume();
            bombToSpawn.RigidBody.AddForce(GetRandomVector() * force, ForceMode.Impulse);   
        }
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
        result = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), 0f, 0F);

        return result;
    }
}
using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] float force = 5f;
    [SerializeField] float timeToSpawnBomb = 5f;
    [SerializeField] float xRandom = 25f, zRandom = 25f;

    Collider spawner;

    Bomb bombToSpawn = null;

    BombPool m_bombPool = null;
    WaitForSeconds waitForSeconds = null;

    private void Awake()
    {
        spawner = GetComponent<Collider>();
        m_bombPool = GetComponent<BombPool>();

        waitForSeconds = new WaitForSeconds(timeToSpawnBomb);
    }

    private void Start()
    {
        GameManager.Manager.Director.stopped += (UnityEngine.Playables.PlayableDirector _Director) => StartCoroutine(SpawnBomb());
    }

    private IEnumerator SpawnBomb()
    {
        bombToSpawn = m_bombPool.GetAvailableBomb();

        if (bombToSpawn != null)
        {
            bombToSpawn.gameObject.SetActive(true);
            bombToSpawn.transform.position = spawner.GetPointInVolume();
            bombToSpawn.RigidBody.velocity = Vector3.zero;
            bombToSpawn.RigidBody.AddForce(GetRandomVector() * force, ForceMode.Impulse);   
        }
        yield return waitForSeconds;
        StartCoroutine(SpawnBomb());
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
        result = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), collider.bounds.max.y, 0f);

        return result;
    }
}
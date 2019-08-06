using UnityEngine;

public class SpawnLine : MonoBehaviour
{
    [SerializeField] Transform[] spawnLine = new Transform[2];
    [SerializeField] float spacing = 0f;

    float midPoint = 0f;
    Vector3 lineVector = Vector3.zero;
    private float distanceBetweenPlayers = 0f;

    private void Start()
    {
        lineVector = spawnLine[0].position - spawnLine[1].position;
        midPoint = Mathf.Abs((spawnLine[0].position.x - spawnLine[1].position.x)) / 2;
    }

    public void InitDistanceBetweenPlayers()
    {
        distanceBetweenPlayers = lineVector.magnitude / (MenuManager.menu.Players.Count + 1);
    }

    public Vector3 GetSpawnPoint(int _index)
    {
        Vector3 result = Vector3.zero;

        if (distanceBetweenPlayers * _index > midPoint)
        {
            result = spawnLine[0].localPosition + new Vector3(distanceBetweenPlayers * _index + spacing, 0f, 0f);
        }
        else if (distanceBetweenPlayers * _index < midPoint)
        {
            result = spawnLine[0].localPosition + new Vector3(distanceBetweenPlayers * _index - spacing, 0f, 0f);
        }
        else
        {
            result = spawnLine[0].localPosition + new Vector3(distanceBetweenPlayers * _index, 0f, 0f);
        }

        return result;
    }
}

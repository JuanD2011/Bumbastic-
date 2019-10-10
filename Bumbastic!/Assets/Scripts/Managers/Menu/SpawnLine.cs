using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnLine : MonoBehaviour
{
    [SerializeField] Transform[] spawnLine = new Transform[2];
    [SerializeField] Quaternion initialRotiation = Quaternion.identity;

    [Tooltip("Only works when the limit of player haven't been reached.")]
    [SerializeField] float spacing = 0f;

    bool useSpacing = true;

    float midPoint = 0f;
    Vector3 lineVector = Vector3.zero;
    private float distanceBetweenPlayers = 0f;

    List<PlayerMenu> activePlayers = new List<PlayerMenu>();

    public static event System.Action<List<PlayerMenu>> OnActivePlayersSorted = null;

    private void Awake()
    {
        OnActivePlayersSorted = null;
    }

    private void Start()
    {
        lineVector = spawnLine[0].position - spawnLine[1].position;
        midPoint = Mathf.Abs(spawnLine[0].position.x - spawnLine[1].position.x) / 2;
    }

    private void InitDistanceBetweenPlayers()
    {
        distanceBetweenPlayers = lineVector.magnitude / (GetNumberOfActivePlayers() + 1);
        //if (GetNumberOfActivePlayers() >= 4) useSpacing = false;
        //else useSpacing = true;
    }

    public void InitPlayersPosition()
    {
        activePlayers.Clear();
        InitDistanceBetweenPlayers();

        foreach (PlayerMenu playerMenu in MenuManager.menu.Players)
        {
            if (playerMenu.Avatar != null)
            {
                activePlayers.Add(playerMenu);
            }
        }

        if (activePlayers.Count <= 0) return;

        activePlayers = activePlayers.OrderBy(w => w.Id).ToList();

        for (int i = 0; i < activePlayers.Count; i++)
        {
            activePlayers[i].transform.GetChild(0).position = GetSpawnPoint(i + 1);
            activePlayers[i].transform.GetChild(0).rotation = initialRotiation;
        }

        OnActivePlayersSorted?.Invoke(activePlayers);
    }

    public void SetPlayerPosition(int _playerID)
    {
        for (int i = 0; i < activePlayers.Count; i++)
        {
            if (activePlayers[i].Id == _playerID)
            {
                activePlayers[i].transform.GetChild(1).position = GetSpawnPoint(i + 1);
                activePlayers[i].transform.GetChild(1).rotation = initialRotiation;
                break;
            }
        }
    }

    private int GetNumberOfActivePlayers()
    {
        int result = 0;

        foreach (PlayerMenu playerMenu in MenuManager.menu.Players)
        {
            if (playerMenu.Avatar != null)
            {
                result++;
            } 
        }

        return result;
    }

    private Vector3 GetSpawnPoint(int _index)
    {
        Vector3 result = Vector3.zero;

        if (distanceBetweenPlayers * _index > midPoint && useSpacing)
        {
            result = spawnLine[0].localPosition + new Vector3(distanceBetweenPlayers * _index + spacing, 0f, 0f);
        }
        else if (distanceBetweenPlayers * _index < midPoint && useSpacing)
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

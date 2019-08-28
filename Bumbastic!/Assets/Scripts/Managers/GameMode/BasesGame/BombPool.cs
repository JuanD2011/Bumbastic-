using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    [SerializeField] GameObject bombTemplate = null;

    [Header("Bombs to instantiate")]
    [Range(2, 100)]
    [SerializeField] int bombsInScene = 10;

    List<Bomb> bombsInGame = new List<Bomb>();

    private void Awake()
    {
        CreateBombs();
    }

    private void CreateBombs()
    {
        for (int i = 0; i < bombsInScene; i++)
        {
            GameObject bombClone = Instantiate(bombTemplate, Vector3.zero, Quaternion.identity);
            bombClone.name = string.Format("{0} bomb", i);
            bombClone.SetActive(false);
            bombsInGame.Add(bombClone.GetComponent<Bomb>());
        }
    }

    public Bomb GetAvailableBomb()
    {
        Bomb result = null;

        for (int i = 0; i < bombsInGame.Count; i++)
        {
            if (!bombsInGame[i].gameObject.activeInHierarchy)
            {
                return result = bombsInGame[i];
            }
        }

        GameObject bombClone = Instantiate(bombTemplate, Vector3.zero, Quaternion.identity);
        bombClone.name = string.Format("{0} bomb", bombsInGame.Count);
        bombClone.SetActive(false);
        bombsInGame.Add(bombClone.GetComponent<Bomb>());
        result = bombClone.GetComponent<Bomb>();

        return result;
    }
}

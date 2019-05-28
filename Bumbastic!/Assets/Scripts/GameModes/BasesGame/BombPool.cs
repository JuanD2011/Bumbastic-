using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    public static BombPool instance;

    [SerializeField] GameObject bombTemplate = null;

    [Header("Bombs to instantiate")]
    [Range(2, 10)]
    [SerializeField] int bombsInScene = 10;

    List<Bomb> bombsInGame = new List<Bomb>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

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
        Bomb result;

        for (int i = 0; i < bombsInGame.Count; i++)
        {
            if (!bombsInGame[i].gameObject.activeInHierarchy)
            {
                return result = bombsInGame[i];
            }
        }

        GameObject bombGO = Instantiate(bombTemplate, Vector3.zero, Quaternion.identity);
        result = bombGO.GetComponent<Bomb>();
        bombsInGame.Add(result);
        bombGO.name = string.Format("{0} bomb", bombsInGame.Count - 1);

        return result;
    }
}

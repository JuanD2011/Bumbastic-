using UnityEngine;

public class FreeForAllManager : HotPotatoManager
{
    public static FreeForAllManager FreeForAll;

    private bool gameOver;

    [SerializeField]
    private byte maxScore = 3;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        if (cooldown)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                cooldown = false;
                time = 0;
                GiveBomb();
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnBombExplode()
    {
        transmitter.transform.position = transmitter.SpawnPoint;
        cooldown = true;
    }

    protected void GiveBomb()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            if (InGame.playerSettings[i].score == maxScore)
            {
                gameOver = true;
            }
        }

        if (!gameOver)
        {
            int random = Random.Range(0, Players.Count);

            Bomb.transform.position = Players[random].transform.position + new Vector3(0, 2, 0);
            Bomb.Timer = Random.Range(minTime, maxTime);
            Bomb.Exploded = false;
            if (Bomb.RigidBody != null)
            {
                Bomb.RigidBody.velocity = Vector3.zero;
            }
            Bomb.transform.rotation = Quaternion.identity;
            Bomb.gameObject.SetActive(true);
        }
        else
        {
            GameOver();
        }
    }
}

﻿using UnityEngine;

public class FreeForAllManager : HotPotatoManager
{
    public static FreeForAllManager FreeForAll;

    private bool gameOver;

    [SerializeField]
    private byte maxKills = 3;

    byte[] killsCounter;
    byte winnerID = 0;

    private Player lastPlayerGiven;

    private byte timesBombPlayed = 0;

    public byte[] KillsCounter { get => killsCounter; private set => killsCounter = value; }
    public byte WinnerID { get => winnerID; private set => winnerID = value; }

    public delegate void DelFreeForAll(byte _killerID);
    public event DelFreeForAll OnPlayerKilled;

    protected override void Awake()
    {
        base.Awake();

        if (FreeForAll == null) FreeForAll = this;
        else Destroy(this);
    }

    protected override void Start()
    {
        base.Start();
        KillsCounter = new byte[players.Count];
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

    protected override void OnBombExplode()
    {
        foreach (Player player in Players)
        {
            StartCoroutine(player.Rumble(0.8f, 0.8f, 1f));
        }

        lastPlayerGiven = BombHolder;
        Bomb.RigidBody.isKinematic = true;
        BombHolder.transform.position = BombHolder.SpawnPoint;

        if (transmitter != null)
        {
            if (timesBombPlayed > 1 && BombHolder.Id != transmitter.Id)
            {
                KillsCounter[transmitter.Id] += 1;
                OnPlayerKilled?.Invoke(transmitter.Id);
                transmitter = null;
            }
        }

        BombHolder = null;

        timesBombPlayed = 0;

        foreach (Player player in Players)
        {
            player.Collider.enabled = true;
        }
        
        cooldown = true;
    }

    protected void GiveBomb()
    {
        for (byte i = 0; i < killsCounter.Length; i++)
        {
            if (KillsCounter[i] == maxKills)
            {
                WinnerID = i;
                InGame.playerSettings[i].score += 1;
                gameOver = true;
                break;
            }
        }

        if (!gameOver)
        {
            int random = Random.Range(0, Players.Count);

            while (Players[random] == lastPlayerGiven)
            {
                random = Random.Range(0, Players.Count);
            }
            Bomb.Collider.enabled = true;
            Bomb.transform.position = Players[random].transform.position + new Vector3(0, 2, 0);
            Bomb.Timer = Random.Range(minTime, maxTime);
            Bomb.Exploded = false;
            Bomb.RigidBody.velocity = Vector3.zero;
            Bomb.transform.rotation = Quaternion.identity;
            Bomb.gameObject.SetActive(true);
            OnBombArmed?.Invoke();//Bomb hears it.
        }
        else
        {
            GameOver();
        }
    }

    protected override void BombHolderChange(Player _player, Bomb _bomb)
    {
        base.BombHolderChange(_player, _bomb);
        if (!Bomb.Exploded) timesBombPlayed++;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}

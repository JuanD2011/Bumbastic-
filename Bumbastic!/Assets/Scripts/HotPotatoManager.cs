using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HotPotatoManager : GameManager
{
    public static HotPotatoManager Manager;

    private Player bombHolder;

    [SerializeField]
    private Bomb bomb;

    [SerializeField]
    private GameObject confettiBomb;

    [SerializeField]
    private float minTime, maxTime;

    private bool cooldown;
    private float time = 0;

    public Player BombHolder { get => bombHolder; private set => bombHolder = value; }

    private void Awake()
    {
        if (Manager == null) Manager = this;
        else Destroy(this);

        Players = new List<Player>();
        Director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        Bomb.OnExplode += StartNewRound;
    }

    private void Update()
    {
        if (cooldown)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                cooldown = false;
                time = 0;
                GiveBombs();
            }
        }
    }

    private void StartNewRound()
    {
        Players.Remove(BombHolder);
        BombHolder.gameObject.SetActive(false);

        foreach (Player player in Players)
        {
            player.transform.position = player.SpawnPoint;
            player.CanMove = false;
        }

        cooldown = true;
    }

    public void GiveBombs()
    {
        if (Players.Count > 1)
        {
            bummies = RandomizeBummieList();

            int[] _bummies = new int[bummies.Count];

            Director.Play();

            for (int i = 0; i < bummies.Count; i++)
            {
                Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 6, 0), Quaternion.identity);
                bummies.RemoveAt(i);
            }
            bomb.transform.position = bummies[0].transform.position + new Vector3(0, 6, 0);
            bomb.Timer = Random.Range(minTime -= 3f, maxTime -= 3f);
            bomb.Exploded = false;
            if (bomb.RigidBody != null)
            {
                bomb.RigidBody.velocity = Vector3.zero;
            }
            bomb.transform.rotation = Quaternion.identity;
            bomb.gameObject.SetActive(true);
        }
        else if (Players.Count == 1)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Pass bomb to the player that the bomb touch
    /// </summary>
    /// <param name="_receiver"></param>
    public void PassBomb(Player _receiver)
    {
        if (BombHolder != null)
        {
            BombHolder.HasBomb = false;
            BombHolder.Collider.enabled = true;
        }
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;
        BombHolder = _receiver;
        this.Bomb.RigidBody.isKinematic = true;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult.transform);
    }

    /// <summary>
    /// Pass bomb between players when one touch another
    /// </summary>
    /// <param name="_receiver"></param>
    /// <param name="_transmitter"></param>
    public void PassBomb(Player _receiver, Player _transmitter)
    {
        _transmitter.HasBomb = false;
        _transmitter.Collider.enabled = true;
        _receiver.HasBomb = true;
        _receiver.Collider.enabled = false;
        BombHolder = _receiver;
        this.Bomb.RigidBody.isKinematic = true;
        Bomb.transform.position = _receiver.Catapult.position;
        Bomb.transform.SetParent(_receiver.Catapult);
    }

    public void PassBomb()
    {
        BombHolder.HasBomb = true;
        this.Bomb.RigidBody.isKinematic = true;
        Bomb.transform.position = BombHolder.Catapult.position;
        Bomb.transform.SetParent(BombHolder.Catapult);
    }
}

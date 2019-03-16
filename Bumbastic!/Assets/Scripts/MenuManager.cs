using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private float startTimer = 5f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI[] texts;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private GameObject playerMenuPrefab;

    private bool countdown = false;

    private byte playersReady = 0;

    public delegate void DelMenuManager(string _sceneName);
    public DelMenuManager OnStartGame;

    public delegate void SetCountdown(bool _bool);
    public SetCountdown OnCountdown;

    public delegate void InputsDelegate(List<PlayerMenu> _players);
    public InputsDelegate OnFirstPlayers;
  
    private void Awake()
    {
        if (menu == null) menu = this;
        else Destroy(this);
    }

    void Start()
    {
        timer = startTimer;
        InputManager.StartInputs += InitializeFirstPlayers;
    }

    void Update()
    {
        if (countdown)
        {
            timer -= Time.deltaTime;

            countdownText.text = string.Format("{0}", Mathf.RoundToInt(timer));

            if (timer <= 0f)
            {
                if (countdownText.text != "")
                {
                    countdownText.text = "";
                }
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        OnStartGame?.Invoke("Game");//MenuUI hears it.
        Debug.Log("The game has started");
    }

    private void InitializeFirstPlayers(byte _number)
    {
        List<PlayerMenu> players = new List<PlayerMenu>();

        for (int i = 0; i < _number; i++)
        {
            PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
            players.Add(player);
        }

        OnFirstPlayers?.Invoke(players);
    }

    public void PlayersReady(byte id)
    {
        playersReady++;
        texts[id].text = "Ready";
        if (playersReady == inGame.maxPlayers)
        {
            countdown = true;
            OnCountdown?.Invoke(true);//MenuUI hears it.
        }
        else
        {
            countdown = false;
        }
    }

    public void PlayerNotReady(byte id)
    {
        playersReady--;
        texts[id].text = "Press Start";
        countdown = false;
        timer = startTimer;
    }
}

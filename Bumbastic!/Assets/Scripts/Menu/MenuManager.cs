using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    [SerializeField]
    private InGame inGame;

    [SerializeField]
    private GameModeDataBase gameMode = null;

    [SerializeField]
    Settings settings = null;

    [SerializeField]
    private float startTimer = 5f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI[] texts = new TextMeshProUGUI[0];

    [SerializeField]
    private TextMeshProUGUI[] playersIDs = new TextMeshProUGUI[0];

    [SerializeField]
    private Image[] playerColors = new Image[0];

    [SerializeField]
    private TextMeshProUGUI countdownText = null;

    [SerializeField]
    private GameObject playerMenuPrefab = null;

    private bool countdown = false;
    bool go = false;

    private byte playersReady = 0;
    private int maxPlayers = 0;

    List<PlayerMenu> players = new List<PlayerMenu>();

    public delegate void DelMenuManager(string _sceneName);
    public DelMenuManager OnStartGame;

    public delegate void SetCountdown(bool _bool);
    public SetCountdown OnCountdown;

    public List<PlayerMenu> Players { get => players; private set => players = value; }

    public MenuCanvas menuCanvas;

    private void Awake()
    {
        if (menu == null) menu = this;
        else Destroy(this);

        Memento.LoadData();
        SetLanguage();
    }

    private void SetLanguage()
    {
        Translation.currentLanguageId = settings.languageID;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.currentLanguageId]);
    }

    void Start()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips.inGameMusic, 0.6f, 0.6f, 0.6f);
        timer = startTimer;

        PlayerMenu.OnReady += PlayersReady;
        PlayerMenu.OnNotReady += PlayerNotReady;
        SkinManager.OnSkinsSet += SetUpMatchMaking;
        InputManager.inputManager.OnDeviceAdded += AddNewPlayer;
        InputManager.inputManager.OnDeviceAdded += SetPlayersColor;

        SetPlayersColor();
    }


    void Update()
    {
        if (countdown)
        {
            timer -= Time.deltaTime;

            if (Mathf.RoundToInt(timer) <= 0)
            {
                if (!go)
                {
                    //AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.go, 1f);
                    switch (Translation.GetCurrentLanguage())
                    {
                        case Languages.en:
                            countdownText.text = "Go!";
                            break;
                        case Languages.es:
                            countdownText.text = "¡Vamos!";
                            break;
                        case Languages.unknown:
                            countdownText.text = "Go!";
                            break;
                        default:
                            break;
                    }
                    go = true;
                }
            }
            else
                countdownText.text = string.Format("{0}", Mathf.RoundToInt(timer));

            if (timer <= 0f)
            {
                if (countdownText.text != "")
                {
                    countdownText.text = "";
                }
                StartGame();
                countdown = false;
            }
        }
    }

    private void AddNewPlayer()
    {
        PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
        player.Id = (byte)Players.Count;
        Players.Add(player);
        maxPlayers = Players.Count;
    }

    private void SetPlayersColor()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Color = settings.playersColor[i];
            playerColors[i].color = Players[i].Color;
        }
    }

    private void SetUpMatchMaking(bool _canActive)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            playerColors[i].enabled = _canActive;
            playersIDs[i].enabled = _canActive;
            texts[i].enabled = _canActive;
        }
    }

    private void StartGame()
    {
        InGame.playerSettings.Clear();
        for (int i = 0; i < Players.Count; i++)
        {
            InGame.playerSettings.Add(new PlayerSettings(Players[i].PrefabName, Players[i].Avatar, Players[i].SkinSprite, Players[i].Color));
        }
        gameMode.GetNextGameMode();
        OnStartGame?.Invoke("GameMode");//MenuUI hears it.
    }

    public void InitializeFirstPlayers(int _gamepadCount)
    {
        for (int i = 0; i < _gamepadCount; i++)
        {
            PlayerMenu player = Instantiate(playerMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerMenu>();
            player.Id = (byte) i;
            Players.Add(player);
        }
        maxPlayers = _gamepadCount;
        //InputManager.inputManager.AssignController(_joysticks);
    }

    public void PlayersReady(byte _id)
    {
        playersReady++;

        switch (Translation.GetCurrentLanguage())
        {
            case Languages.en:
                texts[_id].text = "Ready"; 
                break;
            case Languages.es:
                texts[_id].text = "¡Listo!"; 
                break;
            case Languages.unknown:
                texts[_id].text = "Ready"; 
                break;
            default:
                break;
        }

        if (playersReady == maxPlayers && maxPlayers >= 2)
        {
            countdown = true;
            OnCountdown?.Invoke(true);//MenuUI hears it.
        }
    }

    public void PlayerNotReady(byte _id)
    {
        playersReady--;
        switch (Translation.GetCurrentLanguage())
        {
            case Languages.en:
                texts[_id].text = "Press Start";
                break;
            case Languages.es:
                texts[_id].text = "Presiona Start";
                break;
            case Languages.unknown:
                texts[_id].text = "Press Start";
                break;
            default:
                break;
        }
        countdown = false;
        timer = startTimer;
        OnCountdown?.Invoke(false);//MenuUI hears it.
    }

    public void SaveData(int _id)
    {
        Memento.SaveData(_id);
    }
}

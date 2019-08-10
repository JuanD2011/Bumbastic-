using System;
using UnityEngine;

public class MenuCanvas : CanvasBase
{
    [SerializeField] string levelToLoad;

    [SerializeField] PlayerHUD[] playerHUDs = new PlayerHUD[4];

    public static bool isMatchmaking = false;

    public static event Action<bool> OnMatchmaking;

    protected override void Awake()
    {
        base.Awake();
        OnMatchmaking = null;
        isMatchmaking = false;
    }

    private void Start()
    {
        PlayerMenu.OnBackButton += BackButton;

        MenuManager.menu.OnStartGame += LoadingScreen;
        MenuManager.menu.OnCountdown += Countdown;
        MenuManager.menu.OnNewPlayerAdded += UpdateHUD;

        PlayerInputHandler.OnPlayerDeviceLost += UpdateHUD;
        PlayerInputHandler.OnPlayerDeviceRegained += UpdateHUD;

        SkinManager.OnSkinsSet += InitializeHUD;
    }

    private void UpdateHUD(byte _playerID)
    {
        if (!playerHUDs[_playerID].gameObject.activeInHierarchy) playerHUDs[_playerID].gameObject.SetActive(true);
        else playerHUDs[_playerID].gameObject.SetActive(false);
    }

    private void InitializeHUD()
    {
        for (int i = 0; i < MenuManager.menu.Players.Count; i++)
        {
            if (!playerHUDs[i].gameObject.activeInHierarchy) playerHUDs[i].gameObject.SetActive(true);
        }
    }

    #region AnimationEvents
    public void OnLoadScreenComplete()
    {
        StartCoroutine(OnLoadScene?.Invoke(levelToLoad));//Lvl Manager hears it.
    }
    #endregion

    #region AnimatorStates
    private void Countdown(bool _bool)
    {
        m_Animator.SetBool("Countdown", _bool);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_SceneName">name of the scene to be loaded by LvlMgr</param>
    public void LoadingScreen(string _SceneName)
    {
        levelToLoad = _SceneName;
        m_Animator.SetTrigger("StartGame");
    }

    public void MatchmakingPanel(bool _bool)
    {
        isMatchmaking = _bool;
        m_Animator.SetBool("Play",_bool);

        OnMatchmaking?.Invoke(_bool);
    }

    public void ConfigurationPanel(bool _bool)
    {
        m_Animator.SetBool("Menu_Options", _bool);
    }

    public void CreditsPanel(bool _bool)
    {
        m_Animator.SetBool("Credits", _bool);
    }

    public void QuitPanel(bool _bool)
    {
        m_Animator.SetBool("QuitPanel", _bool);
    }
    #endregion

    public void BackButton(byte _id)
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.buttonBack, 0.6f);

        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animatorStateNames[0]))//Principal Menu
        {
            QuitPanel(true);
        }
        else if (stateInfo.IsName(animatorStateNames[1]))//Options Panel
        {
            ConfigurationPanel(false);
            Memento.SaveData(0);
        }
        else if (stateInfo.IsName(animatorStateNames[2]) || stateInfo.IsName(animatorStateNames[6]))//Matchmaking
        {
            MatchmakingPanel(false);
        }
        else if (stateInfo.IsName(animatorStateNames[3]))//Credits
        {
            CreditsPanel(false);
        }
        else if (stateInfo.IsName(animatorStateNames[4]))//Quit panel
        {
            QuitPanel(false);
        }
        else if (stateInfo.IsName(animatorStateNames[5]))//Countdown
        {
            Countdown(false);
        }
    }

    private void OnDisable()
    {
        isMatchmaking = false;
    }
}
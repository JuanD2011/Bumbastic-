using System.Collections;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;
    [SerializeField] Settings settings;

    [SerializeField] string[] stateName;

    [SerializeField] string levelToLoad;

    public delegate void DelMenuUI(bool _canActive);
    public static DelMenuUI OnMatchmaking;

    public delegate IEnumerator DelLoadScene(string _scene);
    public static DelLoadScene OnLoadScene;

    public static bool isMatchmaking = false;

    private void Awake()
    {
        OnMatchmaking = null;
        OnLoadScene = null;
    }

    private void Start()
    {

        PlayerMenu.OnBackButton += BackButton;
        MenuManager.menu.OnStartGame += LoadingScreen;
        MenuManager.menu.OnCountdown += Countdown;
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
        canvasAnimator.SetBool("Countdown",_bool);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_SceneName">name of the scene to be loaded by LvlMgr</param>
    public void LoadingScreen(string _SceneName)
    {
        levelToLoad = _SceneName;
        canvasAnimator.SetTrigger("StartGame");
    }

    public void MatchmakingPanel(bool _bool)
    {
        isMatchmaking = _bool;
        canvasAnimator.SetBool("Play",_bool);
        OnMatchmaking?.Invoke(_bool);//MenuCamManager, SkinManager, SkinSelector
    }

    public void ConfigurationPanel(bool _bool)
    {
        canvasAnimator.SetBool("Menu_Options", _bool);
    }

    public void CreditsPanel(bool _bool)
    {
        canvasAnimator.SetBool("Credits", _bool);
    }

    public void QuitPanel(bool _bool)
    {
        canvasAnimator.SetBool("QuitPanel", _bool);
    }
    #endregion

    private void BackButton(byte _id)
    {
        AnimatorStateInfo stateInfo = canvasAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(stateName[0]))//Principal Menu
        {
            QuitPanel(true);
        }
        else if (stateInfo.IsName(stateName[1]))//Options Panel
        {
            ConfigurationPanel(false);
            Memento.instance.SaveData(0);
        }
        else if (stateInfo.IsName(stateName[2]) || stateInfo.IsName(stateName[6]))//Matchmaking
        {
            MatchmakingPanel(false);
            OnMatchmaking?.Invoke(false);//MenuCamManager hears it.
        }
        else if (stateInfo.IsName(stateName[3]))//Credits
        {
            CreditsPanel(false);
        }
        else if (stateInfo.IsName(stateName[4]))//Quit panel
        {
            QuitPanel(false);
        }
        else if (stateInfo.IsName(stateName[5]))//Countdown
        {
            Countdown(false);
        }
    }
}
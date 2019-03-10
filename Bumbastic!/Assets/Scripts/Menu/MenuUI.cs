using System.Collections;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;
    [SerializeField] Settings settings;

    public delegate IEnumerator DelLoadString(string _scene);
    public static DelLoadString OnCanLoadScene;

    [SerializeField] string[] stateName;

    [SerializeField] InputManager inputManager;

    private void Start()
    {
        inputManager.UI.Back.performed += context => BackButton();
    }

    #region AnimationEvents
    public void OnLoadScreenComplete()
    {
        StartCoroutine(OnCanLoadScene?.Invoke("Menu"));//Lvl Manager hears it.
    }
    #endregion

    #region AnimatorStates
    public void LoadingScreen()
    {
        canvasAnimator.SetTrigger("StartGame");
    }

    public void PlayPanel(bool _bool)
    {
        canvasAnimator.SetBool("Play",_bool);
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

    private void BackButton()
    {
        AnimatorStateInfo stateInfo = canvasAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(stateName[0]))//Principal Menu
        {
            QuitPanel(true);
        }
        else if (stateInfo.IsName(stateName[1]))//Options Panel
        {
            ConfigurationPanel(false);
        }
        else if (stateInfo.IsName(stateName[2]))//Matchmaking
        {
            PlayPanel(false);
        }
        else if (stateInfo.IsName(stateName[3]))//Credits
        {
            CreditsPanel(false);
        }
        else if (stateInfo.IsName(stateName[4]))//Quit panel
        {
            QuitPanel(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }
    }
}
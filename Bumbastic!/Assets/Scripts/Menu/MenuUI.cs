using System;
using System.Collections;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;
    [SerializeField] Settings settings;

    public delegate void DelMenu();
    public static DelMenu OnLoadData;

    public delegate IEnumerator DelLoadString(string _scene);
    public static DelLoadString OnCanLoadScene;

    [SerializeField] string[] stateName;

    #region AnimationEvents
    public void OnLoadScreenComplete()
    {
        StartCoroutine(OnCanLoadScene?.Invoke("Menu"));//Lvl Manager hears it.
    }

    public void CanLoadData()
    {
        OnLoadData?.Invoke();//Memento hears it.
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName[0]))
            {
                QuitPanel(true);
            }
        }
    }
}
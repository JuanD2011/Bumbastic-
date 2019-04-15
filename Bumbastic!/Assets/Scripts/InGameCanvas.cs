using UnityEngine;
using TMPro;
using System.Collections;

public class InGameCanvas : MonoBehaviour
{
    Animator m_Animator;
    [SerializeField] TextMeshProUGUI textWinner;
    [SerializeField] TextMeshProUGUI[] textPlayerNames;

    public delegate IEnumerator DelLoadString(string _scene);
    public static DelLoadString OnLoadScene;

    bool isEndPanelActive = false;

    [SerializeField] string[] animatorStateNames;

    private void Awake()
    {
        OnLoadScene = null;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        GameManager.manager.OnGameOver += SetEndAnimation;
        PlayerMenu.OnStartButton += StartButton;

        SetScoreNames();
    }

    private void SetScoreNames()
    {
        for (int i = 0; i < textPlayerNames.Length; i++)
        {
            textPlayerNames[i].text = GameManager.manager.Players[i].PrefabName;
        }
    }

    private void StartButton()
    {
        if (isEndPanelActive)
        {
            AnimatorStateInfo animatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

            if (animatorStateInfo.IsName(animatorStateNames[0]))
            {
                m_Animator.SetTrigger("showScore");
            }
            else if (animatorStateInfo.IsName(animatorStateNames[1]))
            {
                m_Animator.SetTrigger("loadingScreen");
            }
        }
    }

    #region Animation events
    public void OnLoadScreenComplete()
    {
        StartCoroutine(OnLoadScene?.Invoke("Menu"));//Lvl Manager hears it.
    }
    #endregion

    private void SetEndAnimation()
    {
        isEndPanelActive = true;
        m_Animator.SetBool("isGameOver", true);
        textWinner.text = string.Format("{0}", GameManager.manager.Players[0].PrefabName);
    }
}

using UnityEngine;
using TMPro;

public class InGameCanvas : Canvas
{
    [SerializeField] TextMeshProUGUI textWinner;
    [SerializeField] PlayerScore[] playerScores;

    bool isEndPanelActive = false;
    private string _scene = "GameMode";

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Manager.OnGameOver += () => _scene = "Menu";

        GameManager.Manager.OnGameModeOver += SetEndAnimation;
        GameManager.Manager.OnGameModeOver += UpdateScore;
        PlayerMenu.OnStartButton += StartButton;

        SetPlayersScore();
    }

    private void UpdateScore()
    {
        for (int i = 0; i < playerScores.Length; i++)
        {
            for (int j = 0; j < InGame.playerSettings[i].score; j++)
            {
                if (!playerScores[i].Stars[j].enabled)
                {
                    playerScores[i].Stars[j].enabled = true; 
                }
            }
        }
    }

    private void SetPlayersScore()
    {
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i].InitComponents();
            playerScores[i].Name.text = InGame.playerSettings[i].name;

            for (int j = 0; j < InGame.playerSettings[i].score; j++)
            {
                playerScores[i].Stars[j].enabled = true;
            }
        }
    }

    private void StartButton(byte _id)
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
        StartCoroutine(OnLoadScene?.Invoke(_scene));//Lvl Manager hears it.
    }
    #endregion

    private void SetEndAnimation()
    {
        isEndPanelActive = true;
        m_Animator.SetTrigger("isGameOver");
        textWinner.text = string.Format("{0}", GameManager.Manager.Players[0].PrefabName);
    }
}

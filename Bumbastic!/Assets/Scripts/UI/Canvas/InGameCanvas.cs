using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameCanvas : Canvas
{
    [SerializeField] TextMeshProUGUI textWinner = null;
    [SerializeField] Image playerSprite = null;
    [SerializeField] PlayerScore[] playerScores = new PlayerScore[0];

    bool isEndPanelActive = false;
    private string _scene = "GameMode";

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Manager.OnGameOver += () => _scene = "Podium";

        GameManager.Manager.OnGameModeOver += Winner;
        GameManager.Manager.OnGameModeOver += UpdateScore;
        PlayerMenu.OnStartButton += StartButton;

        SetPlayersScore();
    }

    /// <summary>
    /// This is executed in timeline animation
    /// </summary>
    public void PlayBumbaSound()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bumba, 1f);
    }

    private void UpdateScore()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            for (int j = 0; j < InGame.playerSettings[i].score; j++)
            {
                playerScores[i].Stars[j].color = Color.white;
            }
        }
    }

    private void SetPlayersScore()
    {
        for (int i = 0; i < InGame.playerSettings.Count; i++)
        {
            if (!playerScores[i].transform.parent.gameObject.activeInHierarchy)
            {
                playerScores[i].transform.parent.gameObject.SetActive(true); 
            }
            playerScores[i].InitComponents();
            playerScores[i].PlayerSkinSprite.enabled = true;
            playerScores[i].PlayerSkinSprite.sprite = InGame.playerSettings[i].skinSprite;
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

    private void Winner()
    {
        isEndPanelActive = true;
        m_Animator.SetTrigger("isGameOver");

        if (GameModeDataBase.IsCurrentHotPotato())
        {
            textWinner.text = string.Format("{0}", GameManager.Manager.Players[0].PrefabName);
            playerSprite.sprite = InGame.playerSettings[GameManager.Manager.Players[0].Id].skinSprite;
        }
        else if (GameModeDataBase.IsCurrentFreeForAll())
        {
            textWinner.text = string.Format("{0}", InGame.playerSettings[FreeForAllManager.FreeForAll.WinnerID].name);
            playerSprite.sprite = InGame.playerSettings[FreeForAllManager.FreeForAll.WinnerID].skinSprite;
        }
        else if (GameModeDataBase.IsCurrentBasesGame())
        {
            //TODO show the information of the winner in bases gamemode.
        }
    }
}
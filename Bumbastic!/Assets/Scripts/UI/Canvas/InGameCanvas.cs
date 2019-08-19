using System.Collections;
using UnityEngine;

public class InGameCanvas : CanvasBase
{
    [SerializeField] PlayerScore[] playerScores = new PlayerScore[0];
    [SerializeField] StarObtained starObtained = null;
    [SerializeField] Continue @continue = null;

    bool isEndPanelActive = false;
    private string _scene = "GameMode";

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameManager.Manager.OnGameOver += () => _scene = "Podium";
        GameManager.Manager.OnGameModeOver += () =>
        {
            m_Animator.SetTrigger("isGameOver");
            starObtained.gameObject.SetActive(true);
        };

        PlayerMenu.OnStartButton += StartButton;

        starObtained.OnAnimationEnded += () => StartCoroutine(LerpStarToPlayer());
        @continue.OnAnimationEnded += () => isEndPanelActive = true;

        InitPlayersScore();
    }

    private void InitPlayersScore()
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

            for (int j = 0; j < InGame.playerSettings[i].score; j++)
            {
                playerScores[i].Stars[j].color = Color.white;
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
                m_Animator.SetTrigger("loadingScreen");
                @continue.Hide();
            }
        }
    }

    IEnumerator LerpStarToPlayer()
    {
        Vector3 initStarPos = starObtained.transform.position;
        Vector3 endStarPosition = playerScores[InGame.lastWinner.id].Stars[InGame.lastWinner.score - 1].transform.position;

        float elapsedTime = 0f;
        float lerpTime = 0.3f;

        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.dash, 0.2f);
        while (elapsedTime <= lerpTime)
        {
            starObtained.transform.position = Vector3.Lerp(initStarPos, endStarPosition, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerScores[InGame.lastWinner.id].Stars[InGame.lastWinner.score - 1].color = Color.white;
        starObtained.gameObject.SetActive(false);
        @continue.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method executed by animation.
    /// </summary>
    public void OnLoadScreenComplete() { StartCoroutine(OnLoadScene?.Invoke(_scene)); }
    /// <summary>
    /// This is executed throught timeline animation
    /// </summary>
    public void PlayBumbaSound() { AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.bumba, 1f); }
}
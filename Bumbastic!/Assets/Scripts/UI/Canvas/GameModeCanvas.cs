using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameModeCanvas : MonoBehaviour
{
    [SerializeField] GameModeManager gameModeManager = null;
    [SerializeField] GameObject bgImageContainer = null;
    [SerializeField] GameObject bgTemplate = null;

    [SerializeField] Continue @continue = null;

    [SerializeField] float timeToChangebg = 1f, alphaImage = 0.6f, alphaImageOut = 0.1f, fadeOut = 1f, fadeIn = 1f;

    [SerializeField] TextTranslation[] gamemodeInfo = new TextTranslation[2];

    Image[] backgroundImages = new Image[0];

    static bool showedIntstructions = false;
    private bool canContinueInstructions = false;

    int imageCount = 0;

    Animator m_Animator = null;

    public static bool ShowedInstructions { get => showedIntstructions; private set => showedIntstructions = value; }

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();

        if (!ShowedInstructions)
        {
            m_Animator.SetBool("ShowInstructions", true);
        }
        SetBackgrounds();
        SetGamemodeInformation();

    }

    private void Start()
    {
        PlayerMenu.OnStartButton += SetGamepad;
    }

    /// <summary>
    /// This is called by animation clip 
    /// </summary>
    public void OnInstructionsComplete()
    {
        canContinueInstructions = true;
    }

    private void SetGamepad(byte _id)
    {
        if (canContinueInstructions)
        {
            m_Animator.SetBool("ShowInstructions", false);
            showedIntstructions = true;
            gameModeManager.LoadScene();
            @continue.Hide();
        }
    }

    private void SetBackgrounds()
    {
        for (int i = 0; i < GameModeDataBase.currentGameMode.GameModeBackgrounds.Length; i++)
        {
            GameObject bgTemplateClon = Instantiate(bgTemplate, bgImageContainer.transform);
            bgTemplateClon.name = string.Format("background {0}", i);
            bgTemplateClon.GetComponent<Image>().sprite = GameModeDataBase.currentGameMode.GameModeBackgrounds[i];

            if (i > 0)
            {
                bgTemplateClon.SetActive(false);
            }
        }

        backgroundImages = bgImageContainer.GetComponentsInChildren<Image>(true);

        foreach (Image background in backgroundImages)
        {
            background.canvasRenderer.SetAlpha(alphaImageOut);
        }
        StartCoroutine(FirstImage());
    }

    private void SetGamemodeInformation()
    {
        if (GameModeDataBase.currentGameMode != null)
        {
            switch (GameModeDataBase.currentGameMode.GameModeType)
            {
                case GameModeType.HotPotato:
                    gamemodeInfo[0].TextID = GameModeType.HotPotato.ToString();
                    gamemodeInfo[1].TextID = string.Format("{0}Des", GameModeType.HotPotato.ToString());
                    break;
                case GameModeType.FreeForAll:
                    gamemodeInfo[0].TextID = GameModeType.FreeForAll.ToString();
                    gamemodeInfo[1].TextID = string.Format("{0}Des", GameModeType.FreeForAll.ToString());
                    break;
                case GameModeType.BasesGame:
                    gamemodeInfo[0].TextID = GameModeType.BasesGame.ToString();
                    gamemodeInfo[1].TextID = string.Format("{0}Des", GameModeType.BasesGame.ToString());
                    break;
                case GameModeType.ExplosiveRain:
                    gamemodeInfo[0].TextID = GameModeType.ExplosiveRain.ToString();
                    gamemodeInfo[1].TextID = string.Format("{0}Des", GameModeType.ExplosiveRain.ToString());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator FirstImage()
    {
        backgroundImages[imageCount].CrossFadeAlpha(alphaImage, fadeIn, false);
        yield return new WaitUntil(() => backgroundImages[imageCount].canvasRenderer.GetAlpha() == alphaImage);
        StartCoroutine(ChangeBackgroundImages());
    }

    IEnumerator ChangeBackgroundImages()
    {
        yield return new WaitForSeconds(timeToChangebg);

        backgroundImages[imageCount].CrossFadeAlpha(alphaImageOut, fadeOut, false);

        yield return new WaitUntil(() => backgroundImages[imageCount].canvasRenderer.GetAlpha() == alphaImageOut);

        backgroundImages[imageCount].gameObject.SetActive(false);
        backgroundImages[imageCount].canvasRenderer.SetAlpha(alphaImageOut);

        if (imageCount != backgroundImages.Length - 1)
        {
            imageCount++;
        }
        else
        {
            imageCount = 0;
        }

        backgroundImages[imageCount].gameObject.SetActive(true);
        backgroundImages[imageCount].CrossFadeAlpha(alphaImage, fadeIn, false);

        yield return new WaitUntil(() => backgroundImages[imageCount].canvasRenderer.GetAlpha() == alphaImage);

        StartCoroutine(ChangeBackgroundImages());
    }
}
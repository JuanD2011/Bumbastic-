using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameModeCanvas : MonoBehaviour
{
    [SerializeField] GameObject bgImageContainer = null;
    [SerializeField] GameObject bgTemplate = null;

    [SerializeField] TextMeshProUGUI gameModeName = null, gamemodeDescription = null;
    [SerializeField] float timeToChangebg = 1f, alphaImage = 0.6f, alphaImageOut = 0.1f, fadeOut = 1f, fadeIn = 1f;

    [SerializeField] TextTranslation[] gamemodeInfo = new TextTranslation[2];

    string[] keysForTranslation = { "HotPotato", "HotPotatoDes", "FreeForAll", "FreeForAllDes" };

    Image[] backgroundImages;

    int imageCount = 0;

    static bool willShowGamepad = true;
    [SerializeField] Image gamepad;

    public static bool WillShowGamepad { get => willShowGamepad; private set => willShowGamepad = value; }

    private void Awake()
    {
        if (WillShowGamepad)
        {
            StartCoroutine(SetGamepad()); 
        }
        SetBackgrounds();
        SetGamemodeInformation();
    }

    private IEnumerator SetGamepad()
    {
        gamepad.enabled = true;

        yield return new WaitForSeconds(5f);

        gamepad.CrossFadeAlpha(0f, 0.6f, false);

        yield return new WaitUntil(() => gamepad.canvasRenderer.GetAlpha() == 0f);

        gamepad.enabled = false;
        WillShowGamepad = false;
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
            if (GameModeDataBase.IsCurrentHotPotato())
            {
                gamemodeInfo[0].TextID = keysForTranslation[0];
                gamemodeInfo[1].TextID = keysForTranslation[1];
            }
            else if (GameModeDataBase.IsCurrentFreeForAll())
            {
                gamemodeInfo[0].TextID = keysForTranslation[2];
                gamemodeInfo[1].TextID = keysForTranslation[3];
            }
            else if (GameModeDataBase.IsCurrentBasesGame())
            {
                //TODO Implement translation for bases gamemode.
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
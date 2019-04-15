using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CanvasGameMode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameModeName, gamemodeDescription;
    [SerializeField] Image image;

    [SerializeField] float timeToChangebg = 4f, alphaImage = 0.6f, fadeOut = 1f, fadeIn = 1f;
    [SerializeField] Image[] backgroundImages;

    [SerializeField] Color[] colors;

    int imageCount = 0;

    private void Start()
    {
        InitCanvas();
        StartCoroutine(FirstImage(fadeIn, alphaImage));
    }

    private void InitCanvas()
    {
        foreach (Image background in backgroundImages)
        {
            background.color = colors[0];
        }

        if (GameModeDataBase.currentGameMode != null)
        {
            gameModeName.text = GameModeDataBase.currentGameMode.name;
            gamemodeDescription.text = GameModeDataBase.currentGameMode.description; 
        }
    }

    IEnumerator FirstImage(float _fadeIn, float _alphaImage)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeIn)
        {
            backgroundImages[imageCount].color = Color.Lerp(colors[0], colors[1], elapsedTime / _fadeIn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ChangeBackgroundImages(timeToChangebg, fadeOut, fadeIn, alphaImage));
    }

    IEnumerator ChangeBackgroundImages(float _timeToChange, float _fadeOut, float _fadeIn, float _alphaImage)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_timeToChange);
        float elapsedTime = 0f;

        yield return waitForSeconds;

        while (elapsedTime < _fadeOut)
        {
            backgroundImages[imageCount].color = Color.Lerp(colors[1], colors[0], elapsedTime / _fadeOut);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        backgroundImages[imageCount].gameObject.SetActive(false);

        if (imageCount != backgroundImages.Length - 1)
        {
            imageCount++;
        }
        else
        {
            imageCount = 0;
        }

        backgroundImages[imageCount].gameObject.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < _fadeIn)
        {
            backgroundImages[imageCount].color = Color.Lerp(colors[0], colors[1], elapsedTime / _fadeIn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ChangeBackgroundImages(timeToChangebg, fadeOut, fadeIn, alphaImage));
    }
}

﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameModeCanvas : MonoBehaviour
{
    [SerializeField] GameObject bgImageContainer = null;
    [SerializeField] GameObject bgTemplate = null;

    [SerializeField] Color[] colors = new Color[0];
    [SerializeField] TextMeshProUGUI gameModeName = null, gamemodeDescription = null;
    [SerializeField] float timeToChangebg = 4f, alphaImage = 0.6f, fadeOut = 1f, fadeIn = 1f;

    [SerializeField] TextTranslation[] gamemodeInfo = new TextTranslation[2];

    string[] keysForTranslation = { "HotPotato", "HotPotatoDes", "FreeForAll", "FreeForAllDes" };

    Image[] backgroundImages;

    int imageCount = 0;

    private void Awake()
    {
        CreateBackgrounds();
    }

    private void Start()
    {
        InitCanvas();
        StartCoroutine(FirstImage(fadeIn, alphaImage));
    }

    private void CreateBackgrounds()
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
    }

    private void InitCanvas()
    {
        backgroundImages = bgImageContainer.GetComponentsInChildren<Image>(true);

        foreach (Image background in backgroundImages)
        {
            background.color = colors[0];
        }

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
            }//TODO Implement translation for bases gamemode.
            //gameModeName.text = GameModeDataBase.currentGameMode.Name;
            //gamemodeDescription.text = GameModeDataBase.currentGameMode.Description;   
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
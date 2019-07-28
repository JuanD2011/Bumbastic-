﻿using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] float timeToLoad = 10f;

    public delegate IEnumerator DelCanvasGameMode(string _sceneToLoad);
    public static DelCanvasGameMode onLoadScene;

    private void Start()
    {
        if (GameModeCanvas.ShowedInstructions)
        {
            LoadScene();
        }
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips.transitionMusic, 0.3f, 0.6f, 5f);
    }

    public void LoadScene()
    {
        Invoke("SceneToLoad", timeToLoad);
    }

    private void SceneToLoad()
    {
        StartCoroutine(onLoadScene?.Invoke(GameModeDataBase.currentGameMode.GameModeType.ToString()));
    }

    private void OnDisable()
    {
        onLoadScene = null;
        PlayerMenu.ResetDel();
    }
}
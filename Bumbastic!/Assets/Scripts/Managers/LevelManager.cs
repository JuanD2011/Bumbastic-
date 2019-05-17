using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Start()
    {
        Canvas.OnLoadScene += LoadAsynchronously;
        GameModeManager.OnLoadScene += LoadAsynchronously;
    }

    IEnumerator LoadAsynchronously(string _sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress * 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
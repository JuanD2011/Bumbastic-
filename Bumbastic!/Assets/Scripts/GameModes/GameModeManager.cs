using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] float timeToLoad = 10f;

    public delegate IEnumerator DelCanvasGameMode(string _sceneToLoad);
    public static DelCanvasGameMode onLoadScene;

    private void Start()
    {
        if (GameModeCanvas.ShowedInstructions)
            Invoke("SceneToLoad", timeToLoad); 
        else
            Invoke("SceneToLoad", timeToLoad + 5f); 
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips.transitionMusic, 0.3f, 0.6f, 5f);
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

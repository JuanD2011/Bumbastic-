using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] float timeToLoad = 8f;

    public delegate IEnumerator DelCanvasGameMode(string _sceneToLoad);
    public static DelCanvasGameMode OnLoadScene;

    private void Awake()
    {
        OnLoadScene = null;
    }

    private void Start()
    {
        Invoke("SceneToLoad", timeToLoad);
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips.transitionMusic, 0.3f, 0.6f, 5f);
    }

    private void SceneToLoad()
    {
        StartCoroutine(OnLoadScene?.Invoke(GameModeDataBase.currentGameMode.GameModeType.ToString()));
    }
}

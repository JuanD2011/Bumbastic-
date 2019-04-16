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
    }

    private void SceneToLoad()
    {
        StartCoroutine(OnLoadScene?.Invoke(GameModeDataBase.currentGameMode.gameModeType.ToString()));
    }
}

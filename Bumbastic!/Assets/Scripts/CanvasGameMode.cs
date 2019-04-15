using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class CanvasGameMode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameModeName, gamemodeDescription;
    [SerializeField] Image image;

    [SerializeField] float timeToLoad = 8f;

    public delegate IEnumerator DelCanvasGameMode(string _sceneToLoad);
    public static DelCanvasGameMode OnLoadScene;

    private void Awake()
    {
        OnLoadScene = null;
    }

    private void Start()
    {
        InitCanvas();
        Invoke("SceneToLoad", timeToLoad);
    }

    private void InitCanvas()
    {
        gameModeName.text = GameModeDataBase.currentGameMode.name;
        gamemodeDescription.text = GameModeDataBase.currentGameMode.description;
    }

    private void SceneToLoad()
    {
        StartCoroutine(OnLoadScene?.Invoke(GameModeDataBase.currentGameMode.gameModeType.ToString()));
    }
}

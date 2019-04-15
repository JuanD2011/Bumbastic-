using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasGameMode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameModeName, gamemodeDescription;
    [SerializeField] Image image;

    private void Start()
    {
        InitCanvas();
    }

    private void InitCanvas()
    {
        gameModeName.text = GameModeDataBase.currentGameMode.name;
        gamemodeDescription.text = GameModeDataBase.currentGameMode.description;
    }
}

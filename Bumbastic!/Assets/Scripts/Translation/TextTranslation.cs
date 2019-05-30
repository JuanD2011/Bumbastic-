using UnityEngine;
using TMPro;

public class TextTranslation : MonoBehaviour
{
    [SerializeField] string TextId = "";
    TextMeshProUGUI text = null;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (text != null)
            text.text = Translation.Fields[TextId];

        Translation.OnLoadedLanguage += UpdateText;
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = Translation.Fields[TextId];
    }
}

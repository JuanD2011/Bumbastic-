using UnityEngine;
using TMPro;

public class TextTranslation : MonoBehaviour
{
    [SerializeField] string textID = "";
    TextMeshProUGUI text = null;

    public string TextID { get => textID; set => textID = value; }

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (text != null)
            text.text = Translation.Fields[TextID];

        Translation.OnLoadedLanguage += UpdateText;
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = Translation.Fields[TextID];
    }
}

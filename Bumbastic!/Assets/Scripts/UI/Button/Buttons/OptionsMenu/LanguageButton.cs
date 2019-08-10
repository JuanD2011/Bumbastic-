using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : UIButtonBase
{
    [SerializeField] Settings settings = null;
    [SerializeField] Image flagImage = null;
    [SerializeField] Sprite[] flags = new Sprite[2];

    TextMeshProUGUI languageText = null;

    protected override void Awake()
    {
        base.Awake();
        languageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void Start()
    {
        base.Start();
        SetFeatures();
    }

    protected override void OnButtonClicked(byte _id)
    {
        base.OnButtonClicked(_id);

        if (interactuable)
        {
            ChangeLanguage();
            ClickSound(true);
        }
    }

    public void ChangeLanguage()
    {
        Translation.ChangeLanguage();
        SetFeatures();
    }

    private void SetFeatures()
    {
        switch (Translation.GetCurrentLanguage())
        {
            case Languages.en:
                languageText.text = Languages.en.ToString();
                flagImage.sprite = flags[0];
                break;
            case Languages.es:
                languageText.text = Languages.es.ToString();
                flagImage.sprite = flags[1];
                break;
            case Languages.unknown:
                break;
            default:
                break;
        }
        settings.languageID = Translation.currentLanguageId;
    }
}

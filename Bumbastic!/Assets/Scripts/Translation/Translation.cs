using System;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    en,
    es,
    unknown
};

public class Translation
{
    public static Dictionary<int, string> idToLanguage = new Dictionary<int, string>{ { 0, "en" }, { 1, "es" } };
    public static byte currentLanguageId = 0;

    public static Action OnLoadedLanguage;

    public static Dictionary<String, String> Fields { get; private set; }

    public static Languages GetCurrentLanguage()
    {
        Languages currentLanguage = Languages.unknown;

        switch (idToLanguage[currentLanguageId])
        {
            case "en":
                currentLanguage = Languages.en;
                break;
            case "es":
                currentLanguage = Languages.es;
                break;
            default:
                currentLanguage = Languages.en;
                break;
        }
        return currentLanguage;
    }

    public static void ChangeLanguage()
    {
        currentLanguageId++;

        if (currentLanguageId > idToLanguage.Count - 1) { currentLanguageId = 0; }

        LoadLanguage(idToLanguage[currentLanguageId]);
    }

    public static void LoadLanguage(string _Language)
    {
        if (Fields == null)
            Fields = new Dictionary<string, string>();

        Fields.Clear();
        string lang = _Language;
        var textAsset = Resources.Load(@"Languages/" + lang); //no .txt needed
        string allTexts = "";
        if (textAsset == null)
            textAsset = Resources.Load(@"Languages/en") as TextAsset; //no .txt needed
        if (textAsset == null)
            Debug.LogError("File not found for I18n: Assets/Resources/Languages/" + lang + ".txt");
        allTexts = (textAsset as TextAsset).text;
        string[] lines = allTexts.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        string key, value;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("#"))
            {
                key = lines[i].Substring(0, lines[i].IndexOf("="));
                value = lines[i].Substring(lines[i].IndexOf("=") + 1, lines[i].Length - lines[i].IndexOf("=") - 1).Replace("\\n", Environment.NewLine);
                Fields.Add(key, value);
            }
        }
        OnLoadedLanguage?.Invoke();
    }
}

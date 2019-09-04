using UnityEngine;
using System.Collections.Generic;

public class Localization : Singleton<Localization>
{
    [HideInInspector]
    public string currentLocalization;
    [HideInInspector]
    public Dictionary<string, string> locale;
    [HideInInspector]
    public List<LocalizatedObject> localizedObjects;

    protected override void Awake()
    {
        base.Awake();
        currentLocalization = LoadLocalization();
        locale = Locale.Read("Locales/" + currentLocalization);
    }

    public static string GetText(string key)
    {
        return Instance.locale[key] ?? string.Empty;
    }

    public static string GetLanguage(SystemLanguage lang)
    {
        switch (lang)
        {
            case SystemLanguage.English:    return "en_GB";
            case SystemLanguage.French:     return "fr_FR";
            case SystemLanguage.German:     return "de_DE";
            case SystemLanguage.Polish:     return "pl_PL";
            case SystemLanguage.Spanish:    return "es_ES";
            default:                        return "en_GB";
        }
    }

    public void SetLanguage(string l)
    {
        currentLocalization = l;
        locale = Locale.Read("Locales/" + currentLocalization);
        SaveLocalization(currentLocalization);

        foreach (LocalizatedObject o in localizedObjects)
            o.UpdateObject();
    }

    public static string LoadLocalization()
    {
        if (PlayerPrefs.HasKey("current_localization"))
            return PlayerPrefs.GetString("current_localization");

        return GetLanguage(Application.systemLanguage);
    }

    public static void SaveLocalization(string l)
    {
        if (l == string.Empty || l.Length != 5)
            l = "en_GB";

        PlayerPrefs.SetString("current_localization", l);
    }
}

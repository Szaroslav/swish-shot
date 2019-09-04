using UnityEngine;

public class UILanguages : MonoBehaviour
{
    public GameObject[] languages;

    private int index = 0;

    public void IncrementLanguage()
    {
        languages[index++].SetActive(false);
        if (index > languages.Length - 1) index = 0;
        languages[index].SetActive(true);

        Localization.Instance.SetLanguage(languages[index].GetComponent<UILanguage>().language);
    }

    public void DecrementLanguage()
    {
        languages[index--].SetActive(false);
        if (index < 0) index = languages.Length - 1;
        languages[index].SetActive(true);
        
        Localization.Instance.SetLanguage(languages[index].GetComponent<UILanguage>().language);
    }

    public void SetLanguage(SystemLanguage l)
    {
        Localization.Instance.SetLanguage(Localization.GetLanguage(l));
    }
}

using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocaleTMProUGUI : MonoBehaviour
{
    public string localeKey;

    private TextMeshProUGUI tmp;

    public void UpdateText()
    {
        tmp.text = Localization.GetText(localeKey);
    }

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        //Localization.Instance.localizedTexts.Add(this);
        UpdateText();
    }
}

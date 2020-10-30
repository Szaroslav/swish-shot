using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedTMP : LocalizatedObject
{
    private TextMeshProUGUI text;

    protected override void Start()
    {
        base.Start();

        if (!GetComponent<TextMeshProUGUI>())
            gameObject.AddComponent<TextMeshProUGUI>();
        text = GetComponent<TextMeshProUGUI>();

        UpdateObject();
    }

    public override void UpdateObject()
    {
        text.text = Localization.Instance.locale[key];
    }
}

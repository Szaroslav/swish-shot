using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UICoinsAnim : Anim
{
    [HideInInspector]
    public TextMeshProUGUI tmp;

    private float sourceValue;
    private float targetValue;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        enabled = false;
    }

    private void Update()
    {
        time += Time.unscaledDeltaTime / duration;

        tmp.text = ((int)Mathf.Lerp(sourceValue, targetValue, time)).ToString();

        if (time >= 1)
            enabled = false;
    }

    public void Set(float duration, float targetValue)
    {
        time = 0;
        sourceValue = Progress.Instance.coins;
        this.duration = duration;
        this.targetValue = targetValue;

        enabled = true;
    }
}

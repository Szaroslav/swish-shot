using UnityEngine;

public class UILanguage : MonoBehaviour
{
    public string language;

    private void Start()
    {
        gameObject.SetActive(Localization.Instance.currentLocalization == language ? true : false);
    }
}

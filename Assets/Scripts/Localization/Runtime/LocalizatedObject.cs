using UnityEngine;

public abstract class LocalizatedObject : MonoBehaviour
{
    public string key;

    protected virtual void Start()
    {
        Localization.Instance.localizedObjects.Add(this);
    }

    public abstract void UpdateObject();
}

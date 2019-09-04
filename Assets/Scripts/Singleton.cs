using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Object
{
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>();
                
                if (!instance)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(obj);
                }
            }

            return instance;
        }
    }

    private static T instance;

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}

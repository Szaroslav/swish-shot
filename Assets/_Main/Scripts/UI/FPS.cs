using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    public float refreshRate;

    private float time;

    private void Start()
    {
        
    }

    private void Update()
    {
        time += Time.unscaledDeltaTime;

        if (time >= refreshRate)
        {
            time = 0;
            GetComponent<TextMeshProUGUI>().text = ((int)(1 / Time.unscaledDeltaTime)).ToString();
        }
    }
}

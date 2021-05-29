using UnityEngine;
using System;
using System.Data;

public class Scenery : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer floor;

    void Start()
    {
        DateTime now = DateTime.Now;
        int h = now.Hour;
        int m = now.Month;

        string season;
        string night = "";

        if (h >= 21 || h <= 6)
            night = "-night";

        if (m >= 3 && m <= 9)
            season = "summer";
        else if (m >= 10 && m <= 11)
            season = "autumn";
        else
            season = "winter";

        background.sprite = Resources.Load<Sprite>($"Scenery/background-{season}{night}");
    }
}

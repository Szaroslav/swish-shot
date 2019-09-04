using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIAtlasImage : MonoBehaviour
{
    protected void Start()
    {
        Image img = GetComponent<Image>();
        img.sprite = Game.Instance.ui.uiAtlas.GetSprite(img.sprite.name);
    }
}

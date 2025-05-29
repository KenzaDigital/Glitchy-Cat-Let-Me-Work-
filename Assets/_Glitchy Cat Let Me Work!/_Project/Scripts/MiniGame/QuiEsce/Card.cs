using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;

    private bool isRevealed = false;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = backSprite;
    }

    public void OnClick()
    {
        isRevealed = !isRevealed;
        image.sprite = isRevealed ? frontSprite : backSprite;
    }
}

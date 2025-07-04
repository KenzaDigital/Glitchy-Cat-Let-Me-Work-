using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite frontSprite;  // L�image face visible de la carte
    public Sprite backSprite;   // L�image du dos de la carte

    private Image image;        // Composant UI Image
    private bool isRevealed = false;  // Carte retourn�e ?

    private void Start()
    {
        image = GetComponent<Image>();
        ShowBack(); // Affiche le dos au d�marrage
    }

    // Appel� quand on clique sur la carte
    public void OnClick()
    {
        if (isRevealed || !CardManager.Instance.CanReveal()) return;

        Reveal();
        CardManager.Instance.CardRevealed(this);
    }

    // Retourne la carte (affiche la face)
    public void Reveal()
    {
        isRevealed = true;
        image.sprite = frontSprite;
    }

    // Cache la carte (affiche le dos)
    public void HideCard()
    {
        isRevealed = false;
        ShowBack();
    }

    // Affiche le dos
    private void ShowBack()
    {
        if (image != null)
            image.sprite = backSprite;
    }

    // Utilis� par le CardManager pour comparer
    public Sprite GetFrontSprite()
    {
        return frontSprite;
    }

    // Pour savoir si la carte est d�j� r�v�l�e
    public bool IsRevealed()
    {
        return isRevealed;
    }
}

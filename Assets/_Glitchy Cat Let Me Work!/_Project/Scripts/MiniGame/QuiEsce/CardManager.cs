using System.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private Card firstCard;
    private Card secondCard;

    private bool canReveal = true;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanReveal()
    {
        return canReveal;
    }

    public void CardRevealed(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canReveal = false;

        yield return new WaitForSeconds(1f);

        if (firstCard.GetFrontSprite() == secondCard.GetFrontSprite())
        {
            // Cartes identiques – on les laisse affichées
        }
        else
        {
            // Pas identiques – on les cache
            firstCard.HideCard();
            secondCard.HideCard();
        }

        firstCard = null;
        secondCard = null;
        canReveal = true;
    }
}

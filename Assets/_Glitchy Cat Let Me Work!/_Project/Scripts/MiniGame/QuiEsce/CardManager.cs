using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            // ✅ Bonne paire
        }
        else
        {
            // ❌ Mauvaise paire
            firstCard.HideCard();
            secondCard.HideCard();

            // 🔊 Son erreur
            audioManager.instance.PlaySFX("Wrong");

            // ⬇️ Retirer de la productivité
            ProductivityManager.Instance?.RemoveProductivity(2);

            // 🔴 Si productivité à zéro => Game Over
            if (ProductivityManager.Instance != null &&
                ProductivityManager.Instance.GetCurrentProductivity() <= 0)
            {
                StartCoroutine(ShowFeedback("Tes clients se fâchent 😡", Color.red));
                GameOver();
                yield break;
            }
        }

        firstCard = null;
        secondCard = null;
        canReveal = true;
    }

    private void GameOver()
    {
        Debug.Log("Game Over – plus de productivité !");
        canReveal = false;
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator ShowFeedback(string message, Color color)
    {
       
       
        GameObject feedbackGO = GameObject.Find("FeedbackText"); // ← nom de ton TextMeshProUGUI
        if (feedbackGO != null)
        {
            var tmp = feedbackGO.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = message;
            tmp.color = color;
            tmp.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            tmp.gameObject.SetActive(false);
        }
    }
}

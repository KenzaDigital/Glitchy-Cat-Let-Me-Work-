using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private Card firstCard;
    private Card secondCard;
    private bool canReveal = true;

    [SerializeField]
    private int totalPairs = 10; //nombre de paires totale

    private int pairsFound = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        audioManager.instance.PlayMusic("QuiEsce", true);
        MiniGameManager.Instance.SetCurrentMiniGame(MiniGameType.FidéliseTesClients);
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
            pairsFound++;
            Debug.Log($"Paires trouvées : {pairsFound}/{totalPairs}");

            if (pairsFound >= totalPairs)
            {
                Debug.Log("🎉 Toutes les paires sont trouvées ! Victoire dans FidéliseTesClients.");

                // ✅ On enregistre la réussite du mini-jeu
                MiniGameManager.Instance?.SetCurrentMiniGame(MiniGameType.FidéliseTesClients);
                ToDoListManager.Instance?.MarkTaskCompletedByName("fidelistesclients");
                ToDoListManager.Instance?.SaveCompletedTasks();

                // 🎁 Récompense de productivité
                ProductivityManager.Instance?.AddProductivity(10);

                yield return new WaitForSeconds(1f); // petite pause avant retour

                SceneManager.LoadScene("MainScene");
                yield break;
            }
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
                StartCoroutine(ShowFeedback("Tes clients se fâchent ", Color.red));
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
        audioManager.instance.StopMusic();

        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator ShowFeedback(string message, Color color)
    {
        GameObject feedbackGO = GameObject.Find("FeedbackText"); // Nom exact du TextMeshProUGUI dans la scène
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

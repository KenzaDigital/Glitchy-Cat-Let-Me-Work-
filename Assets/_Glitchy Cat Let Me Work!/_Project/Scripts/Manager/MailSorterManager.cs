using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MailSorterManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI mailText;
    public Button proButton;
    public Button spamButton;
    public Canvas CanvasMail;
    public Button openMailButton;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;

    [Header("Données")]
    public MailData[] mails;

    [Header("Réglages")]
    public float timeToAnswer = 10f;
    public int maxLives = 3;

    private int currentIndex = 0;
    private int lives;
    private float timer;
    private bool mailActive = false;
    private Coroutine timerCoroutine;

    public ToDoListManager todoListManager;

    void Start()
    {
        lives = maxLives;
        livesText.text = "Vies : " + lives;

        CanvasMail.gameObject.SetActive(false);
        ResetCanvasMail();

        if (mails.Length == 0)
        {
            Debug.LogError("Aucun mail assigné !");
            return;
        }

        ShowMail();

        openMailButton.onClick.AddListener(OpenMail);
        proButton.onClick.AddListener(() => SortMail(true));
        spamButton.onClick.AddListener(() => SortMail(false));
    }

    void ResetCanvasMail()
    {
        feedbackText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    void ShowMail()
    {
        if (currentIndex >= mails.Length)
        {
            mailText.text = "Tous les mails sont triés !";
            proButton.interactable = false;
            spamButton.interactable = false;
            timerText.gameObject.SetActive(false);
            CanvasMail.gameObject.SetActive(false);
            openMailButton.interactable = false;

            Debug.Log("🟢 Tous les mails sont finis. Tentative de marquer la tâche comme complétée...");
            todoListManager?.MarkTaskCompletedByName("Trier les mails");
            return;
        }

        mailText.text = mails[currentIndex].content;
        proButton.interactable = true;
        spamButton.interactable = true;
    }

    void StartTimer()
    {
        mailActive = true;
        timer = timeToAnswer;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerCountdown());

        timerText.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        timerText.text = " " + timeToAnswer.ToString("0");
    }

    IEnumerator TimerCountdown()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = " " + Mathf.CeilToInt(timer).ToString();
            yield return null;
        }

        timerText.text = "0";

        if (mailActive)
        {
            Debug.Log("Temps écoulé sans réponse !");
            OnWrongAnswer();
        }
    }

    void SortMail(bool userSaysPro)
    {
        if (!mailActive) return;

        mailActive = false;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerText.gameObject.SetActive(false);

        bool mailIsPro = mails[currentIndex].isPro;
        bool isCorrect = mailIsPro == userSaysPro;

        if (isCorrect)
        {
            StartCoroutine(ShowFeedback("Yeah !", Color.green));
            ProductivityManager.Instance?.AddProductivity(10);
        }
        else
        {
            OnWrongAnswer();
            return;
        }

        currentIndex++;
        StartCoroutine(NextMailDelay());
    }

    void OnWrongAnswer()
    {
        mailActive = false;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerText.gameObject.SetActive(false);

        lives--;
        livesText.text = "Vies : " + lives;
        StartCoroutine(ShowFeedback("Bouuuuh...", Color.red));
        ProductivityManager.Instance?.RemoveProductivity(10);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            currentIndex++;
            StartCoroutine(NextMailDelay());
        }
    }

    IEnumerator ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        yield return new WaitForSeconds(1.5f);
        feedbackText.text = "";
    }

    IEnumerator NextMailDelay()
    {
        yield return new WaitForSeconds(0.3f);
        CanvasMail.gameObject.SetActive(false);
        openMailButton.interactable = true;
        ShowMail();
    }

    void GameOver()
    {
        Debug.Log("Game Over !");
        mailText.text = "Game Over !";
        proButton.interactable = false;
        spamButton.interactable = false;
        timerText.gameObject.SetActive(false);
        CanvasMail.gameObject.SetActive(false);
        openMailButton.interactable = false;
    }

    public void OpenMail()
    {
        CanvasMail.gameObject.SetActive(true);
        ResetCanvasMail();
        openMailButton.interactable = false;
        Debug.Log("Mail ouvert !");
        StartTimer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasMail.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            feedbackText.gameObject.SetActive(false);
            openMailButton.interactable = true;
            Debug.Log("Mail fermé !");
        }
    }
}

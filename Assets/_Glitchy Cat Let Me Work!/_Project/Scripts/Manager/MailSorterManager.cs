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
        UpdateLivesUI();

        CanvasMail.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        if (mails == null || mails.Length == 0)
        {
            Debug.LogError("Aucun mail assigné !");
            openMailButton.interactable = false;
            return;
        }

        ShowMail();

        openMailButton.onClick.AddListener(OpenMail);
        proButton.onClick.AddListener(() => SortMail(true));
        spamButton.onClick.AddListener(() => SortMail(false));
    }

    void UpdateLivesUI()
    {
        livesText.text = "Vies : " + lives;
    }

    void ShowMail()
    {
        if (currentIndex >= mails.Length)
        {
            mailText.text = "Tous les mails sont triés !";
            proButton.interactable = false;
            spamButton.interactable = false;
            timerText.gameObject.SetActive(false);
            feedbackText.gameObject.SetActive(false);
            CanvasMail.gameObject.SetActive(false);
            openMailButton.interactable = false;

            todoListManager?.MarkTaskCompletedByName("Trier les mails");

            mailActive = false;
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);
            return;
        }

        mailText.text = mails[currentIndex].content;
        proButton.interactable = true;
        spamButton.interactable = true;

        feedbackText.gameObject.SetActive(false);  // On cache le feedback à chaque nouveau mail
        timerText.gameObject.SetActive(false);     // On cache le timer avant d’ouvrir le mail
    }

    void StartTimer()
    {
        mailActive = true;
        timer = timeToAnswer;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerCountdown());

        timerText.gameObject.SetActive(true);
        timerText.text = timer.ToString("0");
    }

    IEnumerator TimerCountdown()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timer).ToString();
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
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerText.gameObject.SetActive(false);

        bool mailIsPro = mails[currentIndex].isPro;
        bool isCorrect = mailIsPro == userSaysPro;

        if (isCorrect)
        {
            StartCoroutine(ShowFeedbackThenNextMail("Yeah !", Color.green));
            ProductivityManager.Instance?.AddProductivity(10);
        }
        else
        {
            StartCoroutine(ShowFeedbackThenNextMail("Bouuuuh...", Color.red, isWrong: true));
        }
    }

    IEnumerator ShowFeedbackThenNextMail(string message, Color color, bool isWrong = false)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        feedbackText.gameObject.SetActive(false);

        if (isWrong)
        {
            lives--;
            UpdateLivesUI();

            ProductivityManager.Instance?.RemoveProductivity(10);

            if (lives <= 0)
            {
                GameOver();
                yield break;
            }
        }

        currentIndex++;
        ShowMail();
        OpenMail();  // relance le canvas + timer
    }

    void OnWrongAnswer()
    {
        // Ne plus rien faire ici, on utilise le coroutine ShowFeedbackThenNextMail avec isWrong = true
    }

    void GameOver()
    {
        Debug.Log("Game Over !");
        mailText.text = "Game Over !";
        proButton.interactable = false;
        spamButton.interactable = false;
        timerText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        CanvasMail.gameObject.SetActive(false);
        openMailButton.interactable = false;
        mailActive = false;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    public void OpenMail()
    {
        if (currentIndex >= mails.Length || lives <= 0) return;

        CanvasMail.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        openMailButton.interactable = false;

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
            mailActive = false;

            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            Debug.Log("Mail fermé !");
        }
    }
}

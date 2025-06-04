using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PongGameManager : MonoBehaviour
{
    [Header("Scores")]
    public int maxScore = 3;
    private int leftScore = 0;
    private int rightScore = 0;

    [Header("UI")]
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;

    private void Start()
    {
        MiniGameManager.Instance.SetCurrentMiniGame(MiniGameType.PongMeeting);  
        UpdateScoreUI();

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        audioManager.instance.PlayMusic("PongMeeting", true);
    }

    public void ScorePoint(string wallName)
    {
        if (wallName == "WallLeft")
            rightScore++;
        else if (wallName == "WallRight")
            leftScore++;

        UpdateScoreUI();
        CheckVictory();
    }

    void UpdateScoreUI()
    {
        if (leftScoreText != null)
            leftScoreText.text = leftScore.ToString();

        if (rightScoreText != null)
            rightScoreText.text = rightScore.ToString();
    }

    void CheckVictory()
    {
        if (leftScore >= maxScore)
        {
            EndGame(true, " PUNCHLINE! "); // victoire joueur gauche
            StartCoroutine(WaitAndLoad("VictoryScene"));
        }
        else if (rightScore >= maxScore)
        {
            EndGame(false, "IL T'A PUNCHLINE!"); // défaite joueur gauche
        }
    }

    void EndGame(bool playerWon, string message)
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (victoryText != null)
            victoryText.text = message;

        if (playerWon)
        {
            ToDoListManager.Instance?.MarkTaskCompletedByName("meeting"); // 
            ProductivityManager.Instance?.AddProductivity(10);
        }
        else
        {
            ProductivityManager.Instance?.RemoveProductivity(15);

            if (ProductivityManager.Instance.GetCurrentProductivity() <= 0)
            {
                // Game over global si productivité à zéro
                Invoke(nameof(GoToGameOver), 3f);
                return;
            }
        }

        Invoke(nameof(BackToMainScene), 3f);
    }

    void BackToMainScene()
    {
        audioManager.instance.StopMusic(); // Arrête la musique
        audioManager.instance.PlaySFX("Achievement"); // Son de victoire
        SceneManager.LoadScene("MainScene");
    }

    void GoToGameOver()
    {
        audioManager.instance.StopMusic();
        SceneManager.LoadScene("GameOverScene");
    }
    private IEnumerator WaitAndLoad(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
}

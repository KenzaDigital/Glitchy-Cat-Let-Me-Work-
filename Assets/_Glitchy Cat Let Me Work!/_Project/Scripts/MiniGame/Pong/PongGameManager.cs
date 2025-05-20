using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        UpdateScoreUI();

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
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
            EndGame("Joueur Gauche gagne !");
        }
        else if (rightScore >= maxScore)
        {
            EndGame("Joueur Droit gagne !");
        }
    }

    void EndGame(string message)
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (victoryText != null)
            victoryText.text = message;

        // Marque la tâche dans la ToDoList si elle existe
        if (ToDoListManager.Instance != null)
            ToDoListManager.Instance.MarkTaskCompletedByName("PongMeeting");

        Invoke(nameof(BackToMainScene), 3f);
    }

    void BackToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SandwichEater : MonoBehaviour
{
    public Button tapButton;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI successText;
    public GameObject sandwichDonePanel; // Le panel complet à cacher à la fin

    public int tapsToEat = 10;
    private int currentTaps = 0;
    private bool finished = false;

    void Start()
    {

        MiniGameManager.Instance.SetCurrentMiniGame(MiniGameType.PauseDejeuner);

        counterText.text = $"Crocs : {currentTaps}/{tapsToEat}";
        successText.gameObject.SetActive(false);
        tapButton.onClick.AddListener(OnTap);
    }

    void OnTap()
    {
        if (finished) return;

        audioManager.instance.PlaySFX("Crunch");
        currentTaps++;
        tapButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 1);
        counterText.text = $"Crocs: {currentTaps}/{tapsToEat}";

        if (currentTaps >= tapsToEat)
        {
            FinishEating();
        }
    }

    void FinishEating()
    {
        finished = true;
        tapButton.interactable = false;
        successText.gameObject.SetActive(true);
        successText.text = "😋 Miam ! Sandwich mangé !";

        Sequence endSequence = DOTween.Sequence();

        // Animation de disparition du sandwich panel
        endSequence.Append(sandwichDonePanel.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        endSequence.AppendCallback(() =>
        {
            sandwichDonePanel.SetActive(false); // le cacher complètement

            // Enregistre le mini-jeu courant
            MiniGameManager.Instance?.SetCurrentMiniGame(MiniGameType.PauseDejeuner);

            // Marque la tâche comme terminée
            Debug.Log("Tâche Pause déjeuner marquée comme accomplie !");
            audioManager.instance.PlaySFX("Achievement");

           
            // Enregistre les tâches complétées
            ToDoListManager.Instance?.SaveCompletedTasks();
            // Retour au menu
            SceneManager.LoadScene("MainScene");
            Debug.Log("BareLenomPauseDEJ");
            ToDoListManager.Instance?.MarkTaskCompletedByName("pausedejeuner");

        });
    }
}

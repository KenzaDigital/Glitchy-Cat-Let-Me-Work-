using UnityEngine;

public class ToDoListUIManager : MonoBehaviour
{
    void Start()
    {
        if (ToDoListManager.Instance != null)
        {
            ToDoListManager.Instance.SyncTasksState();
        }

        if (MiniGameManager.Instance != null)
        {
            MiniGameType lastGame = MiniGameManager.Instance.GetCurrentMiniGame();

            if (lastGame != MiniGameType.None)
            {
                string taskKey = GetTaskKeyFromMiniGame(lastGame);

                if (!string.IsNullOrEmpty(taskKey))
                {
                    ToDoListManager.Instance.MarkTaskCompletedByName(taskKey);
                    ToDoListManager.Instance.SaveCompletedTasks();

                    MiniGameManager.Instance.ClearCurrentMiniGame();

                    if (ToDoListManager.Instance.AreTasksForCurrentStepCompleted())
                    {
                        Debug.Log("✅ Toutes les tâches complétées, passage à l'étape suivante !");
                        GameManager.Instance?.NextDayStep();
                    }
                }
                else
                {
                    Debug.LogWarning($"⚠️ Aucune correspondance de tâche trouvée pour le mini-jeu : {lastGame}");
                }
            }
        }
    }

    private string GetTaskKeyFromMiniGame(MiniGameType miniGame)
    {
        switch (miniGame)
        {
            case MiniGameType.TriDeMail:
                return "trierlesmails";
            case MiniGameType.FichierCrush:
                return "classerlesfichiers";
            case MiniGameType.PauseDejeuner:
                return "pausedejeuner";
            case MiniGameType.FidéliseTesClients:
                return "fidelistesclients";
            case MiniGameType.PongMeeting:
                return "meeting";
            default:
                return null;
        }
    }
}

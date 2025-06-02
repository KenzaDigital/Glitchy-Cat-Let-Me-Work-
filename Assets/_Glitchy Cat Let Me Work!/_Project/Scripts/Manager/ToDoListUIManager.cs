using UnityEngine;

public class ToDoListUIManager : MonoBehaviour
{
    void Start()
    {
        if (ToDoListManager.Instance != null)
        {
            // Synchronise l'état des tâches depuis les données sauvegardées
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
                    // Marque la tâche comme complétée et sauvegarde
                    ToDoListManager.Instance.MarkTaskCompletedByName(taskKey);
                    ToDoListManager.Instance.SaveCompletedTasks();

                    // 🔄 Recharge les états des tâches depuis les données sauvegardées
                    ToDoListManager.Instance.SyncTasksState();

                    // Réinitialise le mini-jeu actif
                    MiniGameManager.Instance.ClearCurrentMiniGame();

                    // ✅ Vérifie si toutes les tâches de l'étape sont complétées
                    if (ToDoListManager.Instance.AreTasksForCurrentStepCompleted())
                    {
                        Debug.Log("✅ Toutes les tâches complétées, passage à l'étape suivante !");
                        GameManager.Instance?.NextDayStep();
                    }
                    else
                    {
                        Debug.Log("🔁 Certaines tâches restent à faire pour cette étape.");
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

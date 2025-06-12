using UnityEngine;

public class ToDoListUIManager : MonoBehaviour
{
    // Appelle cette fonction quand un mini-jeu est fini, avec le type du mini-jeu
    public void OnMiniGameCompleted(MiniGameType miniGame)
    {
        string taskKey = GetTaskKeyFromMiniGame(miniGame);

        if (!string.IsNullOrEmpty(taskKey))
        {
            ToDoListManager.Instance.MarkTaskCompletedByName(taskKey);
            ToDoListManager.Instance.SaveCompletedTasks();
            ToDoListManager.Instance.SyncTasksState();

            MiniGameManager.Instance.ClearCurrentMiniGame();
        }
        else
        {
            Debug.LogWarning($"⚠️ Aucune correspondance de tâche trouvée pour le mini-jeu : {miniGame}");
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

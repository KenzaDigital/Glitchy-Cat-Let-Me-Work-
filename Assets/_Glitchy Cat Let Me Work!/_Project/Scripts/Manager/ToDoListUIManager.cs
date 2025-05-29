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
                // Marquer la tâche comme complétée
                ToDoListManager.Instance.MarkTaskCompletedByName(lastGame.ToString());
                ToDoListManager.Instance.SaveCompletedTasks();

                MiniGameManager.Instance.ClearCurrentMiniGame();

                // Vérifie si toutes les tâches du moment sont finies
                if (ToDoListManager.Instance.AreTasksForCurrentStepCompleted())
                {

                    Debug.Log("Toutes les tâches complétées, passage à l'étape suivante !");
                    GameManager.Instance?.NextDayStep();
                }
            }
        }
    }
}

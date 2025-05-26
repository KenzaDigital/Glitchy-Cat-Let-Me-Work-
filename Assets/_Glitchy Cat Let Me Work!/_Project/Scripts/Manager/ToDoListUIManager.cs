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
                
                ToDoListManager.Instance.MarkTaskCompletedByName(lastGame.ToString());
                ToDoListManager.Instance.SaveCompletedTasks();

                MiniGameManager.Instance.ClearCurrentMiniGame();
            }
        }
    }
}

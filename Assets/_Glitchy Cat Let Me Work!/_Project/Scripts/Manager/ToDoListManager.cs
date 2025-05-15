using UnityEngine;
using System.Collections.Generic;

public class ToDoListManager : MonoBehaviour
{
    public static ToDoListManager Instance;

    [Header("Liste des tâches")]
    public List<TaskItem> tasks = new List<TaskItem>();

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Marque la tâche à l’index donné comme complétée.
    /// </summary>
    public void MarkTaskCompletedByName(string taskName)
    {
        foreach (var task in tasks)
        {
            if (task.taskText == taskName)
            {
                task.CompleteTask();
                Debug.Log($"✅ Tâche complétée : {taskName}");
                return;
            }
        }

        Debug.LogWarning($"❌ Aucune tâche trouvée avec le nom : {taskName}");
    }
}

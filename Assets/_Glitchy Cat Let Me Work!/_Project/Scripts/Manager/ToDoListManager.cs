using UnityEngine;
using System.Collections.Generic;

public class ToDoListManager : MonoBehaviour
{
    public static ToDoListManager Instance;

    [Header("Liste des tâches")]
    public List<TaskItem> tasks = new List<TaskItem>();

    // Stocke les noms des tâches complétées
    private HashSet<string> completedTasks = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkTaskCompletedByName(string taskName)
    {
        if (completedTasks.Contains(taskName))
            return; // Déjà complété

        foreach (var task in tasks)
        {
            if (task.taskText.Trim().ToLower() == taskName.Trim().ToLower())
            {
                task.CompleteTask();
                completedTasks.Add(taskName);
                return;
            }
        }
    }

    // Appelle cette méthode lorsque la scène est chargée, pour synchroniser l’état des tâches
    public void SyncTasksState()
    {
        foreach (var task in tasks)
        {
            if (completedTasks.Contains(task.taskText))
            {
                task.CompleteTask();
            }
        }
    }
}

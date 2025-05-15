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

    public void MarkTaskCompletedByName(string taskName)
    {
       // Debug.Log(" Recherche de la tâche : " + taskName);
        foreach (var task in tasks)
        {
           // Debug.Log("Tâche dans la liste : " + task.taskText);
            if (task.taskText.Trim().ToLower() == taskName.Trim().ToLower())
            {
                task.CompleteTask();
               // Debug.Log($"✅ Tâche complétée : {taskName}");
                return;
            }
        }

        //Debug.LogWarning($"❌ Aucune tâche trouvée avec le nom : {taskName}");
    }
}
